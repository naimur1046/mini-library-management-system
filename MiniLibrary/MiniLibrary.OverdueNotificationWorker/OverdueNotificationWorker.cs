using MiniLibrary.Application.Abstractions.Email;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Borrowings.GetOverdue;

namespace MiniLibrary.OverdueNotificationWorker;

public sealed class OverdueNotificationWorker : BackgroundService
{
    private readonly ILogger<OverdueNotificationWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _scheduledTime;

    public OverdueNotificationWorker(
        ILogger<OverdueNotificationWorker> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        // Read the scheduled time from configuration (default: 09:00)
        var scheduledHour = configuration.GetValue<int>("OverdueNotification:ScheduledHour", 9);
        var scheduledMinute = configuration.GetValue<int>("OverdueNotification:ScheduledMinute", 0);
        _scheduledTime = new TimeSpan(scheduledHour, scheduledMinute, 0);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Overdue Notification Worker starting. Scheduled to run daily at {ScheduledTime}",
            _scheduledTime.ToString(@"hh\:mm"));

        while (!stoppingToken.IsCancellationRequested)
        {
            var nextRunTime = GetNextRunTime();
            var delay = nextRunTime - DateTime.Now;

            _logger.LogInformation(
                "Next notification check scheduled for {NextRunTime} (in {DelayHours:F2} hours)",
                nextRunTime.ToString("yyyy-MM-dd HH:mm:ss"),
                delay.TotalHours);

            try
            {
                await Task.Delay(delay, stoppingToken);

                _logger.LogInformation("Starting scheduled overdue notification check...");
                await ProcessOverdueNotificationsAsync(stoppingToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Worker cancellation requested");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing overdue notifications");
            }
        }

        _logger.LogInformation("Overdue Notification Worker stopping");
    }

    private DateTime GetNextRunTime()
    {
        var now = DateTime.Now;
        var scheduledToday = now.Date.Add(_scheduledTime);

        if (now >= scheduledToday)
        {
            // If the scheduled time has passed today, schedule for tomorrow
            return scheduledToday.AddDays(1);
        }

        return scheduledToday;
    }

    private async Task ProcessOverdueNotificationsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var queryHandler = scope.ServiceProvider
            .GetRequiredService<IQueryHandler<GetOverdueBorrowingsQuery, List<OverdueBorrowingResponse>>>();

        var emailService = scope.ServiceProvider
            .GetRequiredService<IEmailService>();

        _logger.LogInformation("Checking for overdue borrowings...");

        var result = await queryHandler.Handle(new GetOverdueBorrowingsQuery(), cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to get overdue borrowings: {Error}", result.Error.Description);
            return;
        }

        var overdueBorrowings = result.Value;

        if (overdueBorrowings.Count == 0)
        {
            _logger.LogInformation("No overdue borrowings found");
            return;
        }

        _logger.LogInformation("Found {Count} overdue borrowings", overdueBorrowings.Count);

        foreach (var borrowing in overdueBorrowings)
        {
            await SendOverdueNotificationAsync(borrowing, emailService, cancellationToken);
        }

        _logger.LogInformation("Completed processing overdue notifications");
    }

    private async Task SendOverdueNotificationAsync(
        OverdueBorrowingResponse borrowing,
        IEmailService emailService,
        CancellationToken cancellationToken)
    {
        try
        {
            var subject = $"Overdue Book Return Reminder - {borrowing.DaysOverdue} days overdue";

            var booksList = string.Join("\n", borrowing.OverdueBooks.Select(b =>
                $"  - {b.BookTitle} by {b.BookAuthor}"));

            var body = $@"Dear {borrowing.MemberName},

This is a reminder that you have overdue books that need to be returned.

Due Date: {borrowing.DueDate:yyyy-MM-dd}
Days Overdue: {borrowing.DaysOverdue}

Overdue Books:
{booksList}

Please return these books to the library as soon as possible.

Thank you,
Mini Library Management System";

            var success = await emailService.SendEmailAsync(
                borrowing.MemberEmail,
                subject,
                body,
                "OverdueNotification",
                borrowing.BorrowId,
                cancellationToken);

            if (success)
            {
                _logger.LogInformation(
                    "Sent overdue notification to {MemberName} ({Email}) for {Count} book(s)",
                    borrowing.MemberName,
                    borrowing.MemberEmail,
                    borrowing.OverdueBooks.Count);
            }
            else
            {
                _logger.LogWarning(
                    "Failed to send overdue notification to {MemberName} ({Email})",
                    borrowing.MemberName,
                    borrowing.MemberEmail);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error sending notification to {MemberName} ({Email})",
                borrowing.MemberName,
                borrowing.MemberEmail);
        }
    }
}

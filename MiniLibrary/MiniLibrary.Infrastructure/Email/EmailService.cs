using Domain.EmailLogs;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Email;
using SharedKernel;

namespace MiniLibrary.Infrastructure.Email;

internal sealed class EmailService(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IOptions<EmailSettings> emailSettings,
    ILogger<EmailService> logger) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public async Task<bool> SendEmailAsync(
        string recipientEmail,
        string subject,
        string body,
        string emailType,
        Guid? relatedEntityId = null,
        CancellationToken cancellationToken = default)
    {
        bool isSuccess = false;
        string? errorMessage = null;

        try
        {
            logger.LogInformation(
                "Sending email to {RecipientEmail} with subject: {Subject}",
                recipientEmail,
                subject);

            if (_emailSettings.EnableEmailSending)
            {
                await SendEmailViaSmtpAsync(recipientEmail, subject, body, cancellationToken);
                isSuccess = true;

                logger.LogInformation(
                    "Email sent successfully to {RecipientEmail}",
                    recipientEmail);
            }
            else
            {
                logger.LogInformation(
                    "Email sending is disabled. Email to {RecipientEmail} was NOT sent (simulation mode)",
                    recipientEmail);

                // In simulation mode, still mark as success for testing purposes
                isSuccess = true;
            }
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            logger.LogError(
                ex,
                "Failed to send email to {RecipientEmail}",
                recipientEmail);
        }
        finally
        {
            // Log the email attempt to the database
            var emailLog = new EmailLog
            {
                Id = Guid.NewGuid(),
                RecipientEmail = recipientEmail,
                Subject = subject,
                Body = body,
                SentDate = dateTimeProvider.UtcNow,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                EmailType = emailType,
                RelatedEntityId = relatedEntityId
            };

            context.EmailLogs.Add(emailLog);
            await context.SaveChangesAsync(cancellationToken);
        }

        return isSuccess;
    }

    private async Task SendEmailViaSmtpAsync(
        string recipientEmail,
        string subject,
        string body,
        CancellationToken cancellationToken)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
        message.To.Add(new MailboxAddress(string.Empty, recipientEmail));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _emailSettings.SmtpHost,
            _emailSettings.SmtpPort,
            _emailSettings.EnableSsl,
            cancellationToken);

        if (!string.IsNullOrEmpty(_emailSettings.Username) && !string.IsNullOrEmpty(_emailSettings.Password))
        {
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password, cancellationToken);
        }

        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}

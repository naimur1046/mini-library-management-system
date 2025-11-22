namespace MiniLibrary.Application.Abstractions.Email;

public interface IEmailService
{
    Task<bool> SendEmailAsync(
        string recipientEmail,
        string subject,
        string body,
        string emailType,
        Guid? relatedEntityId = null,
        CancellationToken cancellationToken = default);
}

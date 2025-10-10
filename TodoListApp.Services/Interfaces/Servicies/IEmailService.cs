namespace TodoListApp.Services.Interfaces.Servicies;

/// <summary>
/// Service interface for sending emails.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="senderName">The sender name.</param>
    /// <param name="toName">The reciever name.</param>
    /// <param name="toEmail">The reciever email.</param>
    /// <param name="subject">The mail subject.</param>
    /// <param name="content">The mail content.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendEmailAsync(
        string senderName,
        string toName,
        string toEmail,
        string subject,
        string content,
        CancellationToken cancellationToken = default);
}

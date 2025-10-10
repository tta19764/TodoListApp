using MailKit.Net.Smtp;
using MimeKit;
using TodoListApp.Services.Email;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.WebApp.CustomLogs;

namespace TodoListApp.WebApp.Services;

/// <summary>
/// Service for sending emails using SMTP.
/// </summary>
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> logger;
    private readonly EmailSender sender;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="sender">The sender configuration.</param>
    /// <exception cref="ArgumentNullException">Thrown when logger or sender is null.</exception>
    public EmailService(ILogger<EmailService> logger, EmailSender sender)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="senderName">Sender name.</param>
    /// <param name="toName">Reciever name.</param>
    /// <param name="toEmail">Reciever email.</param>
    /// <param name="subject">The mail subject.</param>
    /// <param name="content">The mail content.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendEmailAsync(string senderName, string toName, string toEmail, string subject, string content, CancellationToken cancellationToken = default)
    {
        using var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderName, this.sender.From));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = content,
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(this.sender.SmtpServer, this.sender.Port, cancellationToken: cancellationToken);
        await client.AuthenticateAsync(this.sender.UserName, this.sender.Password, cancellationToken: cancellationToken);

        try
        {
            var result = await client.SendAsync(message, cancellationToken);
            if (result != null)
            {
                AccountLog.LogPasswordResetEmailSent(this.logger, this.sender.From);
            }

            await client.DisconnectAsync(true, cancellationToken);
        }
        catch (Exception ex)
        {
            AccountLog.LogPasswordResetFailedWithErrors(this.logger, this.sender.From, ex.Message);
            throw;
        }
    }
}

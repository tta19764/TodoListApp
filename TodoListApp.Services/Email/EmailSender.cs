namespace TodoListApp.Services.Email;

/// <summary>
/// Email sender configuration class.
/// </summary>
public class EmailSender
{
    /// <summary>
    /// Gets or sets the email address from which emails will be sent.
    /// </summary>
    public string From { get; set; } = null!;

    /// <summary>
    /// Gets or sets the SMTP server address.
    /// </summary>
    public string SmtpServer { get; set; } = null!;

    /// <summary>
    /// Gets or sets the port number for the SMTP server.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets the username for authenticating with the SMTP server.
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password for authenticating with the SMTP server.
    /// </summary>
    public string Password { get; set; } = null!;
}

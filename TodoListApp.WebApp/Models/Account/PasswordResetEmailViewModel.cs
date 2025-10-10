namespace TodoListApp.WebApp.Models.Account;

/// <summary>
/// View model for password reset email template.
/// </summary>
public class PasswordResetEmailViewModel
{
    /// <summary>
    /// Gets or sets user's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets password reset link.
    /// </summary>
    public string ResetLink { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets expiration time in hours.
    /// </summary>
    public int ExpirationHours { get; set; } = 24;
}

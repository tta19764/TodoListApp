namespace TodoListApp.Services.JWT;

/// <summary>
/// Data Transfer Object for user credentials.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

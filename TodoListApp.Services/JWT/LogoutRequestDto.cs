namespace TodoListApp.Services.JWT;

/// <summary>
/// Data Transfer Object for logout requests.
/// </summary>
public class LogoutRequestDto
{
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
}

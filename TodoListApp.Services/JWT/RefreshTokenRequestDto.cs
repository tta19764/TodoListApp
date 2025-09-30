namespace TodoListApp.Services.JWT;

/// <summary>
/// Data Transfer Object for refresh token requests.
/// </summary>
public class RefreshTokenRequestDto
{
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    public string RefreshToken { get; set; } = null!;
}

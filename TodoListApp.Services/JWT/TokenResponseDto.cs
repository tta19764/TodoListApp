namespace TodoListApp.Services.JWT;

/// <summary>
/// Data Transfer Object for token responses.
/// </summary>
public class TokenResponseDto
{
    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    public string RefreshToken { get; set; } = null!;
}

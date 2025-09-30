namespace TodoListApp.Services.JWT;

/// <summary>
/// Payload for refresh token.
/// </summary>
public class RefreshTokenPayload
{
    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the expiry date and time of the refresh token.
    /// </summary>
    public DateTime Expiry { get; set; }
}

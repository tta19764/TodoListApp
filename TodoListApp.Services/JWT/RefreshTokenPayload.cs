namespace TodoListApp.Services.JWT;

public class RefreshTokenPayload
{
    public string Token { get; set; } = string.Empty;

    public DateTime Expiry { get; set; }
}

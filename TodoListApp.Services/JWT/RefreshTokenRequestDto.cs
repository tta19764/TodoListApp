namespace TodoListApp.Services.JWT;
public class RefreshTokenRequestDto
{
    public string UserId { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;
}

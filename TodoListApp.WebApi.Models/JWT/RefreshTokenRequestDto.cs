namespace TodoListApp.WebApi.Models.JWT;
public class RefreshTokenRequestDto
{
    public string UserId { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;
}

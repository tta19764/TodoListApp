namespace TodoListApp.Services.JWT;
public class LogoutRequestDto
{
    public string UserId { get; set; } = null!;

    public string AccessToken { get; set; } = string.Empty;
}

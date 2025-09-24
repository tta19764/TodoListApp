using TodoListApp.WebApi.Models.JWT;

namespace TodoListApp.WebApi.Servicies;

public interface IAuthService
{
    Task<TokenResponseDto?> LoginAsync(UserDto request);

    Task<bool> LogoutAsync(string userId);

    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
}

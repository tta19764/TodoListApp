using TodoListApp.Services.JWT;

namespace TodoListApp.Services.Interfaces.Servicies;

public interface IAuthService
{
    Task<TokenResponseDto?> LoginAsync(UserDto request, CancellationToken cancellationToken = default);

    Task<bool> LogoutAsync(LogoutRequestDto logoutRequestDto, CancellationToken cancellationToken = default);

    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);
}

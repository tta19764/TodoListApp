using TodoListApp.Services.JWT;

namespace TodoListApp.Services.Interfaces.Servicies;

/// <summary>
/// Service interface for user authentication.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Logs in a user and returns a token response if successful.
    /// </summary>
    /// <param name="request">The login request user data transfer object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A token response data transfer object if login is successful; otherwise, null.</returns>
    Task<TokenResponseDto?> LoginAsync(UserDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs out a user by invalidating their tokens.
    /// </summary>
    /// <param name="logoutRequestDto">The logout request user data transfer object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if logout is successful; otherwise, false.</returns>
    Task<bool> LogoutAsync(LogoutRequestDto logoutRequestDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the access and refresh tokens using the provided refresh token.
    /// </summary>
    /// <param name="request">The refresh token data transfer object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A new token response data transfer object if the refresh is successful; otherwise, null.</returns>
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);
}

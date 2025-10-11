using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.JWT;
using TodoListApp.Services.WebApi.CustomLogs;

namespace TodoListApp.Services.WebApi.Servicies;

/// <summary>
/// Service for handling authentication-related operations such as login, logout, and token refresh via HTTP requests.
/// </summary>
public class AuthApiService : IAuthService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<AuthApiService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthApiService"/> class.
    /// </summary>
    /// <param name="httpClient">The request http client.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClient"/> or <paramref name="logger"/> is null.</exception>
    public AuthApiService(HttpClient httpClient, ILogger<AuthApiService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Logs in a user and returns JWT tokens upon successful authentication.
    /// </summary>
    /// <param name="request">The data transfer object that contains the user login info.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="TokenResponseDto"/> containing access and refresh tokens if successful; otherwise, null.</returns>
    /// <exception cref="ArgumentNullException">If request is null.</exception>
    /// <exception cref="ArgumentException">If any of the user info is null.</exception>
    /// <exception cref="TimeoutException">If the request timed out.</exception>
    public Task<TokenResponseDto?> LoginAsync(UserDto request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            JwtLog.LogNullLoginRequest(this.logger);
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Username))
        {
            JwtLog.LogNullOrEmptyUsername(this.logger, request.Username ?? "null");
            throw new ArgumentException("Username cannot be null or empty.", nameof(request));
        }

        return this.LoginInternalAsync(request, cancellationToken);
    }

    /// <summary>
    /// Logs out a user by invalidating their JWT tokens.
    /// </summary>
    /// <param name="logoutRequestDto">The data transfer object containing the access token and user ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if logout was successful; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">If the data tranfer object is null.</exception>
    /// <exception cref="TimeoutException">The request times out.</exception>
    public Task<bool> LogoutAsync(LogoutRequestDto logoutRequestDto, CancellationToken cancellationToken = default)
    {
        if (logoutRequestDto == null)
        {
            JwtLog.LogNullLogoutRequest(this.logger);
            throw new ArgumentNullException(nameof(logoutRequestDto));
        }

        return this.LogoutInternalAsync(logoutRequestDto, cancellationToken);
    }

    /// <summary>
    /// Refreshes JWT tokens using a valid refresh token.
    /// </summary>
    /// <param name="request">The data transfer object containing the user Id and refresh token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="TokenResponseDto"/> containing new access and refresh tokens if successful; otherwise, null.</returns>
    /// <exception cref="ArgumentNullException">If data transfer onject is null.</exception>
    /// <exception cref="ArgumentException">If any properties of the data transfer object is null.</exception>
    /// <exception cref="TimeoutException">If request times out.</exception>
    public Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            JwtLog.LogNullRefreshTokenRequest(this.logger);
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            JwtLog.LogNullRefreshTokenRequest(this.logger);
            throw new ArgumentException("UserId cannot be null or empty.", nameof(request));
        }

        return this.RefreshTokensInteranlAsync(request, cancellationToken);
    }

    private async Task<TokenResponseDto?> LoginInternalAsync(UserDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await this.httpClient.PostAsJsonAsync("login", request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseDto>(cancellationToken: cancellationToken);

                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    JwtLog.LogJwtTokenSuccess(this.logger, request.Username);
                    return tokenResponse;
                }

                JwtLog.LogJwtTokenInvalid(this.logger, request.Username);
                return null;
            }

            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
            JwtLog.LogJwtTokenFailed(this.logger, request.Username, (int)response.StatusCode, errorContent);
            return null;
        }
        catch (HttpRequestException ex)
        {
            JwtLog.LogHttpRequestError(this.logger, request.Username, ex);
            throw;
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken == cancellationToken)
        {
            JwtLog.LogTaskCancelledError(this.logger, request.Username, ex);
            throw;
        }
        catch (TaskCanceledException ex)
        {
            JwtLog.LogTimeoutError(this.logger, request.Username, ex);
            throw new TimeoutException($"Login request timed out for user: {request.Username}", ex);
        }
        catch (JsonException ex)
        {
            JwtLog.LogJsonParsingError(this.logger, request.Username, ex);
            throw;
        }
        catch (Exception ex) when (ex is not ArgumentNullException && ex is not ArgumentException)
        {
            JwtLog.LogUnexpectedLoginError(this.logger, request.Username, ex);
            throw;
        }
    }

    private async Task<bool> LogoutInternalAsync(LogoutRequestDto logoutRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await this.httpClient.PostAsJsonAsync("logout", logoutRequestDto, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                JwtLog.LogJwtTokenRemovalSuccess(this.logger);
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            JwtLog.LogJwtTokenFailedToRemove(this.logger, (int)response.StatusCode, errorContent);
            return false;
        }
        catch (HttpRequestException ex)
        {
            JwtLog.LogHttpClientError(this.logger, ex);
            throw;
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken == cancellationToken)
        {
            JwtLog.LogTaskCancelledError(this.logger, logoutRequestDto.UserId ?? "unknown", ex);
            throw;
        }
        catch (TaskCanceledException ex)
        {
            JwtLog.LogTimeoutError(this.logger, logoutRequestDto.UserId ?? "unknown", ex);
            throw new TimeoutException("Logout request timed out", ex);
        }
        catch (Exception ex) when (ex is not ArgumentNullException)
        {
            JwtLog.LogUnexpectedLogoutError(this.logger, ex);
            throw;
        }
    }

    private async Task<TokenResponseDto?> RefreshTokensInteranlAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await this.httpClient.PostAsJsonAsync("refresh-token", request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseDto>(cancellationToken: cancellationToken);

                if (tokenResponse != null &&
                    !string.IsNullOrEmpty(tokenResponse.AccessToken) &&
                    !string.IsNullOrEmpty(tokenResponse.RefreshToken))
                {
                    JwtLog.LogRefreshTokenSuccess(this.logger, request.UserId);
                    return tokenResponse;
                }

                JwtLog.LogRefreshTokenInvalid(this.logger, request.UserId);
                return null;
            }

            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
            JwtLog.LogRefreshTokenFailed(this.logger, request.UserId, (int)response.StatusCode, errorContent);
            return null;
        }
        catch (HttpRequestException ex)
        {
            JwtLog.LogHttpRequestError(this.logger, request.UserId, ex);
            throw;
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken == cancellationToken)
        {
            JwtLog.LogTaskCancelledError(this.logger, request.UserId, ex);
            throw;
        }
        catch (TaskCanceledException ex)
        {
            JwtLog.LogTimeoutError(this.logger, request.UserId, ex);
            throw new TimeoutException($"Refresh token request timed out for user ID: {request.UserId}", ex);
        }
        catch (JsonException ex)
        {
            JwtLog.LogJsonParsingError(this.logger, request.UserId, ex);
            throw;
        }
        catch (Exception ex) when (ex is not ArgumentNullException && ex is not ArgumentException)
        {
            JwtLog.LogUnexpectedRefreshError(this.logger, request.UserId, ex);
            throw;
        }
    }
}

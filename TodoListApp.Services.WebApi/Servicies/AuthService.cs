using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.JWT;
using TodoListApp.Services.WebApi.CustomLogs;

namespace TodoListApp.Services.WebApi.Servicies;
public class AuthService : IAuthService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<AuthService> logger;

    public AuthService(HttpClient httpClient, ILogger<AuthService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TokenResponseDto?> LoginAsync(UserDto request, CancellationToken cancellationToken = default)
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

    public async Task<bool> LogoutAsync(LogoutRequestDto logoutRequestDto, CancellationToken cancellationToken = default)
    {
        if (logoutRequestDto == null)
        {
            JwtLog.LogNullLogoutRequest(this.logger);
            throw new ArgumentNullException(nameof(logoutRequestDto));
        }

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

    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
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

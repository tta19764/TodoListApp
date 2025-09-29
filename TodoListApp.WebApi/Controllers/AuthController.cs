using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.JWT;
using TodoListApp.WebApi.CustomLogs;

namespace TodoListApp.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly ILogger<AuthController> logger;

    public AuthController(
        IAuthService authService, ILogger<AuthController> logger)
    {
        this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login([FromBody] UserDto request)
    {
        try
        {
            if (request == null)
            {
                AuthLog.LogNullLoginRequest(this.logger);
                return this.BadRequest(new { error = "Login request data is required." });
            }

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                AuthLog.LogNullUserLoginOrPassword(this.logger);
                return this.BadRequest(new { error = "Username and password are required." });
            }

            var result = await this.authService.LoginAsync(request);

            if (result == null)
            {
                AuthLog.LogInvalidLoginAttempt(this.logger, request.Username);
                return this.BadRequest(new { error = "Invalid username or password." });
            }

            AuthLog.LogUserLoggedInSuccessfully(this.logger, request.Username);
            return this.Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            AuthLog.LogNullUserLoginOrPassword(this.logger, ex);
            return this.BadRequest(new { error = "Invalid login data provided." });
        }
        catch (Exception ex)
        {
            AuthLog.LogUnexpectedErrorDuringLogin(this.logger, request?.Username ?? "unknown", ex);
            return this.StatusCode(500, new { error = "An unexpected error occurred during login. Please try again later." });
            throw;
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto logoutRequestDto)
    {
        try
        {
            if (logoutRequestDto == null)
            {
                AuthLog.LogNullLogoutRequest(this.logger);
                return this.BadRequest(new { error = "Logout request data is required." });
            }

            if (string.IsNullOrWhiteSpace(logoutRequestDto.UserId) || string.IsNullOrWhiteSpace(logoutRequestDto.AccessToken))
            {
                AuthLog.LogInvalidLogoutAttempt(this.logger, logoutRequestDto.UserId ?? "unknown");
                return this.BadRequest(new { error = "User ID and access token are required." });
            }

            var result = await this.authService.LogoutAsync(logoutRequestDto);

            if (result)
            {
                AuthLog.LogUserLoggedOutSuccessfully(this.logger, logoutRequestDto.UserId);
                return this.Ok(new { message = "User logged out successfully." });
            }

            AuthLog.LogInvalidLogoutAttempt(this.logger, logoutRequestDto.UserId);
            return this.BadRequest(new { error = "Invalid token or request." });
        }
        catch (ArgumentNullException ex)
        {
            AuthLog.LogNullLogoutRequest(this.logger, ex);
            return this.BadRequest(new { error = "Invalid logout data provided." });
        }
        catch (Exception ex)
        {
            AuthLog.LogUnexpectedErrorDuringLogout(this.logger, logoutRequestDto?.UserId ?? "unknown", ex);
            return this.StatusCode(500, new { error = "An unexpected error occurred during logout. Please try again later." });
            throw;
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
    {
        try
        {
            if (request == null)
            {
                AuthLog.LogNullRefreshTokenRequest(this.logger);
                return this.BadRequest(new { error = "Refresh token request data is required." });
            }

            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                AuthLog.LogInvalidRefreshTokenAttempt(this.logger, request.UserId ?? "unknown");
                return this.BadRequest(new { error = "User ID and refresh token are required." });
            }

            var result = await this.authService.RefreshTokensAsync(request);

            if (result == null || string.IsNullOrWhiteSpace(result.AccessToken) || string.IsNullOrWhiteSpace(result.RefreshToken))
            {
                AuthLog.LogInvalidRefreshTokenAttempt(this.logger, request.UserId);
                return this.Unauthorized(new { error = "Invalid refresh token." });
            }

            AuthLog.LogTokenRefreshedSuccessfully(this.logger, request.UserId);
            return this.Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            AuthLog.LogNullRefreshTokenRequest(this.logger, ex);
            return this.BadRequest(new { error = "Invalid refresh token data provided." });
        }
        catch (Exception ex)
        {
            AuthLog.LogUnexpectedErrorDuringRefresh(this.logger, request?.UserId ?? "unknown", ex);
            return this.StatusCode(500, new { error = "An unexpected error occurred during token refresh. Please try again later." });
            throw;
        }
    }
}

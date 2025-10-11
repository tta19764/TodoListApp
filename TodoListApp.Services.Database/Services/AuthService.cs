using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.Services.Database.CustomLogs;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.JWT;

namespace TodoListApp.Services.Database.Services;

/// <summary>
/// Service for handling authentication-related operations such as login, logout, and token refresh.
/// </summary>
public class AuthService : IAuthService
{
    private const string LOGINPROVIDER = "JwtBearer";
    private readonly UserManager<User> userManager;
    private readonly IConfiguration configuration;
    private readonly ILogger<AuthService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">If any of the parameters are null.</exception>
    public AuthService(
        UserManager<User> userManager,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Logs in a user and returns JWT tokens upon successful authentication.
    /// </summary>
    /// <param name="request">The data transfer object containing the user login info.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="TokenResponseDto"/> containing access and refresh tokens if successful; otherwise, null.</returns>
    /// <exception cref="ArgumentNullException">If data transfer object is null.</exception>
    public Task<TokenResponseDto?> LoginAsync(UserDto request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            JwtLog.LogNullUserLoginOrPassword(this.logger);
            throw new ArgumentNullException(nameof(request));
        }

        return this.LoginInternalAsync(request);
    }

    /// <summary>
    /// Logs out a user by invalidating their refresh token.
    /// </summary>
    /// <param name="logoutRequestDto">The data transfer object containing the access token and user ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if logout was successful; otherwise, false.</returns>
    public Task<bool> LogoutAsync(LogoutRequestDto logoutRequestDto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(logoutRequestDto);

        return this.LogoutInternalAsync(logoutRequestDto);
    }

    /// <summary>
    /// Refreshes JWT tokens using a valid refresh token.
    /// </summary>
    /// <param name="request">The data transfer object containing the refresh token and user ID.</param>
    /// <param name="cancellationToken">The cancellation toke.</param>
    /// <returns>A <see cref="TokenResponseDto"/> containing new access and refresh tokens if successful; otherwise, null.</returns>
    public Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return this.RefreshTokensInternalAsync(request);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private static string? GetUserIdFromToken(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);

        var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        return userIdClaim?.Value;
    }

    private async Task<TokenResponseDto?> LoginInternalAsync(UserDto request)
    {
        var user = await this.userManager.FindByNameAsync(request.Username);
        if (user is null)
        {
            JwtLog.LogUserNotFound(this.logger, request.Username);
            return null;
        }

        bool res = await this.userManager.CheckPasswordAsync(user, request.Password);

        if (!res)
        {
            JwtLog.LogInvalidPassword(this.logger, request.Username);
            return null;
        }

        var tokenResponse = await this.CreateTokenResponse(user);

        return tokenResponse;
    }

    private async Task<TokenResponseDto?> RefreshTokensInternalAsync(RefreshTokenRequestDto request)
    {
        var user = await this.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
        if (user is null)
        {
            return null;
        }

        var tokenResponse = await this.CreateTokenResponse(user);

        return tokenResponse;
    }

    private async Task<bool> LogoutInternalAsync(LogoutRequestDto logoutRequestDto)
    {
        var user = await this.userManager.FindByIdAsync(logoutRequestDto.UserId);
        if (user == null)
        {
            JwtLog.LogUserNotFound(this.logger, logoutRequestDto.UserId);
            return false;
        }

        var userId = GetUserIdFromToken(logoutRequestDto.AccessToken);
        if (userId != logoutRequestDto.UserId)
        {
            JwtLog.LogTokenMismatch(this.logger, logoutRequestDto.UserId);
            return false;
        }

        var accessToken = await this.userManager.GetAuthenticationTokenAsync(user, LOGINPROVIDER, "JwtToken");
        if (accessToken == null || accessToken != logoutRequestDto.AccessToken)
        {
            return false;
        }

        var refreshResult = await this.userManager.RemoveAuthenticationTokenAsync(
            user,
            LOGINPROVIDER,
            "JwtRefreshToken");

        if (refreshResult.Succeeded)
        {
            JwtLog.LogRefreshTokenRemoved(this.logger, user.Id.ToString(CultureInfo.InvariantCulture));
            return true;
        }
        else
        {
            JwtLog.LogFailedToRemoveRefreshToken(this.logger, user.Id.ToString(CultureInfo.InvariantCulture));
            return false;
        }
    }

    private async Task<User?> ValidateRefreshTokenAsync(string userNameIdentifier, string refreshToken)
    {
        if (userNameIdentifier is null)
        {
            return null;
        }

        var user = await this.userManager.FindByIdAsync(userNameIdentifier);

        if (user is null)
        {
            return null;
        }

        var payloadJson = await this.userManager.GetAuthenticationTokenAsync(
            user,
            LOGINPROVIDER,
            "JwtRefreshToken");

        var payload = JsonSerializer.Deserialize<RefreshTokenPayload>(payloadJson);

        if (payload is null || payload.Token != refreshToken)
        {
            return null;
        }

        if (payload.Expiry <= DateTime.UtcNow)
        {
            JwtLog.LogRefreshTokenExpired(this.logger, userNameIdentifier);
            return null;
        }

        return user;
    }

    private async Task<TokenResponseDto?> CreateTokenResponse(User user)
    {
        var token = this.CreateToken(user);
        var refreshToken = await this.GenerateAndSaveRefreshTokenAsync(user);

        if (token is null || refreshToken is null)
        {
            return null;
        }

        JwtLog.LogJwtTokenCreated(this.logger, user.Id.ToString(CultureInfo.InvariantCulture));

        return new TokenResponseDto
        {
            AccessToken = token,
            RefreshToken = refreshToken,
        };
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
            {
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture)),
            };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(this.configuration["AppSettings:Token"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: this.configuration["AppSettings:Issuer"],
            audience: this.configuration["AppSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return token;
    }

    private async Task<string?> GenerateAndSaveRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        var expiry = DateTime.UtcNow.AddDays(30);
        var payload = JsonSerializer.Serialize(new RefreshTokenPayload
        {
            Token = refreshToken,
            Expiry = expiry,
        });

        var result = await this.userManager.SetAuthenticationTokenAsync(
            user,
            LOGINPROVIDER,
            "JwtRefreshToken",
            payload);

        if (result.Succeeded)
        {
            JwtLog.LogRefreshTokenCreated(this.logger, user.Id.ToString(CultureInfo.InvariantCulture));
            return refreshToken;
        }
        else
        {
            JwtLog.LogFailedToSaveRefreshToken(this.logger, user.Id.ToString(CultureInfo.InvariantCulture));
            return null;
        }
    }
}

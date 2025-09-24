using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.Services.Database.Entities;
using TodoListApp.WebApi.CustomLogs;
using TodoListApp.WebApi.Models.JWT;
using TodoListApp.WebApi.Paylodas;

namespace TodoListApp.WebApi.Servicies;

public class AuthService : IAuthService
{
    private const string LOGINPROVIDER = "JwtBearer";
    private readonly UserManager<User> userManager;
    private readonly ILogger<AuthService> logger;
    private readonly IConfiguration configuration;

    public AuthService(
        UserManager<User> userManager,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        this.userManager = userManager;
        this.logger = logger;
        this.configuration = configuration;
    }

    public async Task<TokenResponseDto?> LoginAsync(UserDto request)
    {
        if (request == null)
        {
            AuthLog.LogNullUserLoginOrPassword(this.logger);
            throw new ArgumentNullException(nameof(request));
        }

        var user = await this.userManager.FindByNameAsync(request.Username);
        if (user is null)
        {
            AuthLog.LogUserNotfound(this.logger, request.Username);
            return null;
        }

        bool res = await this.userManager.CheckPasswordAsync(user, request.Password);

        if (!res)
        {
            AuthLog.LogUserNotfound(this.logger, user.Id.ToString(CultureInfo.InvariantCulture));
            return null;
        }

        return await this.CreateTokenResponse(user);
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        var user = await this.userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        // Remove the authentication token
        var result = await this.userManager.RemoveAuthenticationTokenAsync(
            user,
            LOGINPROVIDER,
            "JwtToken");

        if (result.Succeeded)
        {
            AuthLog.LogJwtTokenRemoved(this.logger, user.Id.ToString(CultureInfo.InvariantCulture));
            return true;
        }
        else
        {
            AuthLog.LogJwtTokenNotRemoved(this.logger, user.Id.ToString(CultureInfo.InvariantCulture));
            return false;
        }
    }

    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = await this.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
        if (user is null)
        {
            return null;
        }

        return await this.CreateTokenResponse(user);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
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

        if (payload is null || payload.Token != refreshToken
                || payload.Expiry <= DateTime.UtcNow)
        {
            return null;
        }

        return user;
    }

    private async Task<TokenResponseDto?> CreateTokenResponse(User? user)
    {
        ArgumentNullException.ThrowIfNull(user);

        string? token = await this.CreateToken(user);
        string? refreshToken = await this.GenerateAndSaveRefreshTokenAsync(user);

        if (token is null || refreshToken is null)
        {
            return null;
        }

        AuthLog.LogJwtTokenCreated(this.logger, user.Id.ToString(CultureInfo.InvariantCulture));

        return new TokenResponseDto
        {
            AccessToken = token,
            RefreshToken = refreshToken,
        };
    }

    private async Task<string?> CreateToken(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture)),
            };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(this.configuration.GetValue<string>("AppSettings:Token")!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: this.configuration.GetValue<string>("AppSettings:Issuer"),
            audience: this.configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        var result = await this.userManager.SetAuthenticationTokenAsync(user, LOGINPROVIDER, "JwtToken", token);

        return result.Succeeded ? token : null;
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

        AuthLog.LogJwtTokenRefreshed(this.logger, user.Id.ToString(CultureInfo.InvariantCulture));

        return result.Succeeded ? refreshToken : null;
    }
}

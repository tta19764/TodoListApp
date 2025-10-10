using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.JWT;
using TodoListApp.WebApp.CustomLogs;
using TodoListApp.WebApp.Data;

namespace TodoListApp.WebApp.Handlers;

/// <summary>
/// HTTP handler for attaching and refreshing JWT tokens in outgoing requests.
/// </summary>
internal sealed class JwtTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly UserManager<AppUser> userManager;
    private readonly IAuthService authService;
    private readonly ITokenStorageService tokenStorageService;
    private readonly ILogger<JwtTokenHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtTokenHandler"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The http context accessor.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="authService">The auth service.</param>
    /// <param name="tokenStorageService">The token storage service.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">If any of the parameters are null.</exception>
    public JwtTokenHandler(
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager,
        IAuthService authService,
        ITokenStorageService tokenStorageService,
        ILogger<JwtTokenHandler> logger)
    {
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
        this.tokenStorageService = tokenStorageService ?? throw new ArgumentNullException(nameof(tokenStorageService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var httpContext = this.httpContextAccessor.HttpContext;

        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var user = await this.userManager.GetUserAsync(httpContext.User);
            if (user != null)
            {
                var token = await this.tokenStorageService.GetToken(user.Id.ToString(CultureInfo.InvariantCulture));

                if (token is not null && !string.IsNullOrEmpty(token))
                {
                    // Check expiry
                    var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    var exp = jwt.ValidTo;

                    if (exp < DateTime.UtcNow.AddMinutes(-1))
                    {
                        var refreshToken = JsonSerializer.Deserialize<RefreshTokenPayload>(await this.userManager.GetAuthenticationTokenAsync(user, "JwtBearer", "JwtRefreshToken"));

                        if (refreshToken is not null && !string.IsNullOrEmpty(refreshToken.Token))
                        {
                            var tokenRequestDto = new RefreshTokenRequestDto { RefreshToken = refreshToken.Token, UserId = user.Id.ToString(CultureInfo.InvariantCulture) };
                            var tokenResponse = await this.authService.RefreshTokensAsync(tokenRequestDto, cancellationToken);
                            if (tokenResponse != null)
                            {
                                AccountLog.LogJwtTokenRefreshed(this.logger, user.UserName);
                                token = tokenResponse.AccessToken;
                                var tokenSaveResult = await this.tokenStorageService.SaveToken(user.Id.ToString(CultureInfo.InvariantCulture), token);
                                if (tokenSaveResult)
                                {
                                    AccountLog.LogJwtTokenStored(this.logger, user.UserName);
                                }
                                else
                                {
                                    AccountLog.LogJwtTokenStoreFailed(this.logger, user.UserName);
                                }
                            }
                            else
                            {
                                AccountLog.LogJwtTokenRefreshFailed(this.logger, user.UserName);
                            }
                        }
                    }

                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

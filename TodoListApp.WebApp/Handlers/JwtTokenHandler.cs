using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using TodoListApp.WebApi.Models.JWT;
using TodoListApp.WebApp.Data;

namespace TodoListApp.WebApp.Handlers;

public class JwtTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly UserManager<AppUser> userManager;
    private readonly IHttpClientFactory httpClientFactory;

    public JwtTokenHandler(
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager,
        IHttpClientFactory httpClientFactory)
    {
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
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
                // Grab current token
                var token = await this.userManager.GetAuthenticationTokenAsync(user, "JwtBearer", "JwtToken");

                if (!string.IsNullOrEmpty(token))
                {
                    // Check expiry
                    var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    var exp = jwt.ValidTo;

                    if (exp < DateTime.UtcNow.AddMinutes(-1)) // expired or close to expiry
                    {
                        var refreshToken = await this.userManager.GetAuthenticationTokenAsync(user, "JwtBearer", "JwtRefreshToken");

                        if (!string.IsNullOrEmpty(refreshToken))
                        {
                            var client = this.httpClientFactory.CreateClient("ApiClient");
                            var response = await client.PostAsJsonAsync(
                                "/api/auth/refresh-token",
                                new RefreshTokenRequestDto
                                {
                                    UserId = user.Id.ToString(CultureInfo.InvariantCulture),
                                    RefreshToken = refreshToken,
                                },
                                cancellationToken);

                            if (response.IsSuccessStatusCode)
                            {
                                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseDto>(cancellationToken: cancellationToken);
                                if (tokenResponse != null)
                                {
                                    token = tokenResponse.AccessToken;
                                }
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

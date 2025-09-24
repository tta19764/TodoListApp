using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TodoListApp.Services.WebApi.CustomLogs;
using TodoListApp.Services.WebApi.Interfaces;
using TodoListApp.WebApi.Models.JWT;

namespace TodoListApp.Services.WebApi.Servicies;
public class UserService : IUserService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<UserService> logger;

    public UserService(HttpClient httpClient, ILogger<UserService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string?> Login(string login, string password, Uri? endpoint = null)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentNullException(nameof(login));
        }
        else if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        var userDto = new UserDto { Username = login, Password = password };

        using var response = await this.httpClient.PostAsJsonAsync(endpoint ?? new Uri("auth/login", UriKind.Relative), userDto);

        if (response.IsSuccessStatusCode)
        {
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
            if (!string.IsNullOrEmpty(tokenResponse?.AccessToken))
            {
                Log.LogJwtTokenSuccess(this.logger, userDto.Username);
                return tokenResponse.AccessToken;
            }

            Log.LogJwtTokenInvalid(this.logger, userDto.Username);
            return null;
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        Log.LogJwtTokenFailed(this.logger, userDto.Username, (int)response.StatusCode, errorContent);

        return null;
    }
}

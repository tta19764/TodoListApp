using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models.JWT;
using TodoListApp.WebApi.Servicies;

namespace TodoListApp.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(
        IAuthService authService)
    {
        this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login([FromBody] UserDto request)
    {
        var result = await this.authService.LoginAsync(request);
        if (result is null)
        {
            return this.BadRequest("Invalid username or password.");
        }

        return this.Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return this.BadRequest("Invalid token");
        }

        // Find the user
        var result = await this.authService.LogoutAsync(userId);

        if (result)
        {
            return this.Ok("User loged out.");
        }

        return this.BadRequest("Failed to logout the user.");
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
    {
        var result = await this.authService.RefreshTokensAsync(request);
        if (result is null || result.AccessToken is null || result.RefreshToken is null)
        {
            return this.Unauthorized("Invalid refresh token.");
        }

        return this.Ok(result);
    }
}

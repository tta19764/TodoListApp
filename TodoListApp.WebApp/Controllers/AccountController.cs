using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.WebApi.Interfaces;
using TodoListApp.WebApp.CustomLogs;
using TodoListApp.WebApp.Data;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
[Route("Account")]
public class AccountController : Controller
{
    private readonly SignInManager<AppUser> signInManager;
    private readonly IUserService userService;
    private readonly ILogger<AccountController> logger;
    private readonly IConfiguration configuration;

    public AccountController(
        SignInManager<AppUser> signInManager,
        IUserService userService,
        ILogger<AccountController> logger,
        IConfiguration configuration)
    {
        this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    [HttpGet]
    [Route("Login")]
    [AllowAnonymous]
    public async Task<ViewResult> LoginAsync(Uri? returnUrl = null)
    {
        _ = this.ModelState.IsValid;

        returnUrl ??= new Uri("/", UriKind.Relative);

        // Clear the existing external cookie to ensure a clean login process
        await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        return this.View(new LoginViewModel
        {
            ReturnUrl = returnUrl,
        });
    }

    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginViewModel loginViewModel)
    {
        ArgumentNullException.ThrowIfNull(loginViewModel);

        if (!this.ModelState.IsValid)
        {
            return this.View(loginViewModel);
        }

        var result = await this.signInManager.PasswordSignInAsync(
                loginViewModel.Login,
                loginViewModel.Password,
                loginViewModel.RememberMe,
                lockoutOnFailure: false);

        if (result.Succeeded)
        {
            Log.LogUserLoggedIn(this.logger, loginViewModel.Login);
            var connection = this.configuration.GetValue<string>("ApiSettings:ApiBaseUrl");
            ArgumentNullException.ThrowIfNull(connection);

            var uri = new Uri($"{connection}auth/login", UriKind.Absolute);

            var tokenResult = await this.userService.Login(loginViewModel.Login, loginViewModel.Password, uri);

            if (tokenResult is not null)
            {
                Log.LogJwtTokenStored(this.logger, loginViewModel.Login);
            }

            // Redirect to return URL or default location
            var returnUrl = string.IsNullOrEmpty(loginViewModel.ReturnUrl.ToString()) ? "~/" : loginViewModel.ReturnUrl.ToString();
            return this.LocalRedirect(returnUrl);
        }

        if (result.IsLockedOut)
        {
            Log.LogUserLockedOut(this.logger);
            return this.RedirectToPage("./Lockout");
        }

        this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return this.View(loginViewModel);
    }
}

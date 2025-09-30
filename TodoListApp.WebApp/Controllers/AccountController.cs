using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.JWT;
using TodoListApp.WebApp.CustomLogs;
using TodoListApp.WebApp.Data;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
[Route("Account")]
public class AccountController : Controller
{
    private readonly SignInManager<AppUser> signInManager;
    private readonly IAuthService authService;
    private readonly ILogger<AccountController> logger;
    private readonly UserManager<AppUser> userManager;
    private readonly ITokenStorageService tokenStorageService;

    public AccountController(
        SignInManager<AppUser> signInManager,
        IAuthService authService,
        ILogger<AccountController> logger,
        UserManager<AppUser> userManager,
        ITokenStorageService tokenStorageService)
    {
        this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.tokenStorageService = tokenStorageService ?? throw new ArgumentNullException(nameof(tokenStorageService));
    }

    [HttpGet]
    [Route("Login")]
    [AllowAnonymous]
    public async Task<ViewResult> LoginAsync(Uri? returnUrl = null)
    {
        try
        {
            _ = this.ModelState.IsValid;

            returnUrl ??= new Uri("/", UriKind.Relative);

            AccountLog.LogLoginPageAccessed(this.logger, returnUrl);

            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return this.View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
            });
        }
        catch (Exception ex)
        {
            AccountLog.LogUnexpectedErrorDuringLogin(this.logger, "Anonymous", ex);
            throw;
        }
    }

    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginViewModel loginViewModel)
    {
        try
        {
            if (loginViewModel == null)
            {
                AccountLog.LogNullLoginViewModel(this.logger);
                throw new ArgumentNullException(nameof(loginViewModel));
            }

            if (!this.ModelState.IsValid)
            {
                AccountLog.LogInvalidModelState(this.logger);
                return this.View(loginViewModel);
            }

            // Sign out any existing sessions
            await this.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await this.signInManager.SignOutAsync();

            var userDto = new UserDto()
            {
                Username = loginViewModel.Login,
                Password = loginViewModel.Password,
            };

            TokenResponseDto? tokenResult = null;

            try
            {
                tokenResult = await this.authService.LoginAsync(userDto);
            }
            catch (Exception ex)
            {
                AccountLog.LogApiAuthenticationFailed(this.logger, loginViewModel.Login);
                AccountLog.LogUnexpectedErrorDuringLogin(this.logger, loginViewModel.Login, ex);

                this.ModelState.AddModelError(string.Empty, "Unable to connect to authentication service. Please try again later.");
                return this.View(loginViewModel);
                throw;
            }

            if (tokenResult == null)
            {
                AccountLog.LogApiAuthenticationFailed(this.logger, loginViewModel.Login);
                this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return this.View(loginViewModel);
            }

            // Sign in with Identity
            Microsoft.AspNetCore.Identity.SignInResult result;
            try
            {
                result = await this.signInManager.PasswordSignInAsync(
                    loginViewModel.Login,
                    loginViewModel.Password,
                    loginViewModel.RememberMe,
                    lockoutOnFailure: false);
            }
            catch (Exception ex)
            {
                AccountLog.LogIdentitySignInFailed(this.logger, loginViewModel.Login, ex);
                throw;
            }

            if (result.Succeeded)
            {
                AccountLog.LogUserLoggedIn(this.logger, loginViewModel.Login);

                // Store JWT token
                var user = await this.userManager.FindByNameAsync(loginViewModel.Login);
                if (user != null)
                {
                    try
                    {
                        var tokenSaveResult = await this.tokenStorageService.SaveToken(
                            user.Id.ToString(CultureInfo.InvariantCulture),
                            tokenResult.AccessToken);

                        if (!tokenSaveResult)
                        {
                            AccountLog.LogJwtTokenStoreFailed(this.logger, loginViewModel.Login);
                        }
                    }
                    catch (Exception ex)
                    {
                        AccountLog.LogUnexpectedErrorStoringToken(this.logger, user.Id.ToString(CultureInfo.InvariantCulture), ex);
                        throw;
                    }
                }

                var returnUrl = string.IsNullOrEmpty(loginViewModel.ReturnUrl.ToString())
                    ? "~/"
                    : loginViewModel.ReturnUrl.ToString();

                return this.LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                AccountLog.LogUserLockedOut(this.logger);
                return this.RedirectToPage("./Lockout");
            }

            AccountLog.LogInvalidLoginAttempt(this.logger, loginViewModel.Login);
            this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return this.View(loginViewModel);
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (Exception ex)
        {
            AccountLog.LogUnexpectedErrorDuringLogin(this.logger, loginViewModel?.Login ?? "unknown", ex);
            throw;
        }
    }

    [HttpPost]
    [Route("Logout")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutAsync()
    {
        AppUser? user = null;

        try
        {
            user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                AccountLog.LogUserNotFoundInStorage(this.logger, "Current user");
                return this.RedirectToAction("Index", "Home");
            }

            var token = await this.userManager.GetAuthenticationTokenAsync(user, "JwtBearer", "JwtToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                // Logout from API
                var logoutRequestDto = new LogoutRequestDto()
                {
                    UserId = user.Id.ToString(CultureInfo.InvariantCulture),
                    AccessToken = token,
                };

                bool apiLogoutSuccess = false;

                try
                {
                    apiLogoutSuccess = await this.authService.LogoutAsync(logoutRequestDto);
                }
                catch (Exception ex)
                {
                    AccountLog.LogUnexpectedErrorDuringLogout(this.logger, user.UserName ?? user.Id.ToString(CultureInfo.InvariantCulture), ex);
                    throw;
                }

                if (apiLogoutSuccess)
                {
                    // Remove token from storage
                    try
                    {
                        var accessTokenRemoved = await this.tokenStorageService.RemoveToken(
                            user.Id.ToString(CultureInfo.InvariantCulture));

                        if (accessTokenRemoved)
                        {
                            AccountLog.LogJwtTokenRemoved(this.logger, user.UserName ?? user.Id.ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            AccountLog.LogJwtTokenRemoveFailed(this.logger, user.UserName ?? user.Id.ToString(CultureInfo.InvariantCulture));
                        }
                    }
                    catch (Exception ex)
                    {
                        AccountLog.LogUnexpectedErrorRemovingToken(
                            this.logger,
                            user.Id.ToString(CultureInfo.InvariantCulture),
                            ex);
                        throw;
                    }
                }
                else
                {
                    AccountLog.LogJwtTokenRemoveFailed(this.logger, user.UserName ?? user.Id.ToString(CultureInfo.InvariantCulture));
                }
            }

            await this.PerformLocalLogout();
            AccountLog.LogUserLoggedOut(this.logger);

            return this.RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            AccountLog.LogUnexpectedErrorDuringLogout(
                this.logger,
                user?.UserName ?? "unknown",
                ex);

            await this.PerformLocalLogout();

            throw;
        }
    }

    [HttpGet]
    [Route("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(Uri? returnUrl = null)
    {
        try
        {
            _ = this.ModelState.IsValid;

            returnUrl ??= new Uri("/", UriKind.Relative);

            AccountLog.LogRegisterPageAccessed(this.logger, returnUrl);

            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return this.View(new RegisterViewModel
            {
                ReturnUrl = returnUrl,
            });
        }
        catch (Exception ex)
        {
            AccountLog.LogUnexpectedErrorDuringRegister(this.logger, "Anonymous", ex);
            throw;
        }
    }

    [HttpPost]
    [Route("Register")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (registerViewModel == null)
        {
            AccountLog.LogNullRegisterViewModel(this.logger);
            throw new ArgumentNullException(nameof(registerViewModel));
        }

        try
        {
            if (!this.ModelState.IsValid)
            {
                AccountLog.LogInvalidModelState(this.logger);
                return this.View(registerViewModel);
            }

            // Sign out any existing sessions
            await this.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await this.signInManager.SignOutAsync();

            AppUser user = new AppUser
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                Email = registerViewModel.Email,
                UserName = registerViewModel.Login,
            };

            // Sign in with Identity
            IdentityResult result;
            try
            {
                result = await this.userManager.CreateAsync(user, registerViewModel.Password);
            }
            catch (Exception ex)
            {
                AccountLog.LogIdentityRegisterFailed(this.logger, registerViewModel.Login, ex);
                throw;
            }

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                AccountLog.LogRegistrationFailedWithErrors(this.logger, registerViewModel.Login, errors);

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.View(registerViewModel);
            }

            // User created successfully
            AccountLog.LogUserRegistered(this.logger, registerViewModel.Login);

            // Authenticate with API to get JWT token
            var userDto = new UserDto()
            {
                Username = registerViewModel.Login,
                Password = registerViewModel.Password,
            };

            TokenResponseDto? tokenResult = null;

            try
            {
                tokenResult = await this.authService.LoginAsync(userDto);
            }
            catch (Exception ex)
            {
                AccountLog.LogApiAuthenticationFailed(this.logger, registerViewModel.Login);
                AccountLog.LogUnexpectedErrorDuringLogin(this.logger, registerViewModel.Login, ex);

                this.ModelState.AddModelError(string.Empty, "Account created successfully, but unable to connect to authentication service. Please try logging in.");
                return this.View(registerViewModel);
                throw;
            }

            if (tokenResult == null)
            {
                AccountLog.LogApiAuthenticationFailed(this.logger, registerViewModel.Login);
                this.ModelState.AddModelError(string.Empty, "Account created successfully, but authentication failed. Please try logging in.");
                return this.View(registerViewModel);
            }

            // Sign in the newly registered user
            Microsoft.AspNetCore.Identity.SignInResult signInResult;
            try
            {
                signInResult = await this.signInManager.PasswordSignInAsync(
                    registerViewModel.Login,
                    registerViewModel.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);
            }
            catch (Exception ex)
            {
                AccountLog.LogIdentitySignInFailed(this.logger, registerViewModel.Login, ex);
                this.ModelState.AddModelError(string.Empty, "Account created successfully, but sign-in failed. Please try logging in.");
                return this.View(registerViewModel);
                throw;
            }

            if (!signInResult.Succeeded)
            {
                AccountLog.LogIdentitySignInFailed(this.logger, registerViewModel.Login, null);
                this.ModelState.AddModelError(string.Empty, "Account created successfully, but sign-in failed. Please try logging in.");
                return this.View(registerViewModel);
            }

            AccountLog.LogUserLoggedIn(this.logger, registerViewModel.Login);

            // Store JWT token
            var createdUser = await this.userManager.FindByNameAsync(registerViewModel.Login);
            if (createdUser != null)
            {
                try
                {
                    var tokenSaveResult = await this.tokenStorageService.SaveToken(
                        createdUser.Id.ToString(CultureInfo.InvariantCulture),
                        tokenResult.AccessToken);

                    if (!tokenSaveResult)
                    {
                        AccountLog.LogJwtTokenStoreFailed(this.logger, registerViewModel.Login);
                    }
                }
                catch (Exception ex)
                {
                    AccountLog.LogUnexpectedErrorStoringToken(this.logger, createdUser.Id.ToString(CultureInfo.InvariantCulture), ex);
                    throw;
                }
            }

            var returnUrl = string.IsNullOrEmpty(registerViewModel.ReturnUrl.ToString())
            ? "~/"
            : registerViewModel.ReturnUrl.ToString();

            return this.LocalRedirect(returnUrl);
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (Exception ex)
        {
            AccountLog.LogUnexpectedErrorDuringRegister(this.logger, registerViewModel.Login ?? "unknown", ex);
            throw;
        }
    }

    private async Task PerformLocalLogout()
    {
        try
        {
            await this.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await this.signInManager.SignOutAsync();
        }
        catch (Exception ex)
        {
            AccountLog.LogUnexpectedErrorDuringLogout(this.logger, "System", ex);
            throw;
        }
    }
}

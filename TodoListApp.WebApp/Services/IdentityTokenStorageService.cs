using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.WebApp.CustomLogs;
using TodoListApp.WebApp.Data;

namespace TodoListApp.WebApp.Services;

public class IdentityTokenStorageService : ITokenStorageService
{
    private readonly UserManager<AppUser> userManager;
    private readonly ApplicationDbContext context;
    private readonly ILogger<IdentityTokenStorageService> logger;

    public IdentityTokenStorageService(
        UserManager<AppUser> userManager,
        ApplicationDbContext context,
        ILogger<IdentityTokenStorageService> logger)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string?> GetToken(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            AccountLog.LogInvalidUserIdFormat(this.logger, userId ?? "null");
            return null;
        }

        try
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                AccountLog.LogUserNotFoundInStorage(this.logger, userId);
                return null;
            }

            var token = await this.userManager.GetAuthenticationTokenAsync(user, "JwtBearer", "JwtToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                AccountLog.LogTokenNotFoundInStorage(this.logger, userId);
                return null;
            }

            AccountLog.LogTokenRetrievedFromStorage(this.logger, userId);
            return token;
        }
        catch (Exception ex)
        {
            AccountLog.LogUnexpectedErrorRetrievingToken(this.logger, userId, ex);
            throw;
        }
    }

    public async Task<bool> RemoveToken(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            AccountLog.LogInvalidUserIdFormat(this.logger, userId ?? "null");
            return false;
        }

        if (!int.TryParse(userId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
        {
            AccountLog.LogInvalidUserIdFormat(this.logger, userId);
            return false;
        }

        try
        {
            var accessToken = await this.context.UserTokens
                .FirstOrDefaultAsync(t => t.UserId == id &&
                                  t.LoginProvider == "JwtBearer" &&
                                  t.Name == "JwtToken");

            if (accessToken != null)
            {
                _ = this.context.UserTokens.Remove(accessToken);
                _ = await this.context.SaveChangesAsync();
            }

            return true;
        }
        catch (DbUpdateException ex)
        {
            AccountLog.LogDatabaseErrorRemovingToken(this.logger, userId, ex);
            return false;
        }
        catch (Exception ex)
        {
            AccountLog.LogUnexpectedErrorRemovingToken(this.logger, userId, ex);
            throw;
        }
    }

    public async Task<bool> SaveToken(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            AccountLog.LogInvalidUserIdFormat(this.logger, userId ?? "null");
            return false;
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            AccountLog.LogJwtTokenStoreFailed(this.logger, userId);
            return false;
        }

        try
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                AccountLog.LogUserNotFoundInStorage(this.logger, userId);
                return false;
            }

            var tokenSaveResult = await this.userManager.SetAuthenticationTokenAsync(user, "JwtBearer", "JwtToken", token);

            if (tokenSaveResult.Succeeded)
            {
                AccountLog.LogJwtTokenStored(this.logger, user.UserName ?? userId);
                return true;
            }
            else
            {
                AccountLog.LogJwtTokenStoreFailed(this.logger, user.UserName ?? userId);
                return false;
            }
        }
        catch (Exception ex)
        {
            AccountLog.LogUnexpectedErrorStoringToken(this.logger, userId, ex);
            throw;
        }
    }
}

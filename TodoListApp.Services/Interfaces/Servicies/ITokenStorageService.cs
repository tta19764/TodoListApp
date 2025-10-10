namespace TodoListApp.Services.Interfaces.Servicies;

/// <summary>
/// Service for storing and retrieving user tokens.
/// </summary>
public interface ITokenStorageService
{
    /// <summary>
    /// Saves a token for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="token">The access token.</param>
    /// <returns>True if the token was saved successfully; otherwise, false.</returns>
    Task<bool> SaveToken(string userId, string token);

    /// <summary>
    /// Removes the token for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>True if the token was removed successfully; otherwise, false.</returns>
    Task<bool> RemoveToken(string userId);

    /// <summary>
    /// Retrieves the token for a specific user.
    /// </summary>
    /// <param name="userId">The suer ID.</param>
    /// <returns>The access token if found; otherwise, null.</returns>
    Task<string?> GetToken(string userId);
}

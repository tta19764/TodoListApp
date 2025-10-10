using Microsoft.Extensions.Logging;

namespace TodoListApp.Services.Database.CustomLogs;

/// <summary>
/// Logging helper for JWT-related events.
/// </summary>
public static class JwtLog
{
    private static readonly Action<ILogger, string, Exception?> TokenMismatch =
    LoggerMessage.Define<string>(
        LogLevel.Warning,
        new EventId(1101, nameof(TokenMismatch)),
        "Token mismatch detected for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> UserNotFound =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1102, nameof(UserNotFound)),
            "User not found for username/ID: {UserIdentifier}");

    private static readonly Action<ILogger, string, Exception?> InvalidPassword =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1103, nameof(InvalidPassword)),
            "Invalid password provided for username: {Username}");

    private static readonly Action<ILogger, string, Exception?> RefreshTokenExpired =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1104, nameof(RefreshTokenExpired)),
            "Refresh token expired for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> FailedToCreateToken =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(2101, nameof(FailedToCreateToken)),
            "Failed to create authentication token for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> FailedToRemoveRefreshToken =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(2102, nameof(FailedToRemoveRefreshToken)),
            "Failed to remove refresh token for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> FailedToSaveRefreshToken =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(2103, nameof(FailedToSaveRefreshToken)),
            "Failed to save refresh token for user ID: {UserId}");

    private static readonly Action<ILogger, Exception?> NullUserLoginOrPassword =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(2104, nameof(NullUserLoginOrPassword)),
            "User login or password not set to an instance");

    private static readonly Action<ILogger, string, Exception?> JwtTokenCreated =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(3101, nameof(JwtTokenCreated)),
            "JWT token created for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> RefreshTokenCreated =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(3102, nameof(RefreshTokenCreated)),
            "Refresh token created for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> RefreshTokenRemoved =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(3103, nameof(RefreshTokenRemoved)),
            "Refresh token removed for user ID: {UserId}");

    /// <summary>
    /// Logs a warning that a token mismatch was detected for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user with the token mismatch.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogTokenMismatch(ILogger logger, string userId, Exception? exception = null) =>
        TokenMismatch(logger, userId, exception);

    /// <summary>
    /// Logs a warning that a user was not found by username or ID.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userIdentifier">The username or user ID that was not found.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogUserNotFound(ILogger logger, string userIdentifier, Exception? exception = null) =>
        UserNotFound(logger, userIdentifier, exception);

    /// <summary>
    /// Logs a warning that an invalid password was provided for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for which an invalid password was provided.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogInvalidPassword(ILogger logger, string username, Exception? exception = null) =>
        InvalidPassword(logger, username, exception);

    /// <summary>
    /// Logs a warning that a refresh token has expired for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user whose refresh token expired.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogRefreshTokenExpired(ILogger logger, string userId, Exception? exception = null) =>
        RefreshTokenExpired(logger, userId, exception);

    /// <summary>
    /// Logs an error that creating an authentication token failed for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user for whom token creation failed.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogFailedToCreateToken(ILogger logger, string userId, Exception? exception = null) =>
        FailedToCreateToken(logger, userId, exception);

    /// <summary>
    /// Logs an error that removing a refresh token failed for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user for whom refresh token removal failed.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogFailedToRemoveRefreshToken(ILogger logger, string userId, Exception? exception = null) =>
        FailedToRemoveRefreshToken(logger, userId, exception);

    /// <summary>
    /// Logs an error that saving a refresh token failed for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user for whom refresh token saving failed.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogFailedToSaveRefreshToken(ILogger logger, string userId, Exception? exception = null) =>
        FailedToSaveRefreshToken(logger, userId, exception);

    /// <summary>
    /// Logs an error that the user login or password was not set to an instance (null reference).
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogNullUserLoginOrPassword(ILogger logger, Exception? exception = null) =>
        NullUserLoginOrPassword(logger, exception);

    /// <summary>
    /// Logs that a JWT token has been successfully created for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user for whom the JWT token was created.</param>
    public static void LogJwtTokenCreated(ILogger logger, string userId) =>
        JwtTokenCreated(logger, userId, null);

    /// <summary>
    /// Logs that a refresh token has been successfully created for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user for whom the refresh token was created.</param>
    public static void LogRefreshTokenCreated(ILogger logger, string userId) =>
        RefreshTokenCreated(logger, userId, null);

    /// <summary>
    /// Logs that a refresh token has been successfully removed for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user for whom the refresh token was removed.</param>
    public static void LogRefreshTokenRemoved(ILogger logger, string userId) =>
        RefreshTokenRemoved(logger, userId, null);
}

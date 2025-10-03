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

    public static void LogTokenMismatch(ILogger logger, string userId, Exception? exception = null) =>
        TokenMismatch(logger, userId, exception);

    public static void LogUserNotFound(ILogger logger, string userIdentifier, Exception? exception = null) =>
        UserNotFound(logger, userIdentifier, exception);

    public static void LogInvalidPassword(ILogger logger, string username, Exception? exception = null) =>
        InvalidPassword(logger, username, exception);

    public static void LogRefreshTokenExpired(ILogger logger, string userId, Exception? exception = null) =>
        RefreshTokenExpired(logger, userId, exception);

    public static void LogFailedToCreateToken(ILogger logger, string userId, Exception? exception = null) =>
        FailedToCreateToken(logger, userId, exception);

    public static void LogFailedToRemoveRefreshToken(ILogger logger, string userId, Exception? exception = null) =>
        FailedToRemoveRefreshToken(logger, userId, exception);

    public static void LogFailedToSaveRefreshToken(ILogger logger, string userId, Exception? exception = null) =>
        FailedToSaveRefreshToken(logger, userId, exception);

    public static void LogNullUserLoginOrPassword(ILogger logger, Exception? exception = null) =>
        NullUserLoginOrPassword(logger, exception);

    public static void LogJwtTokenCreated(ILogger logger, string userId) =>
        JwtTokenCreated(logger, userId, null);

    public static void LogRefreshTokenCreated(ILogger logger, string userId) =>
        RefreshTokenCreated(logger, userId, null);

    public static void LogRefreshTokenRemoved(ILogger logger, string userId) =>
        RefreshTokenRemoved(logger, userId, null);
}

namespace TodoListApp.WebApi.CustomLogs;

/// <summary>
/// Static class for authentication-related logging.
/// </summary>
public static class AuthLog
{
    // Warning level logs - Authentication failures
    private static readonly Action<ILogger, string, Exception?> InvalidLoginAttempt =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1001, nameof(InvalidLoginAttempt)),
            "Invalid login attempt for username: {Username}");

    private static readonly Action<ILogger, string, Exception?> InvalidLogoutAttempt =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1002, nameof(InvalidLogoutAttempt)),
            "Invalid logout attempt for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> InvalidRefreshTokenAttempt =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1003, nameof(InvalidRefreshTokenAttempt)),
            "Invalid refresh token attempt for user ID: {UserId}");

    private static readonly Action<ILogger, Exception?> NullLoginRequest =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(1004, nameof(NullLoginRequest)),
            "Null login request received");

    private static readonly Action<ILogger, Exception?> NullLogoutRequest =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(1005, nameof(NullLogoutRequest)),
            "Null logout request received");

    private static readonly Action<ILogger, Exception?> NullRefreshTokenRequest =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(1006, nameof(NullRefreshTokenRequest)),
            "Null refresh token request received");

    // Error level logs - System failures
    private static readonly Action<ILogger, string, Exception?> UnexpectedErrorDuringLogin =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(2001, nameof(UnexpectedErrorDuringLogin)),
            "Unexpected error occurred during login for username: {Username}");

    private static readonly Action<ILogger, string, Exception?> UnexpectedErrorDuringLogout =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(2002, nameof(UnexpectedErrorDuringLogout)),
            "Unexpected error occurred during logout for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> UnexpectedErrorDuringRefresh =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(2003, nameof(UnexpectedErrorDuringRefresh)),
            "Unexpected error occurred during token refresh for user ID: {UserId}");

    private static readonly Action<ILogger, Exception?> NullUserLoginOrPassword =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(2004, nameof(NullUserLoginOrPassword)),
            "User login or password not set to an instance");

    // Information level logs - Successful operations
    private static readonly Action<ILogger, string, Exception?> UserLoggedInSuccessfully =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(3001, nameof(UserLoggedInSuccessfully)),
            "User logged in successfully: {Username}");

    private static readonly Action<ILogger, string, Exception?> UserLoggedOutSuccessfully =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(3002, nameof(UserLoggedOutSuccessfully)),
            "User logged out successfully: {UserId}");

    private static readonly Action<ILogger, string, Exception?> TokenRefreshedSuccessfully =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(3003, nameof(TokenRefreshedSuccessfully)),
            "Token refreshed successfully for user ID: {UserId}");

    // Public methods for Warning level logs
    public static void LogInvalidLoginAttempt(ILogger logger, string username, Exception? exception = null) =>
        InvalidLoginAttempt(logger, username, exception);

    public static void LogInvalidLogoutAttempt(ILogger logger, string userId, Exception? exception = null) =>
        InvalidLogoutAttempt(logger, userId, exception);

    public static void LogInvalidRefreshTokenAttempt(ILogger logger, string userId, Exception? exception = null) =>
        InvalidRefreshTokenAttempt(logger, userId, exception);

    public static void LogNullLoginRequest(ILogger logger, Exception? exception = null) =>
        NullLoginRequest(logger, exception);

    public static void LogNullLogoutRequest(ILogger logger, Exception? exception = null) =>
        NullLogoutRequest(logger, exception);

    public static void LogNullRefreshTokenRequest(ILogger logger, Exception? exception = null) =>
        NullRefreshTokenRequest(logger, exception);

    // Public methods for Error level logs
    public static void LogUnexpectedErrorDuringLogin(ILogger logger, string username, Exception exception) =>
        UnexpectedErrorDuringLogin(logger, username, exception);

    public static void LogUnexpectedErrorDuringLogout(ILogger logger, string userId, Exception exception) =>
        UnexpectedErrorDuringLogout(logger, userId, exception);

    public static void LogUnexpectedErrorDuringRefresh(ILogger logger, string userId, Exception exception) =>
        UnexpectedErrorDuringRefresh(logger, userId, exception);

    public static void LogNullUserLoginOrPassword(ILogger logger, Exception? exception = null) =>
        NullUserLoginOrPassword(logger, exception);

    // Public methods for Information level logs
    public static void LogUserLoggedInSuccessfully(ILogger logger, string username) =>
        UserLoggedInSuccessfully(logger, username, null);

    public static void LogUserLoggedOutSuccessfully(ILogger logger, string userId) =>
        UserLoggedOutSuccessfully(logger, userId, null);

    public static void LogTokenRefreshedSuccessfully(ILogger logger, string userId) =>
        TokenRefreshedSuccessfully(logger, userId, null);
}

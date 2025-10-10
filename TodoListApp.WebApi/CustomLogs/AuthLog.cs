namespace TodoListApp.WebApi.CustomLogs;

/// <summary>
/// Static class for authentication-related logging.
/// </summary>
internal static class AuthLog
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

    /// <summary>
    /// Logs a warning about an invalid login attempt.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username that attempted to login.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogInvalidLoginAttempt(ILogger logger, string username, Exception? exception = null) =>
        InvalidLoginAttempt(logger, username, exception);

    /// <summary>
    /// Logs a warning about an invalid logout attempt.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The user ID that attempted to logout.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogInvalidLogoutAttempt(ILogger logger, string userId, Exception? exception = null) =>
        InvalidLogoutAttempt(logger, userId, exception);

    /// <summary>
    /// Logs a warning about an invalid refresh token attempt.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The user ID that attempted to refresh the token.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogInvalidRefreshTokenAttempt(ILogger logger, string userId, Exception? exception = null) =>
        InvalidRefreshTokenAttempt(logger, userId, exception);

    /// <summary>
    /// Logs a warning that a null login request was received.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogNullLoginRequest(ILogger logger, Exception? exception = null) =>
        NullLoginRequest(logger, exception);

    /// <summary>
    /// Logs a warning that a null logout request was received.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogNullLogoutRequest(ILogger logger, Exception? exception = null) =>
        NullLogoutRequest(logger, exception);

    /// <summary>
    /// Logs a warning that a null refresh token request was received.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogNullRefreshTokenRequest(ILogger logger, Exception? exception = null) =>
        NullRefreshTokenRequest(logger, exception);

    // Public methods for Error level logs

    /// <summary>
    /// Logs an error that an unexpected exception occurred during login.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username attempting to login.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorDuringLogin(ILogger logger, string username, Exception exception) =>
        UnexpectedErrorDuringLogin(logger, username, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred during logout.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The user ID attempting to logout.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorDuringLogout(ILogger logger, string userId, Exception exception) =>
        UnexpectedErrorDuringLogout(logger, userId, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred during token refresh.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The user ID attempting to refresh the token.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorDuringRefresh(ILogger logger, string userId, Exception exception) =>
        UnexpectedErrorDuringRefresh(logger, userId, exception);

    /// <summary>
    /// Logs an error that the user login or password was not set to an instance.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogNullUserLoginOrPassword(ILogger logger, Exception? exception = null) =>
        NullUserLoginOrPassword(logger, exception);

    // Public methods for Information level logs

    /// <summary>
    /// Logs that a user has successfully logged in.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username that logged in successfully.</param>
    public static void LogUserLoggedInSuccessfully(ILogger logger, string username) =>
        UserLoggedInSuccessfully(logger, username, null);

    /// <summary>
    /// Logs that a user has successfully logged out.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The user ID that logged out successfully.</param>
    public static void LogUserLoggedOutSuccessfully(ILogger logger, string userId) =>
        UserLoggedOutSuccessfully(logger, userId, null);

    /// <summary>
    /// Logs that a token has been successfully refreshed.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The user ID for which the token was refreshed.</param>
    public static void LogTokenRefreshedSuccessfully(ILogger logger, string userId) =>
        TokenRefreshedSuccessfully(logger, userId, null);
}

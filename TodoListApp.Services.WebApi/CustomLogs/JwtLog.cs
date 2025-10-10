using Microsoft.Extensions.Logging;

namespace TodoListApp.Services.WebApi.CustomLogs;

/// <summary>
/// Static class for structured logging related to JWT operations.
/// </summary>
public static class JwtLog
{
    // Information level logs - Successful operations
    private static readonly Action<ILogger, string, Exception?> JwtTokenSuccess =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1101, nameof(JwtTokenSuccess)),
            "JWT token successfully retrieved for user: {Username}");

    private static readonly Action<ILogger, Exception?> JwtTokenRemovalSuccess =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1102, nameof(JwtTokenRemovalSuccess)),
            "JWT token successfully removed");

    private static readonly Action<ILogger, string, Exception?> RefreshTokenSuccess =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1103, nameof(RefreshTokenSuccess)),
            "Refresh token successfully retrieved for user ID: {UserId}");

    // Warning level logs - Expected failures
    private static readonly Action<ILogger, string, Exception?> JwtTokenInvalid =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2101, nameof(JwtTokenInvalid)),
            "JWT token response was null or invalid for user: {Username}");

    private static readonly Action<ILogger, string, int, string, Exception?> JwtTokenFailed =
        LoggerMessage.Define<string, int, string>(
            LogLevel.Warning,
            new EventId(2102, nameof(JwtTokenFailed)),
            "JWT token request failed for user: {Username}. Status: {StatusCode}, Error: {ErrorMessage}");

    private static readonly Action<ILogger, int, string, Exception?> JwtTokenFailedToRemove =
        LoggerMessage.Define<int, string>(
            LogLevel.Warning,
            new EventId(2103, nameof(JwtTokenFailedToRemove)),
            "JWT token removal request failed. Status: {StatusCode}, Error: {ErrorMessage}");

    private static readonly Action<ILogger, string, int, string, Exception?> RefreshTokenFailed =
        LoggerMessage.Define<string, int, string>(
            LogLevel.Warning,
            new EventId(2104, nameof(RefreshTokenFailed)),
            "Refresh token request failed for user ID: {UserId}. Status: {StatusCode}, Error: {ErrorMessage}");

    private static readonly Action<ILogger, string, Exception?> RefreshTokenInvalid =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2105, nameof(RefreshTokenInvalid)),
            "Refresh token response was null or invalid for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> NullOrEmptyUsername =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2106, nameof(NullOrEmptyUsername)),
            "Login request with null or empty username: {Username}");

    private static readonly Action<ILogger, Exception?> NullLoginRequest =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2107, nameof(NullLoginRequest)),
            "Null login request received");

    private static readonly Action<ILogger, Exception?> NullLogoutRequest =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2108, nameof(NullLogoutRequest)),
            "Null logout request received");

    private static readonly Action<ILogger, Exception?> NullRefreshTokenRequest =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2109, nameof(NullRefreshTokenRequest)),
            "Null refresh token request received");

    // Error level logs - System failures
    private static readonly Action<ILogger, string, Exception?> HttpRequestError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3101, nameof(HttpRequestError)),
            "HTTP request error occurred while requesting JWT token for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> TimeoutError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3102, nameof(TimeoutError)),
            "Timeout occurred while requesting JWT token for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> JsonParsingError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3103, nameof(JsonParsingError)),
            "JSON parsing error while processing JWT token response for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> UnexpectedLoginError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3104, nameof(UnexpectedLoginError)),
            "Unexpected error occurred while requesting JWT token for user: {Username}");

    private static readonly Action<ILogger, Exception?> UnexpectedLogoutError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3105, nameof(UnexpectedLogoutError)),
            "Unexpected error occurred during logout operation");

    private static readonly Action<ILogger, string, Exception?> UnexpectedRefreshError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3106, nameof(UnexpectedRefreshError)),
            "Unexpected error occurred while refreshing token for user ID: {UserId}");

    private static readonly Action<ILogger, Exception?> HttpClientError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3107, nameof(HttpClientError)),
            "HTTP client error occurred during authentication request");

    private static readonly Action<ILogger, string, Exception?> TaskCancelledError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3108, nameof(TaskCancelledError)),
            "Authentication request was cancelled for user: {Username}");

    // Public methods for Information level logs

    /// <summary>
    /// Logs that a JWT token has been successfully retrieved for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for whom the token was retrieved.</param>
    public static void LogJwtTokenSuccess(ILogger logger, string username) =>
        JwtTokenSuccess(logger, username, null);

    /// <summary>
    /// Logs that a JWT token has been successfully removed.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogJwtTokenRemovalSuccess(ILogger logger) =>
        JwtTokenRemovalSuccess(logger, null);

    /// <summary>
    /// Logs that a refresh token has been successfully retrieved for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user for whom the refresh token was retrieved.</param>
    public static void LogRefreshTokenSuccess(ILogger logger, string userId) =>
        RefreshTokenSuccess(logger, userId, null);

    // Public methods for Warning level logs

    /// <summary>
    /// Logs a warning that a JWT token response was null or invalid.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for whom the token was invalid.</param>
    public static void LogJwtTokenInvalid(ILogger logger, string username) =>
        JwtTokenInvalid(logger, username, null);

    /// <summary>
    /// Logs a warning that a JWT token request failed with a status code and error message.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for whom the request failed.</param>
    /// <param name="statusCode">The HTTP status code returned.</param>
    /// <param name="errorMessage">The error message returned.</param>
    public static void LogJwtTokenFailed(ILogger logger, string username, int statusCode, string errorMessage) =>
        JwtTokenFailed(logger, username, statusCode, errorMessage, null);

    /// <summary>
    /// Logs a warning that a JWT token removal request failed.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="statusCode">The HTTP status code returned.</param>
    /// <param name="errorMessage">The error message returned.</param>
    public static void LogJwtTokenFailedToRemove(ILogger logger, int statusCode, string errorMessage) =>
        JwtTokenFailedToRemove(logger, statusCode, errorMessage, null);

    /// <summary>
    /// Logs a warning that a refresh token request failed.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user for whom the request failed.</param>
    /// <param name="statusCode">The HTTP status code returned.</param>
    /// <param name="errorMessage">The error message returned.</param>
    public static void LogRefreshTokenFailed(ILogger logger, string userId, int statusCode, string errorMessage) =>
        RefreshTokenFailed(logger, userId, statusCode, errorMessage, null);

    /// <summary>
    /// Logs a warning that a refresh token response was null or invalid.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user for whom the token was invalid.</param>
    public static void LogRefreshTokenInvalid(ILogger logger, string userId) =>
        RefreshTokenInvalid(logger, userId, null);

    /// <summary>
    /// Logs a warning that a login request was received with a null or empty username.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The null or empty username.</param>
    public static void LogNullOrEmptyUsername(ILogger logger, string username) =>
        NullOrEmptyUsername(logger, username, null);

    /// <summary>
    /// Logs a warning that a null login request was received.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogNullLoginRequest(ILogger logger) =>
        NullLoginRequest(logger, null);

    /// <summary>
    /// Logs a warning that a null logout request was received.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogNullLogoutRequest(ILogger logger) =>
        NullLogoutRequest(logger, null);

    /// <summary>
    /// Logs a warning that a null refresh token request was received.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogNullRefreshTokenRequest(ILogger logger) =>
        NullRefreshTokenRequest(logger, null);

    // Public methods for Error level logs

    /// <summary>
    /// Logs an error that an HTTP request error occurred while requesting a JWT token.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for whom the request was made.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogHttpRequestError(ILogger logger, string username, Exception exception) =>
        HttpRequestError(logger, username, exception);

    /// <summary>
    /// Logs an error that a timeout occurred while requesting a JWT token.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for whom the request was made.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogTimeoutError(ILogger logger, string username, Exception exception) =>
        TimeoutError(logger, username, exception);

    /// <summary>
    /// Logs an error that a JSON parsing error occurred while processing a JWT token response.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for whom the response was being processed.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogJsonParsingError(ILogger logger, string username, Exception exception) =>
        JsonParsingError(logger, username, exception);

    /// <summary>
    /// Logs an error that an unexpected error occurred while requesting a JWT token.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for whom the request was made.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedLoginError(ILogger logger, string username, Exception exception) =>
        UnexpectedLoginError(logger, username, exception);

    /// <summary>
    /// Logs an error that an unexpected error occurred during a logout operation.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedLogoutError(ILogger logger, Exception exception) =>
        UnexpectedLogoutError(logger, exception);

    /// <summary>
    /// Logs an error that an unexpected error occurred while refreshing a token.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user for whom the token was being refreshed.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedRefreshError(ILogger logger, string userId, Exception exception) =>
        UnexpectedRefreshError(logger, userId, exception);

    /// <summary>
    /// Logs an error that an HTTP client error occurred during an authentication request.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogHttpClientError(ILogger logger, Exception exception) =>
        HttpClientError(logger, exception);

    /// <summary>
    /// Logs an error that an authentication request was cancelled.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for whom the request was cancelled.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogTaskCancelledError(ILogger logger, string username, Exception exception) =>
        TaskCancelledError(logger, username, exception);
}

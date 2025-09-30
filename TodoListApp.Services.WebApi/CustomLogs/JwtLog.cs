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
            new EventId(1001, nameof(JwtTokenSuccess)),
            "JWT token successfully retrieved for user: {Username}");

    private static readonly Action<ILogger, Exception?> JwtTokenRemovalSuccess =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1002, nameof(JwtTokenRemovalSuccess)),
            "JWT token successfully removed");

    private static readonly Action<ILogger, string, Exception?> RefreshTokenSuccess =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1003, nameof(RefreshTokenSuccess)),
            "Refresh token successfully retrieved for user ID: {UserId}");

    // Warning level logs - Expected failures
    private static readonly Action<ILogger, string, Exception?> JwtTokenInvalid =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2001, nameof(JwtTokenInvalid)),
            "JWT token response was null or invalid for user: {Username}");

    private static readonly Action<ILogger, string, int, string, Exception?> JwtTokenFailed =
        LoggerMessage.Define<string, int, string>(
            LogLevel.Warning,
            new EventId(2002, nameof(JwtTokenFailed)),
            "JWT token request failed for user: {Username}. Status: {StatusCode}, Error: {ErrorMessage}");

    private static readonly Action<ILogger, int, string, Exception?> JwtTokenFailedToRemove =
        LoggerMessage.Define<int, string>(
            LogLevel.Warning,
            new EventId(2003, nameof(JwtTokenFailedToRemove)),
            "JWT token removal request failed. Status: {StatusCode}, Error: {ErrorMessage}");

    private static readonly Action<ILogger, string, int, string, Exception?> RefreshTokenFailed =
        LoggerMessage.Define<string, int, string>(
            LogLevel.Warning,
            new EventId(2004, nameof(RefreshTokenFailed)),
            "Refresh token request failed for user ID: {UserId}. Status: {StatusCode}, Error: {ErrorMessage}");

    private static readonly Action<ILogger, string, Exception?> RefreshTokenInvalid =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2005, nameof(RefreshTokenInvalid)),
            "Refresh token response was null or invalid for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> NullOrEmptyUsername =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2006, nameof(NullOrEmptyUsername)),
            "Login request with null or empty username: {Username}");

    private static readonly Action<ILogger, Exception?> NullLoginRequest =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2007, nameof(NullLoginRequest)),
            "Null login request received");

    private static readonly Action<ILogger, Exception?> NullLogoutRequest =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2008, nameof(NullLogoutRequest)),
            "Null logout request received");

    private static readonly Action<ILogger, Exception?> NullRefreshTokenRequest =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2009, nameof(NullRefreshTokenRequest)),
            "Null refresh token request received");

    // Error level logs - System failures
    private static readonly Action<ILogger, string, Exception?> HttpRequestError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3001, nameof(HttpRequestError)),
            "HTTP request error occurred while requesting JWT token for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> TimeoutError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3002, nameof(TimeoutError)),
            "Timeout occurred while requesting JWT token for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> JsonParsingError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3003, nameof(JsonParsingError)),
            "JSON parsing error while processing JWT token response for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> UnexpectedLoginError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3004, nameof(UnexpectedLoginError)),
            "Unexpected error occurred while requesting JWT token for user: {Username}");

    private static readonly Action<ILogger, Exception?> UnexpectedLogoutError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3005, nameof(UnexpectedLogoutError)),
            "Unexpected error occurred during logout operation");

    private static readonly Action<ILogger, string, Exception?> UnexpectedRefreshError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3006, nameof(UnexpectedRefreshError)),
            "Unexpected error occurred while refreshing token for user ID: {UserId}");

    private static readonly Action<ILogger, Exception?> HttpClientError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3007, nameof(HttpClientError)),
            "HTTP client error occurred during authentication request");

    private static readonly Action<ILogger, string, Exception?> TaskCancelledError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3008, nameof(TaskCancelledError)),
            "Authentication request was cancelled for user: {Username}");

    // Public methods for Information level logs
    public static void LogJwtTokenSuccess(ILogger logger, string username) =>
        JwtTokenSuccess(logger, username, null);

    public static void LogJwtTokenRemovalSuccess(ILogger logger) =>
        JwtTokenRemovalSuccess(logger, null);

    public static void LogRefreshTokenSuccess(ILogger logger, string userId) =>
        RefreshTokenSuccess(logger, userId, null);

    // Public methods for Warning level logs
    public static void LogJwtTokenInvalid(ILogger logger, string username) =>
        JwtTokenInvalid(logger, username, null);

    public static void LogJwtTokenFailed(ILogger logger, string username, int statusCode, string errorMessage) =>
        JwtTokenFailed(logger, username, statusCode, errorMessage, null);

    public static void LogJwtTokenFailedToRemove(ILogger logger, int statusCode, string errorMessage) =>
        JwtTokenFailedToRemove(logger, statusCode, errorMessage, null);

    public static void LogRefreshTokenFailed(ILogger logger, string userId, int statusCode, string errorMessage) =>
        RefreshTokenFailed(logger, userId, statusCode, errorMessage, null);

    public static void LogRefreshTokenInvalid(ILogger logger, string userId) =>
        RefreshTokenInvalid(logger, userId, null);

    public static void LogNullOrEmptyUsername(ILogger logger, string username) =>
        NullOrEmptyUsername(logger, username, null);

    public static void LogNullLoginRequest(ILogger logger) =>
        NullLoginRequest(logger, null);

    public static void LogNullLogoutRequest(ILogger logger) =>
        NullLogoutRequest(logger, null);

    public static void LogNullRefreshTokenRequest(ILogger logger) =>
        NullRefreshTokenRequest(logger, null);

    // Public methods for Error level logs
    public static void LogHttpRequestError(ILogger logger, string username, Exception exception) =>
        HttpRequestError(logger, username, exception);

    public static void LogTimeoutError(ILogger logger, string username, Exception exception) =>
        TimeoutError(logger, username, exception);

    public static void LogJsonParsingError(ILogger logger, string username, Exception exception) =>
        JsonParsingError(logger, username, exception);

    public static void LogUnexpectedLoginError(ILogger logger, string username, Exception exception) =>
        UnexpectedLoginError(logger, username, exception);

    public static void LogUnexpectedLogoutError(ILogger logger, Exception exception) =>
        UnexpectedLogoutError(logger, exception);

    public static void LogUnexpectedRefreshError(ILogger logger, string userId, Exception exception) =>
        UnexpectedRefreshError(logger, userId, exception);

    public static void LogHttpClientError(ILogger logger, Exception exception) =>
        HttpClientError(logger, exception);

    public static void LogTaskCancelledError(ILogger logger, string username, Exception exception) =>
        TaskCancelledError(logger, username, exception);
}

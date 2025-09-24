using Microsoft.Extensions.Logging;

namespace TodoListApp.Services.WebApi.CustomLogs;
public static class Log
{
    private static readonly Action<ILogger, string, Exception?> JwtTokenSuccess =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1, nameof(JwtTokenSuccess)),
            "JWT token successfully retrieved for user {Login}");

    private static readonly Action<ILogger, string, Exception?> JwtTokenInvalid =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2, nameof(JwtTokenInvalid)),
            "JWT token response was null or invalid for user {Login}");

    private static readonly Action<ILogger, string, object, string, Exception?> JwtTokenFailed =
        LoggerMessage.Define<string, object, string>(
            LogLevel.Warning,
            new EventId(3, nameof(JwtTokenFailed)),
            "JWT token request failed for user {Login}. Status: {StatusCode}, Error: {Error}");

    private static readonly Action<ILogger, string, Exception?> HttpError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(4, nameof(HttpError)),
            "HTTP error occurred while requesting JWT token for user {Login}");

    private static readonly Action<ILogger, string, Exception?> Timeout =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(5, nameof(Timeout)),
            "Timeout occurred while requesting JWT token for user {Login}");

    private static readonly Action<ILogger, string, Exception?> JsonError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(6, nameof(JsonError)),
            "JSON parsing error while processing JWT token response for user {Login}");

    private static readonly Action<ILogger, string, Exception?> UnexpectedError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(7, nameof(UnexpectedError)),
            "Unexpected error while requesting JWT token for user {Login}");

    private static readonly Action<ILogger, Exception?> SessionStore =
        LoggerMessage.Define(
            LogLevel.Debug,
            new EventId(8, nameof(SessionStore)),
            "JWT token stored in session");

    private static readonly Action<ILogger, bool, Exception?> SessionRetrieved =
        LoggerMessage.Define<bool>(
            LogLevel.Debug,
            new EventId(9, nameof(SessionRetrieved)),
            "JWT token retrieved from session: {HasToken}");

    private static readonly Action<ILogger, Exception?> SessionRemoved =
        LoggerMessage.Define(
            LogLevel.Debug,
            new EventId(10, nameof(SessionRemoved)),
            "JWT token removed from session");

    private static readonly Action<ILogger, Exception?> SessionUnavailable =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(11, nameof(SessionUnavailable)),
            "Unable to access session");

    public static void LogJwtTokenSuccess(ILogger logger, string login) => JwtTokenSuccess(logger, login, null);

    public static void LogJwtTokenInvalid(ILogger logger, string login) => JwtTokenInvalid(logger, login, null);

    public static void LogJwtTokenFailed(ILogger logger, string login, object statusCode, string error) => JwtTokenFailed(logger, login, statusCode, error, null);

    public static void LogHttpError(ILogger logger, string login, Exception ex) => HttpError(logger, login, ex);

    public static void LogTimeout(ILogger logger, string login, Exception ex) => Timeout(logger, login, ex);

    public static void LogJsonError(ILogger logger, string login, Exception ex) => JsonError(logger, login, ex);

    public static void LogUnexpectedError(ILogger logger, string login, Exception ex) => UnexpectedError(logger, login, ex);

    public static void LogSessionStore(ILogger logger) => SessionStore(logger, null);

    public static void LogSessionRetrieved(ILogger logger, bool hasToken) => SessionRetrieved(logger, hasToken, null);

    public static void LogSessionRemoved(ILogger logger) => SessionRemoved(logger, null);

    public static void LogSessionUnavailable(ILogger logger) => SessionUnavailable(logger, null);
}

namespace TodoListApp.WebApi.CustomLogs;

public static class AuthLog
{
    private static readonly Action<ILogger, Exception?> NullUserLoginOrPassword =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(1, nameof(NullUserLoginOrPassword)),
            "User login or password not set to an instance");

    private static readonly Action<ILogger, string, Exception?> UserNotFound =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(UserNotFound)),
            "Wrong login or password {Login}.");

    private static readonly Action<ILogger, string, Exception?> JwtTokenCreated =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1, nameof(UserNotFound)),
            "Jwt token for user {Id}.");

    private static readonly Action<ILogger, string, Exception?> FailedToCreateToken =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(UserNotFound)),
            "Failed to create token for user {Id}.");

    private static readonly Action<ILogger, string, Exception?> JwtTokenRemoved =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1, nameof(JwtTokenRemoved)),
            "Jwt token for user {Id} removed.");

    private static readonly Action<ILogger, string, Exception?> JwtTokenNotRemoved =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(JwtTokenNotRemoved)),
            "Failed to remove Jwt token for user {Id}.");

    private static readonly Action<ILogger, string, Exception?> JwtTokenRefreshed =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1, nameof(JwtTokenRefreshed)),
            "Jwt token refreshed for user {Id}.");

    public static void LogNullUserLoginOrPassword(ILogger logger) => NullUserLoginOrPassword(logger, null);

    public static void LogUserNotfound(ILogger logger, string login) => UserNotFound(logger, login, null);

    public static void LogJwtTokenCreated(ILogger logger, string id) => JwtTokenCreated(logger, id, null);

    public static void LogFailedToCreateToken(ILogger logger, string id) => FailedToCreateToken(logger, id, null);

    public static void LogJwtTokenRemoved(ILogger logger, string id) => JwtTokenRemoved(logger, id, null);

    public static void LogJwtTokenRefreshed(ILogger logger, string id) => JwtTokenRefreshed(logger, id, null);

    public static void LogJwtTokenNotRemoved(ILogger logger, string id) => JwtTokenNotRemoved(logger, id, null);
}

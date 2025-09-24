namespace TodoListApp.WebApi.CustomLogs;

public class Log
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
            "Jwt token for user {Login}.");

    private static readonly Action<ILogger, string, Exception?> FailedToCreateToken =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(UserNotFound)),
            "Failed to create token for user {Login}.");

    public static void LogNullUserLoginOrPassword(ILogger logger) => NullUserLoginOrPassword(logger, null);

    public static void LogUserNotfound(ILogger logger, string login) => UserNotFound(logger, login, null);

    public static void LogJwtTokenCreated(ILogger logger, string login) => JwtTokenCreated(logger, login, null);

    public static void LogFailedToCreateToken(ILogger logger, string login) => FailedToCreateToken(logger, login, null);
}

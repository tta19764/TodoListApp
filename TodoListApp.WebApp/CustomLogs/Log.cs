namespace TodoListApp.WebApp.CustomLogs;

public static class Log
{
    private static readonly Action<ILogger, string, Exception?> UserLoggedIn =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1, nameof(UserLoggedIn)),
            "User {User} logged in with Identity");

    private static readonly Action<ILogger, string, Exception?> JwtTokenStored =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(2, nameof(JwtTokenStored)),
            "JWT token successfully obtained and stored for user {User}");

    private static readonly Action<ILogger, string, string, Exception?> JwtTokenFailed =
        LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            new EventId(3, nameof(JwtTokenFailed)),
            "JWT token retrieval failed for user {User}: {Error}. API calls may not work.");

    private static readonly Action<ILogger, Exception?> UserLockedOut =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(4, nameof(UserLockedOut)),
            "User account locked out.");

    public static void LogUserLoggedIn(ILogger logger, string user) => UserLoggedIn(logger, user, null);

    public static void LogJwtTokenStored(ILogger logger, string user) => JwtTokenStored(logger, user, null);

    public static void LogJwtTokenFailed(ILogger logger, string user, string error) => JwtTokenFailed(logger, user, error, null);

    public static void LogUserLockedOut(ILogger logger) => UserLockedOut(logger, null);
}

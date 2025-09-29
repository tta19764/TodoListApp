namespace TodoListApp.WebApp.CustomLogs;

public static class AccountLog
{
    private static readonly Action<ILogger, string, Exception?> UserLoggedIn =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1001, nameof(UserLoggedIn)),
            "User {Username} logged in successfully with Identity");

    private static readonly Action<ILogger, string, Exception?> JwtTokenStored =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1002, nameof(JwtTokenStored)),
            "JWT token successfully obtained and stored for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> JwtTokenRemoved =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1003, nameof(JwtTokenRemoved)),
            "JWT token successfully removed for user: {Username}");

    private static readonly Action<ILogger, Exception?> UserLoggedOut =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1004, nameof(UserLoggedOut)),
            "User logged out successfully");

    private static readonly Action<ILogger, string, Exception?> JwtTokenRefreshed =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1005, nameof(JwtTokenRefreshed)),
            "JWT token successfully refreshed for user: {Username}");

    private static readonly Action<ILogger, Uri, Exception?> LoginPageAccessed =
        LoggerMessage.Define<Uri>(
            LogLevel.Information,
            new EventId(1006, nameof(LoginPageAccessed)),
            "Login page accessed with return URL: {ReturnUrl}");

    private static readonly Action<ILogger, string, Exception?> TokenRetrievedFromStorage =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1007, nameof(TokenRetrievedFromStorage)),
            "Token retrieved from storage for user ID: {UserId}");

    // Warning level logs - Expected failures
    private static readonly Action<ILogger, string, Exception?> JwtTokenStoreFailed =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2001, nameof(JwtTokenStoreFailed)),
            "Failed to store JWT token for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> JwtTokenRemoveFailed =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2002, nameof(JwtTokenRemoveFailed)),
            "Failed to remove JWT token for user: {Username}");

    private static readonly Action<ILogger, Exception?> UserLockedOut =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2003, nameof(UserLockedOut)),
            "User account is locked out");

    private static readonly Action<ILogger, string, Exception?> JwtTokenRefreshFailed =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2004, nameof(JwtTokenRefreshFailed)),
            "JWT token refresh failed for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> InvalidLoginAttempt =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2005, nameof(InvalidLoginAttempt)),
            "Invalid login attempt for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> ApiAuthenticationFailed =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2006, nameof(ApiAuthenticationFailed)),
            "API authentication failed for user: {Username}. Token retrieval unsuccessful");

    private static readonly Action<ILogger, Exception?> InvalidModelState =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2007, nameof(InvalidModelState)),
            "Login attempt with invalid model state");

    private static readonly Action<ILogger, Exception?> NullLoginViewModel =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2008, nameof(NullLoginViewModel)),
            "Null login view model received");

    private static readonly Action<ILogger, string, Exception?> UserNotFoundInStorage =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2009, nameof(UserNotFoundInStorage)),
            "User not found in storage for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> TokenNotFoundInStorage =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2010, nameof(TokenNotFoundInStorage)),
            "Token not found in storage for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> InvalidUserIdFormat =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2011, nameof(InvalidUserIdFormat)),
            "Invalid user ID format: {UserId}");

    // Error level logs - System failures
    private static readonly Action<ILogger, string, Exception?> UnexpectedErrorDuringLogin =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3001, nameof(UnexpectedErrorDuringLogin)),
            "Unexpected error occurred during login for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> UnexpectedErrorDuringLogout =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3002, nameof(UnexpectedErrorDuringLogout)),
            "Unexpected error occurred during logout for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> UnexpectedErrorStoringToken =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3003, nameof(UnexpectedErrorStoringToken)),
            "Unexpected error occurred while storing token for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> UnexpectedErrorRemovingToken =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3004, nameof(UnexpectedErrorRemovingToken)),
            "Unexpected error occurred while removing token for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> UnexpectedErrorRetrievingToken =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3005, nameof(UnexpectedErrorRetrievingToken)),
            "Unexpected error occurred while retrieving token for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> DatabaseErrorRemovingToken =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3006, nameof(DatabaseErrorRemovingToken)),
            "Database error occurred while removing token for user ID: {UserId}");

    private static readonly Action<ILogger, string, Exception?> IdentitySignInFailed =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3007, nameof(IdentitySignInFailed)),
            "Identity sign-in failed for user: {Username}");

    // Public methods for Information level logs
    public static void LogUserLoggedIn(ILogger logger, string username) =>
        UserLoggedIn(logger, username, null);

    public static void LogJwtTokenStored(ILogger logger, string username) =>
        JwtTokenStored(logger, username, null);

    public static void LogJwtTokenRemoved(ILogger logger, string username) =>
        JwtTokenRemoved(logger, username, null);

    public static void LogUserLoggedOut(ILogger logger) =>
        UserLoggedOut(logger, null);

    public static void LogJwtTokenRefreshed(ILogger logger, string username) =>
        JwtTokenRefreshed(logger, username, null);

    public static void LogLoginPageAccessed(ILogger logger, Uri returnUrl) =>
        LoginPageAccessed(logger, returnUrl, null);

    public static void LogTokenRetrievedFromStorage(ILogger logger, string userId) =>
        TokenRetrievedFromStorage(logger, userId, null);

    // Public methods for Warning level logs
    public static void LogJwtTokenStoreFailed(ILogger logger, string username) =>
        JwtTokenStoreFailed(logger, username, null);

    public static void LogJwtTokenRemoveFailed(ILogger logger, string username) =>
        JwtTokenRemoveFailed(logger, username, null);

    public static void LogUserLockedOut(ILogger logger) =>
        UserLockedOut(logger, null);

    public static void LogJwtTokenRefreshFailed(ILogger logger, string username) =>
        JwtTokenRefreshFailed(logger, username, null);

    public static void LogInvalidLoginAttempt(ILogger logger, string username) =>
        InvalidLoginAttempt(logger, username, null);

    public static void LogApiAuthenticationFailed(ILogger logger, string username) =>
        ApiAuthenticationFailed(logger, username, null);

    public static void LogInvalidModelState(ILogger logger) =>
        InvalidModelState(logger, null);

    public static void LogNullLoginViewModel(ILogger logger) =>
        NullLoginViewModel(logger, null);

    public static void LogUserNotFoundInStorage(ILogger logger, string userId) =>
        UserNotFoundInStorage(logger, userId, null);

    public static void LogTokenNotFoundInStorage(ILogger logger, string userId) =>
        TokenNotFoundInStorage(logger, userId, null);

    public static void LogInvalidUserIdFormat(ILogger logger, string userId) =>
        InvalidUserIdFormat(logger, userId, null);

    // Public methods for Error level logs
    public static void LogUnexpectedErrorDuringLogin(ILogger logger, string username, Exception exception) =>
        UnexpectedErrorDuringLogin(logger, username, exception);

    public static void LogUnexpectedErrorDuringLogout(ILogger logger, string username, Exception exception) =>
        UnexpectedErrorDuringLogout(logger, username, exception);

    public static void LogUnexpectedErrorStoringToken(ILogger logger, string userId, Exception exception) =>
        UnexpectedErrorStoringToken(logger, userId, exception);

    public static void LogUnexpectedErrorRemovingToken(ILogger logger, string userId, Exception exception) =>
        UnexpectedErrorRemovingToken(logger, userId, exception);

    public static void LogUnexpectedErrorRetrievingToken(ILogger logger, string userId, Exception exception) =>
        UnexpectedErrorRetrievingToken(logger, userId, exception);

    public static void LogDatabaseErrorRemovingToken(ILogger logger, string userId, Exception exception) =>
        DatabaseErrorRemovingToken(logger, userId, exception);

    public static void LogIdentitySignInFailed(ILogger logger, string username, Exception exception) =>
        IdentitySignInFailed(logger, username, exception);
}

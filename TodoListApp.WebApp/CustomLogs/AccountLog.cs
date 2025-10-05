using Microsoft.Extensions.Logging;

namespace TodoListApp.WebApp.CustomLogs;

public static class AccountLog
{
    // Information level logs - Successful operations
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

    private static readonly Action<ILogger, Uri, Exception?> RegisterPageAccessed =
        LoggerMessage.Define<Uri>(
            LogLevel.Information,
            new EventId(1008, nameof(RegisterPageAccessed)),
            "Register page accessed with return URL: {ReturnUrl}");

    private static readonly Action<ILogger, string, Exception?> UserRegistered =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1009, nameof(UserRegistered)),
            "User {Username} registered successfully with Identity");

    private static readonly Action<ILogger, string, Exception?> PasswordResetTokenGenerated =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1010, nameof(PasswordResetTokenGenerated)),
            "Password reset token generated for user: {Email}");

    private static readonly Action<ILogger, string, Exception?> PasswordResetEmailSent =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1011, nameof(PasswordResetEmailSent)),
            "Password reset email sent to: {Email}");

    private static readonly Action<ILogger, string, Exception?> PasswordResetSuccessful =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1012, nameof(PasswordResetSuccessful)),
            "Password reset successful for user: {Email}");

    private static readonly Action<ILogger, Exception?> ForgotPasswordPageAccessed =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1013, nameof(ForgotPasswordPageAccessed)),
            "Forgot password page accessed");

    private static readonly Action<ILogger, string, Exception?> ResetPasswordPageAccessed =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1014, nameof(ResetPasswordPageAccessed)),
            "Reset password page accessed for email: {Email}");

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
            "Request with invalid model state");

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

    private static readonly Action<ILogger, Exception?> NullRegisterViewModel =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2012, nameof(NullRegisterViewModel)),
            "Null register view model received");

    private static readonly Action<ILogger, string, string, Exception?> RegistrationFailedWithErrors =
        LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            new EventId(2013, nameof(RegistrationFailedWithErrors)),
            "Registration failed for user {Login} with errors: {Errors}");

    private static readonly Action<ILogger, string, Exception?> UserNotFoundForPasswordReset =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2014, nameof(UserNotFoundForPasswordReset)),
            "Password reset requested for non-existent email: {Email}");

    private static readonly Action<ILogger, string, Exception?> InvalidPasswordResetToken =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2015, nameof(InvalidPasswordResetToken)),
            "Invalid password reset token used for email: {Email}");

    private static readonly Action<ILogger, string, string, Exception?> PasswordResetFailedWithErrors =
        LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            new EventId(2016, nameof(PasswordResetFailedWithErrors)),
            "Password reset failed for user {Email} with errors: {Errors}");

    private static readonly Action<ILogger, Exception?> NullForgotPasswordViewModel =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2017, nameof(NullForgotPasswordViewModel)),
            "Null forgot password view model received");

    private static readonly Action<ILogger, Exception?> NullResetPasswordViewModel =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2018, nameof(NullResetPasswordViewModel)),
            "Null reset password view model received");

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

    private static readonly Action<ILogger, string, Exception?> UnexpectedErrorDuringRegister =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3008, nameof(UnexpectedErrorDuringRegister)),
            "Unexpected error occurred during register for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> IdentityRegisterFailed =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3009, nameof(IdentityRegisterFailed)),
            "Identity register failed for user: {Username}");

    private static readonly Action<ILogger, string, Exception?> UnexpectedErrorGeneratingResetToken =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3010, nameof(UnexpectedErrorGeneratingResetToken)),
            "Unexpected error occurred while generating password reset token for: {Email}");

    private static readonly Action<ILogger, string, Exception?> UnexpectedErrorSendingResetEmail =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3011, nameof(UnexpectedErrorSendingResetEmail)),
            "Unexpected error occurred while sending password reset email to: {Email}");

    private static readonly Action<ILogger, string, Exception?> UnexpectedErrorDuringPasswordReset =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3012, nameof(UnexpectedErrorDuringPasswordReset)),
            "Unexpected error occurred during password reset for: {Email}");

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

    public static void LogRegisterPageAccessed(ILogger logger, Uri returnUrl) =>
        RegisterPageAccessed(logger, returnUrl, null);

    public static void LogUserRegistered(ILogger logger, string username) =>
        UserRegistered(logger, username, null);

    public static void LogPasswordResetTokenGenerated(ILogger logger, string email) =>
        PasswordResetTokenGenerated(logger, email, null);

    public static void LogPasswordResetEmailSent(ILogger logger, string email) =>
        PasswordResetEmailSent(logger, email, null);

    public static void LogPasswordResetSuccessful(ILogger logger, string email) =>
        PasswordResetSuccessful(logger, email, null);

    public static void LogForgotPasswordPageAccessed(ILogger logger) =>
        ForgotPasswordPageAccessed(logger, null);

    public static void LogResetPasswordPageAccessed(ILogger logger, string email) =>
        ResetPasswordPageAccessed(logger, email, null);

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

    public static void LogNullRegisterViewModel(ILogger logger) =>
        NullRegisterViewModel(logger, null);

    public static void LogRegistrationFailedWithErrors(ILogger logger, string login, string errors, Exception? exception = null) =>
        RegistrationFailedWithErrors(logger, login, errors, exception);

    public static void LogUserNotFoundForPasswordReset(ILogger logger, string email) =>
        UserNotFoundForPasswordReset(logger, email, null);

    public static void LogInvalidPasswordResetToken(ILogger logger, string email) =>
        InvalidPasswordResetToken(logger, email, null);

    public static void LogPasswordResetFailedWithErrors(ILogger logger, string email, string errors, Exception? exception = null) =>
        PasswordResetFailedWithErrors(logger, email, errors, exception);

    public static void LogNullForgotPasswordViewModel(ILogger logger) =>
        NullForgotPasswordViewModel(logger, null);

    public static void LogNullResetPasswordViewModel(ILogger logger) =>
        NullResetPasswordViewModel(logger, null);

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

    public static void LogIdentitySignInFailed(ILogger logger, string username, Exception? exception) =>
        IdentitySignInFailed(logger, username, exception);

    public static void LogIdentityRegisterFailed(ILogger logger, string username, Exception exception) =>
        IdentityRegisterFailed(logger, username, exception);

    public static void LogUnexpectedErrorDuringRegister(ILogger logger, string username, Exception exception) =>
        UnexpectedErrorDuringRegister(logger, username, exception);

    public static void LogUnexpectedErrorGeneratingResetToken(ILogger logger, string email, Exception exception) =>
        UnexpectedErrorGeneratingResetToken(logger, email, exception);

    public static void LogUnexpectedErrorSendingResetEmail(ILogger logger, string email, Exception exception) =>
        UnexpectedErrorSendingResetEmail(logger, email, exception);

    public static void LogUnexpectedErrorDuringPasswordReset(ILogger logger, string email, Exception exception) =>
        UnexpectedErrorDuringPasswordReset(logger, email, exception);
}

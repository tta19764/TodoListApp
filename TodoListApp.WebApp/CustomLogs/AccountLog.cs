namespace TodoListApp.WebApp.CustomLogs;

/// <summary>
/// Static class for structured logging in account-related operations.
/// </summary>
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

    private static readonly Action<ILogger, string, Exception?> PasswordResetLinkGenerationFailed =
    LoggerMessage.Define<string>(
        LogLevel.Warning,
        new EventId(2019, nameof(PasswordResetLinkGenerationFailed)),
        "Password reset link generation failed for email: {Email}");

    private static readonly Action<ILogger, Exception?> InvalidPasswordResetLink =
    LoggerMessage.Define(
        LogLevel.Warning,
        new EventId(2020, nameof(InvalidPasswordResetLink)),
        "Invalid password reset link accessed");

    private static readonly Action<ILogger, string, Exception?> PasswordResetFailedUserNotFound =
    LoggerMessage.Define<string>(
        LogLevel.Warning,
        new EventId(2021, nameof(PasswordResetFailedUserNotFound)),
        "Password reset failed - user not found: {Email}");

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

    /// <summary>
    /// Logs that a user has successfully logged in with Identity.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username of the logged in user.</param>
    public static void LogUserLoggedIn(ILogger logger, string username) =>
        UserLoggedIn(logger, username, null);

    /// <summary>
    /// Logs that a JWT token has been successfully obtained and stored for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for which the token was stored.</param>
    public static void LogJwtTokenStored(ILogger logger, string username) =>
        JwtTokenStored(logger, username, null);

    /// <summary>
    /// Logs that a JWT token has been successfully removed for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for which the token was removed.</param>
    public static void LogJwtTokenRemoved(ILogger logger, string username) =>
        JwtTokenRemoved(logger, username, null);

    /// <summary>
    /// Logs that a user has successfully logged out.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogUserLoggedOut(ILogger logger) =>
        UserLoggedOut(logger, null);

    /// <summary>
    /// Logs that a JWT token has been successfully refreshed for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for which the token was refreshed.</param>
    public static void LogJwtTokenRefreshed(ILogger logger, string username) =>
        JwtTokenRefreshed(logger, username, null);

    /// <summary>
    /// Logs that the login page has been accessed with a specific return URL.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="returnUrl">The return URL after successful login.</param>
    public static void LogLoginPageAccessed(ILogger logger, Uri returnUrl) =>
        LoginPageAccessed(logger, returnUrl, null);

    /// <summary>
    /// Logs that a token has been successfully retrieved from storage for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user whose token was retrieved.</param>
    public static void LogTokenRetrievedFromStorage(ILogger logger, string userId) =>
        TokenRetrievedFromStorage(logger, userId, null);

    /// <summary>
    /// Logs that the registration page has been accessed with a specific return URL.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="returnUrl">The return URL after successful registration.</param>
    public static void LogRegisterPageAccessed(ILogger logger, Uri returnUrl) =>
        RegisterPageAccessed(logger, returnUrl, null);

    /// <summary>
    /// Logs that a user has successfully registered with Identity.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username of the newly registered user.</param>
    public static void LogUserRegistered(ILogger logger, string username) =>
        UserRegistered(logger, username, null);

    /// <summary>
    /// Logs that a password reset token has been generated for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="email">The email address for which the token was generated.</param>
    public static void LogPasswordResetTokenGenerated(ILogger logger, string email) =>
        PasswordResetTokenGenerated(logger, email, null);

    /// <summary>
    /// Logs that a password reset email has been successfully sent.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="email">The email address to which the reset link was sent.</param>
    public static void LogPasswordResetEmailSent(ILogger logger, string email) =>
        PasswordResetEmailSent(logger, email, null);

    /// <summary>
    /// Logs that a password has been successfully reset for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="email">The email address of the user whose password was reset.</param>
    public static void LogPasswordResetSuccessful(ILogger logger, string email) =>
        PasswordResetSuccessful(logger, email, null);

    /// <summary>
    /// Logs that the forgot password page has been accessed.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogForgotPasswordPageAccessed(ILogger logger) =>
        ForgotPasswordPageAccessed(logger, null);

    /// <summary>
    /// Logs that the reset password page has been accessed for a specific email.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="email">The email address attempting to reset password.</param>
    public static void LogResetPasswordPageAccessed(ILogger logger, string email) =>
        ResetPasswordPageAccessed(logger, email, null);

    // Public methods for Warning level logs

    /// <summary>
    /// Logs a warning that storing a JWT token failed for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for which token storage failed.</param>
    public static void LogJwtTokenStoreFailed(ILogger logger, string username) =>
        JwtTokenStoreFailed(logger, username, null);

    /// <summary>
    /// Logs a warning that removing a JWT token failed for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for which token removal failed.</param>
    public static void LogJwtTokenRemoveFailed(ILogger logger, string username) =>
        JwtTokenRemoveFailed(logger, username, null);

    /// <summary>
    /// Logs a warning that a user account is locked out.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogUserLockedOut(ILogger logger) =>
        UserLockedOut(logger, null);

    /// <summary>
    /// Logs a warning that JWT token refresh failed for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for which token refresh failed.</param>
    public static void LogJwtTokenRefreshFailed(ILogger logger, string username) =>
        JwtTokenRefreshFailed(logger, username, null);

    /// <summary>
    /// Logs a warning about an invalid login attempt for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username that attempted to login.</param>
    public static void LogInvalidLoginAttempt(ILogger logger, string username) =>
        InvalidLoginAttempt(logger, username, null);

    /// <summary>
    /// Logs a warning that API authentication failed for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username for which API authentication failed.</param>
    public static void LogApiAuthenticationFailed(ILogger logger, string username) =>
        ApiAuthenticationFailed(logger, username, null);

    /// <summary>
    /// Logs a warning that a request was received with invalid model state.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogInvalidModelState(ILogger logger) =>
        InvalidModelState(logger, null);

    /// <summary>
    /// Logs a warning that a null login view model was received.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogNullLoginViewModel(ILogger logger) =>
        NullLoginViewModel(logger, null);

    /// <summary>
    /// Logs a warning that a user was not found in storage.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user that was not found.</param>
    public static void LogUserNotFoundInStorage(ILogger logger, string userId) =>
        UserNotFoundInStorage(logger, userId, null);

    /// <summary>
    /// Logs a warning that a token was not found in storage for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user whose token was not found.</param>
    public static void LogTokenNotFoundInStorage(ILogger logger, string userId) =>
        TokenNotFoundInStorage(logger, userId, null);

    /// <summary>
    /// Logs a warning about an invalid user ID format.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The invalid user ID.</param>
    public static void LogInvalidUserIdFormat(ILogger logger, string userId) =>
        InvalidUserIdFormat(logger, userId, null);

    /// <summary>
    /// Logs a warning that a null register view model was received.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogNullRegisterViewModel(ILogger logger) =>
        NullRegisterViewModel(logger, null);

    /// <summary>
    /// Logs a warning that user registration failed with specific errors.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="login">The login/username that failed to register.</param>
    /// <param name="errors">The error messages that occurred during registration.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogRegistrationFailedWithErrors(ILogger logger, string login, string errors, Exception? exception = null) =>
        RegistrationFailedWithErrors(logger, login, errors, exception);

    /// <summary>
    /// Logs a warning that a password reset was requested for a non-existent email.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="email">The email address that was not found.</param>
    public static void LogUserNotFoundForPasswordReset(ILogger logger, string email) =>
        UserNotFoundForPasswordReset(logger, email, null);

    /// <summary>
    /// Logs a warning that an invalid password reset token was used.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="email">The email address associated with the invalid token.</param>
    public static void LogInvalidPasswordResetToken(ILogger logger, string email) =>
        InvalidPasswordResetToken(logger, email, null);

    /// <summary>
    /// Logs a warning that password reset failed with specific errors.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="email">The email address for which password reset failed.</param>
    /// <param name="errors">The error messages that occurred during password reset.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogPasswordResetFailedWithErrors(ILogger logger, string email, string errors, Exception? exception = null) =>
        PasswordResetFailedWithErrors(logger, email, errors, exception);

    /// <summary>
    /// Logs a warning that a null forgot password view model was received.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogNullForgotPasswordViewModel(ILogger logger) =>
        NullForgotPasswordViewModel(logger, null);

    /// <summary>
    /// Logs a warning that a null reset password view model was received.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogNullResetPasswordViewModel(ILogger logger) =>
        NullResetPasswordViewModel(logger, null);

    /// <summary>
    /// Logs a warning that password reset link generation failed.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="email">The email address for which link generation failed.</param>
    public static void LogPasswordResetLinkGenerationFailed(ILogger logger, string email) =>
        PasswordResetLinkGenerationFailed(logger, email, null);

    /// <summary>
    /// Logs a warning that an invalid password reset link was accessed.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogInvalidPasswordResetLink(ILogger logger) =>
        InvalidPasswordResetLink(logger, null);

    /// <summary>
    /// Logs a warning that password reset failed because the user was not found.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="email">The email address that was not found.</param>
    public static void LogPasswordResetFailedUserNotFound(ILogger logger, string email) =>
        PasswordResetFailedUserNotFound(logger, email, null);

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
    /// <param name="username">The username attempting to logout.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorDuringLogout(ILogger logger, string username, Exception exception) =>
        UnexpectedErrorDuringLogout(logger, username, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred while storing a token.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The user ID for which token storage failed.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorStoringToken(ILogger logger, string userId, Exception exception) =>
        UnexpectedErrorStoringToken(logger, userId, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred while removing a token.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The user ID for which token removal failed.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorRemovingToken(ILogger logger, string userId, Exception exception) =>
        UnexpectedErrorRemovingToken(logger, userId, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred while retrieving a token.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The user ID for which token retrieval failed.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorRetrievingToken(ILogger logger, string userId, Exception exception) =>
        UnexpectedErrorRetrievingToken(logger, userId, exception);

    /// <summary>
    /// Logs an error that a database error occurred while removing a token.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The user ID for which token removal failed.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogDatabaseErrorRemovingToken(ILogger logger, string userId, Exception exception) =>
        DatabaseErrorRemovingToken(logger, userId, exception);

    /// <summary>
    /// Logs an error that Identity sign-in failed for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username that failed to sign in.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogIdentitySignInFailed(ILogger logger, string username, Exception? exception) =>
        IdentitySignInFailed(logger, username, exception);

    /// <summary>
    /// Logs an error that Identity registration failed for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username that failed to register.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogIdentityRegisterFailed(ILogger logger, string username, Exception exception) =>
        IdentityRegisterFailed(logger, username, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred during registration.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="username">The username attempting to register.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorDuringRegister(ILogger logger, string username, Exception exception) =>
        UnexpectedErrorDuringRegister(logger, username, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred while generating a password reset token.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="email">The email address for which token generation failed.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorGeneratingResetToken(ILogger logger, string email, Exception exception) =>
        UnexpectedErrorGeneratingResetToken(logger, email, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred while sending a password reset email.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="email">The email address to which the reset email failed to send.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorSendingResetEmail(ILogger logger, string email, Exception exception) =>
        UnexpectedErrorSendingResetEmail(logger, email, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred during password reset.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="email">The email address attempting to reset password.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorDuringPasswordReset(ILogger logger, string email, Exception exception) =>
        UnexpectedErrorDuringPasswordReset(logger, email, exception);
}

namespace TodoListApp.WebApp.CustomLogs;

/// <summary>
/// Custom logs for HomeController.
/// </summary>
public static class HomeLog
{
    private static readonly Action<ILogger, Exception?> IndexPageEntered =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1901, nameof(IndexPageEntered)),
            "Index page entered.");

    private static readonly Action<ILogger, Exception?> PrivacyPageEntered =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1902, nameof(PrivacyPageEntered)),
            "Privacy page entered.");

    private static readonly Action<ILogger, Exception?> ErrorPageEntered =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1903, nameof(ErrorPageEntered)),
            "Error page entered.");

    /// <summary>
    /// Logs that the index (home) page has been entered.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogIndexPageEntered(ILogger logger)
    {
        IndexPageEntered(logger, null);
    }

    /// <summary>
    /// Logs that the privacy page has been entered.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogPrivacyPageEntered(ILogger logger)
    {
        PrivacyPageEntered(logger, null);
    }

    /// <summary>
    /// Logs that the error page has been entered.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public static void LogErrorPageEntered(ILogger logger)
    {
        ErrorPageEntered(logger, null);
    }
}

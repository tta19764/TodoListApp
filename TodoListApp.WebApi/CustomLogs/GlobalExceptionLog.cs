namespace TodoListApp.WebApi.CustomLogs;

public static partial class GlobalExceptionLog
{
    /// <summary>
    /// Logs unexpected unhandled exceptions from the global exception handler.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">The caught exception.</param>
    /// <param name="path">The request path where the exception occurred.</param>
    [LoggerMessage(
    EventId = 5000,
    Level = LogLevel.Error,
    Message = "Unhandled exception occurred at path '{Path}'. Exception message: {ExceptionMessage}")]
    public static partial void LogUnhandledException(ILogger logger, string path, string exceptionMessage);
}

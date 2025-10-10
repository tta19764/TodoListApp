using Microsoft.Extensions.Logging;

namespace TodoListApp.Services.WebApi.CustomLogs;

/// <summary>
/// Custom logging class for Tag API service operations.
/// </summary>
public static class TagLog
{
    // Information level logs
    private static readonly Action<ILogger, int, Exception?> TagCreated =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1601, nameof(TagCreated)),
            "Tag {TagId} created successfully");

    private static readonly Action<ILogger, int, Exception?> TagRetrieved =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1602, nameof(TagRetrieved)),
            "Tag {TagId} retrieved successfully");

    private static readonly Action<ILogger, int, Exception?> TagsRetrieved =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1603, nameof(TagsRetrieved)),
            "Retrieved {Count} tags successfully");

    private static readonly Action<ILogger, int, int, Exception?> TagsPageRetrieved =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1604, nameof(TagsPageRetrieved)),
            "Retrieved {Count} tags for page {PageNumber}");

    private static readonly Action<ILogger, int, Exception?> TagUpdated =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1605, nameof(TagUpdated)),
            "Tag {TagId} updated successfully");

    private static readonly Action<ILogger, int, Exception?> TagDeleted =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1606, nameof(TagDeleted)),
            "Tag {TagId} deleted successfully");

    private static readonly Action<ILogger, int, int, Exception?> AvailableTagsRetrieved =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(5007, nameof(AvailableTagsRetrieved)),
            "Retrieved {Count} available tags for task {TaskId}");

    // Warning level logs
    private static readonly Action<ILogger, int, Exception?> TagNotFound =
        LoggerMessage.Define<int>(
            LogLevel.Warning,
            new EventId(2601, nameof(TagNotFound)),
            "Tag {TagId} not found");

    private static readonly Action<ILogger, string, Exception?> NullResponse =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2602, nameof(NullResponse)),
            "Received null response during {Operation}");

    private static readonly Action<ILogger, string, int, int, string, Exception?> OperationFailed =
        LoggerMessage.Define<string, int, int, string>(
            LogLevel.Warning,
            new EventId(2603, nameof(OperationFailed)),
            "Tag {Operation} failed for ID {TagId} with status {StatusCode}: {ErrorContent}");

    // Error level logs
    private static readonly Action<ILogger, Exception?> HttpRequestError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3601, nameof(HttpRequestError)),
            "HTTP request error occurred");

    private static readonly Action<ILogger, Exception?> JsonParsingError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3602, nameof(JsonParsingError)),
            "JSON parsing error occurred");

    private static readonly Action<ILogger, Exception?> UnexpectedError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3603, nameof(UnexpectedError)),
            "Unexpected error occurred");

    // Public methods for Information level logs

    /// <summary>
    /// Logs that a tag has been successfully created.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the created tag.</param>
    public static void LogTagCreated(ILogger logger, int tagId) =>
        TagCreated(logger, tagId, null);

    /// <summary>
    /// Logs that a tag has been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the retrieved tag.</param>
    public static void LogTagRetrieved(ILogger logger, int tagId) =>
        TagRetrieved(logger, tagId, null);

    /// <summary>
    /// Logs that tags have been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tags retrieved.</param>
    public static void LogTagsRetrieved(ILogger logger, int count) =>
        TagsRetrieved(logger, count, null);

    /// <summary>
    /// Logs that a page of tags has been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tags retrieved.</param>
    /// <param name="pageNumber">The page number retrieved.</param>
    public static void LogTagsPageRetrieved(ILogger logger, int count, int pageNumber) =>
        TagsPageRetrieved(logger, count, pageNumber, null);

    /// <summary>
    /// Logs that a tag has been successfully updated.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the updated tag.</param>
    public static void LogTagUpdated(ILogger logger, int tagId) =>
        TagUpdated(logger, tagId, null);

    /// <summary>
    /// Logs that a tag has been successfully deleted.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the deleted tag.</param>
    public static void LogTagDeleted(ILogger logger, int tagId) =>
        TagDeleted(logger, tagId, null);

    /// <summary>
    /// Logs that available tags for a task have been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of available tags retrieved.</param>
    /// <param name="taskId">The ID of the task.</param>
    public static void LogAvailableTagsRetrieved(ILogger logger, int count, int taskId) =>
        AvailableTagsRetrieved(logger, count, taskId, null);

    // Public methods for Warning level logs

    /// <summary>
    /// Logs a warning that a tag was not found.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag that was not found.</param>
    public static void LogTagNotFound(ILogger logger, int tagId) =>
        TagNotFound(logger, tagId, null);

    /// <summary>
    /// Logs a warning that a null response was received during an operation.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="operation">The name of the operation that returned null.</param>
    public static void LogNullResponse(ILogger logger, string operation) =>
        NullResponse(logger, operation, null);

    /// <summary>
    /// Logs a warning that a tag operation failed.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="operation">The name of the operation that failed.</param>
    /// <param name="tagId">The ID of the tag involved.</param>
    /// <param name="statusCode">The HTTP status code returned.</param>
    /// <param name="errorContent">The error content returned.</param>
    public static void LogOperationFailed(ILogger logger, string operation, int tagId, int statusCode, string errorContent) =>
        OperationFailed(logger, operation, tagId, statusCode, errorContent, null);

    // Public methods for Error level logs

    /// <summary>
    /// Logs an error that an HTTP request error occurred.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogHttpRequestError(ILogger logger, Exception exception) =>
        HttpRequestError(logger, exception);

    /// <summary>
    /// Logs an error that a JSON parsing error occurred.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogJsonParsingError(ILogger logger, Exception exception) =>
        JsonParsingError(logger, exception);

    /// <summary>
    /// Logs an error that an unexpected error occurred.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedError(ILogger logger, Exception exception) =>
        UnexpectedError(logger, exception);
}

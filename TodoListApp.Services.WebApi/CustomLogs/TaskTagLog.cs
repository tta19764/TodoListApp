using Microsoft.Extensions.Logging;

namespace TodoListApp.Services.WebApi.CustomLogs;

/// <summary>
/// Custom logging class for TaskTag API service operations.
/// </summary>
public static class TaskTagLog
{
    // Information level logs
    private static readonly Action<ILogger, int, int, Exception?> TagAddedToTask =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1701, nameof(TagAddedToTask)),
            "Tag {TagId} added to task {TaskId} successfully");

    private static readonly Action<ILogger, int, int, Exception?> TagRemovedFromTask =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1702, nameof(TagRemovedFromTask)),
            "Tag {TagId} removed from task {TaskId} successfully");

    private static readonly Action<ILogger, int, int, Exception?> UserTaskTagsRetrieved =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1703, nameof(UserTaskTagsRetrieved)),
            "Retrieved {Count} tags for user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TaggedTasksRetrieved =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1704, nameof(TaggedTasksRetrieved)),
            "Retrieved {Count} tasks for tag {TagId} and user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TaggedTasksPageRetrieved =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1705, nameof(TaggedTasksPageRetrieved)),
            "Retrieved {Count} tasks for tag {TagId} on page {PageNumber}");

    // Warning level logs
    private static readonly Action<ILogger, string, Exception?> NullResponse =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2701, nameof(NullResponse)),
            "Received null response during {Operation}");

    private static readonly Action<ILogger, string, int, int, string, Exception?> OperationFailed =
        LoggerMessage.Define<string, int, int, string>(
            LogLevel.Warning,
            new EventId(2702, nameof(OperationFailed)),
            "TaskTag {Operation} failed for ID {Id} with status {StatusCode}: {ErrorContent}");

    // Error level logs
    private static readonly Action<ILogger, Exception?> HttpRequestError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3701, nameof(HttpRequestError)),
            "HTTP request error occurred");

    private static readonly Action<ILogger, Exception?> JsonParsingError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3702, nameof(JsonParsingError)),
            "JSON parsing error occurred");

    private static readonly Action<ILogger, Exception?> UnexpectedError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3703, nameof(UnexpectedError)),
            "Unexpected error occurred");

    // Public methods for Information level logs

    /// <summary>
    /// Logs that a tag has been successfully added to a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag that was added.</param>
    /// <param name="taskId">The ID of the task to which the tag was added.</param>
    public static void LogTagAddedToTask(ILogger logger, int tagId, int taskId) =>
        TagAddedToTask(logger, tagId, taskId, null);

    /// <summary>
    /// Logs that a tag has been successfully removed from a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag that was removed.</param>
    /// <param name="taskId">The ID of the task from which the tag was removed.</param>
    public static void LogTagRemovedFromTask(ILogger logger, int tagId, int taskId) =>
        TagRemovedFromTask(logger, tagId, taskId, null);

    /// <summary>
    /// Logs that tags for a user's tasks have been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tags retrieved.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogUserTaskTagsRetrieved(ILogger logger, int count, int userId) =>
        UserTaskTagsRetrieved(logger, count, userId, null);

    /// <summary>
    /// Logs that tasks with a specific tag have been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks retrieved.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogTaggedTasksRetrieved(ILogger logger, int count, int tagId, int userId) =>
        TaggedTasksRetrieved(logger, count, tagId, userId, null);

    /// <summary>
    /// Logs that a page of tasks with a specific tag has been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks retrieved.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <param name="pageNumber">The page number retrieved.</param>
    public static void LogTaggedTasksPageRetrieved(ILogger logger, int count, int tagId, int pageNumber) =>
        TaggedTasksPageRetrieved(logger, count, tagId, pageNumber, null);

    // Public methods for Warning level logs

    /// <summary>
    /// Logs a warning that a null response was received during an operation.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="operation">The name of the operation that returned null.</param>
    public static void LogNullResponse(ILogger logger, string operation) =>
        NullResponse(logger, operation, null);

    /// <summary>
    /// Logs a warning that a task-tag operation failed.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="operation">The name of the operation that failed.</param>
    /// <param name="id">The ID involved in the operation.</param>
    /// <param name="statusCode">The HTTP status code returned.</param>
    /// <param name="errorContent">The error content returned.</param>
    public static void LogOperationFailed(ILogger logger, string operation, int id, int statusCode, string errorContent) =>
        OperationFailed(logger, operation, id, statusCode, errorContent, null);

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

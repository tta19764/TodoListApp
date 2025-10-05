using Microsoft.Extensions.Logging;

namespace TodoListApp.Services.WebApi.CustomLogs;
public static class TodoTaskLog
{
    // Information
    private static readonly Action<ILogger, int, Exception?> TaskCreated =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1301, nameof(TaskCreated)),
            "Todo task created successfully: {TaskId}");

    private static readonly Action<ILogger, int, Exception?> TaskDeleted =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1302, nameof(TaskDeleted)),
            "Todo task deleted successfully: {TaskId}");

    private static readonly Action<ILogger, int, int, Exception?> TaskRetrieved =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1303, nameof(TaskRetrieved)),
            "Retrieved todo task {TaskId} for user {UserId}");

    private static readonly Action<ILogger, int, Exception?> TaskUpdated =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1304, nameof(TaskUpdated)),
            "Todo task {TaskId} updated successfully");

    private static readonly Action<ILogger, int, int, int, Exception?> TodoTasksRetrievedByListId =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1305, nameof(TodoTasksRetrievedByListId)),
            "Retrieved {Count} todo tasks for list {ListId} for user {UserId}");

    private static readonly Action<ILogger, int, int, int, int, Exception?> TodoTasksPageRetrievedByListId =
        LoggerMessage.Define<int, int, int, int>(
            LogLevel.Information,
            new EventId(1306, nameof(TodoTasksPageRetrievedByListId)),
            "Retrieved {Count} todo tasks (page {PageNumber}) for list {ListId} for user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> TodoTasksRetrievedByOwner =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1307, nameof(TodoTasksRetrievedByOwner)),
            "Retrieved todo tasks {Count} for owner {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TodoTasksPageRetrievedByOwner =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1308, nameof(TodoTasksPageRetrievedByOwner)),
            "Retrieved todo tasks {Count} (page {PageNumber}) for owner {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> TodoTasksRetrievedByUser =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1309, nameof(TodoTasksRetrievedByUser)),
            "Retrieved todo tasks {Count} for user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TodoTasksPageRetrievedByUser =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1310, nameof(TodoTasksPageRetrievedByUser)),
            "Retrieved todo tasks {Count} (page {PageNumber}) for user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> TodoTasksRetrievedBySearch =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1311, nameof(TodoTasksRetrievedBySearch)),
            "Retrieved search todo tasks {Count} for user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TodoTasksPageRetrievedBySearch =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1313, nameof(TodoTasksPageRetrievedBySearch)),
            "Retrieved search todo tasks {Count} (page {PageNumber}) for user {UserId}");

    // Warnings
    private static readonly Action<ILogger, string, Exception?> NullResponse =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2301, nameof(NullResponse)),
            "Todo task {Operation} returned null response");

    private static readonly Action<ILogger, int, Exception?> TaskNotFound =
        LoggerMessage.Define<int>(
            LogLevel.Warning,
            new EventId(2302, nameof(TaskNotFound)),
            "Todo task {TaskId} was not found");

    private static readonly Action<ILogger, string, int, int, string, Exception?> TaskFailed =
        LoggerMessage.Define<string, int, int, string>(
            LogLevel.Warning,
            new EventId(2303, nameof(TaskFailed)),
            "Failed to {Operation} task {TaskId}. Status: {StatusCode}, Error: {Error}");

    // Errors
    private static readonly Action<ILogger, Exception?> HttpRequestError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3301, nameof(HttpRequestError)),
            "HTTP error occurred while performing todo task operation");

    private static readonly Action<ILogger, Exception?> JsonParsingError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3302, nameof(JsonParsingError)),
            "JSON parsing error while processing todo task response");

    private static readonly Action<ILogger, Exception?> UnexpectedError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3303, nameof(UnexpectedError)),
            "Unexpected error occurred during todo task operation");

    // Public wrappers
    public static void LogTaskCreated(ILogger logger, int taskId) => TaskCreated(logger, taskId, null);

    public static void LogTaskDeleted(ILogger logger, int taskId) => TaskDeleted(logger, taskId, null);

    public static void LogTaskRetrieved(ILogger logger, int taskId, int userId) => TaskRetrieved(logger, taskId, userId, null);

    public static void LogTaskUpdated(ILogger logger, int taskId) => TaskUpdated(logger, taskId, null);

    public static void LogTodoTasksRetrievedByListId(ILogger logger, int count, int listId, int userId) =>
        TodoTasksRetrievedByListId(logger, count, listId, userId, null);

    public static void LogTodoTasksPageRetrievedByListId(ILogger logger, int count, int pageNumber, int listId, int userId) =>
        TodoTasksPageRetrievedByListId(logger, count, pageNumber, listId, userId, null);

    public static void LogTodoTasksRetrievedByOwner(ILogger logger, int count, int userId) =>
        TodoTasksRetrievedByOwner(logger, count, userId, null);

    public static void LogTodoTasksPageRetrievedByOwner(ILogger logger, int count, int pageNumber, int userId) =>
        TodoTasksPageRetrievedByOwner(logger, count, pageNumber, userId, null);

    public static void LogTodoTasksRetrievedByUser(ILogger logger, int count, int userId) =>
        TodoTasksRetrievedByUser(logger, count, userId, null);

    public static void LogTodoTasksPageRetrievedByUser(ILogger logger, int count, int pageNumber, int userId) =>
        TodoTasksPageRetrievedByUser(logger, count, pageNumber, userId, null);

    public static void LogTodoTasksRetrievedBySearch(ILogger logger, int count, int userId) =>
        TodoTasksRetrievedBySearch(logger, count, userId, null);

    public static void LogTodoTasksPageRetrievedBySearch(ILogger logger, int count, int pageNumber, int userId) =>
        TodoTasksPageRetrievedBySearch(logger, count, pageNumber, userId, null);

    public static void LogNullResponse(ILogger logger, string operation) => NullResponse(logger, operation, null);

    public static void LogTaskNotFound(ILogger logger, int taskId) => TaskNotFound(logger, taskId, null);

    public static void LogTaskFailed(ILogger logger, string operation, int taskId, int statusCode, string error) =>
        TaskFailed(logger, operation, taskId, statusCode, error, null);

    public static void LogHttpRequestError(ILogger logger, Exception ex) => HttpRequestError(logger, ex);

    public static void LogJsonParsingError(ILogger logger, Exception ex) => JsonParsingError(logger, ex);

    public static void LogUnexpectedError(ILogger logger, Exception ex) => UnexpectedError(logger, ex);
}

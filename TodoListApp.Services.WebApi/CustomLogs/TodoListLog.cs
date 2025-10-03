using Microsoft.Extensions.Logging;

namespace TodoListApp.Services.WebApi.CustomLogs;
public static class TodoListLog
{
    // Information level logs
    private static readonly Action<ILogger, int, Exception?> TodoListCreated =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1201, nameof(TodoListCreated)),
            "Todo list created successfully: {ListId}");

    private static readonly Action<ILogger, int, Exception?> TodoListDeleted =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1202, nameof(TodoListDeleted)),
            "Todo list deleted successfully: {ListId}");

    private static readonly Action<ILogger, int, int, Exception?> TodoListsRetrieved =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1203, nameof(TodoListsRetrieved)),
            "Retrieved {Count} todo lists for user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TodoListsPageRetrieved =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1204, nameof(TodoListsPageRetrieved)),
            "Retrieved {Count} todo lists (page {PageNumber}) for user {UserId}");

    private static readonly Action<ILogger, int, Exception?> TodoListUpdated =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1205, nameof(TodoListUpdated)),
            "Todo list {ListId} updated successfully");

    private static readonly Action<ILogger, int, int, Exception?> TodoListRetrievedById =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1206, nameof(TodoListRetrievedById)),
            "Retrieved todo list {ListId} for user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> TodoListsRetrievedByAuthor =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1207, nameof(TodoListsRetrievedByAuthor)),
            "Retrieved todo lists {Count} for user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TodoListsPageRetrievedByAuthor =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1208, nameof(TodoListsPageRetrievedByAuthor)),
            "Retrieved todo lists {Count} (page {PageNumber}) for user {UserId}");

    // Warning level logs
    private static readonly Action<ILogger, string, Exception?> NullResponse =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2201, nameof(NullResponse)),
            "Todo list {Operation} returned null response");

    private static readonly Action<ILogger, int, Exception?> TodoListNotFound =
        LoggerMessage.Define<int>(
            LogLevel.Warning,
            new EventId(2202, nameof(TodoListNotFound)),
            "Todo list {ListId} was not found");

    private static readonly Action<ILogger, string, int, int, string, Exception?> TodoListFailed =
        LoggerMessage.Define<string, int, int, string>(
            LogLevel.Warning,
            new EventId(2203, nameof(TodoListFailed)),
            "Failed to {Operation} on {ListId}. Status: {StatusCode}, Error: {Error}");

    // Error level logs
    private static readonly Action<ILogger, Exception?> HttpRequestError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3201, nameof(HttpRequestError)),
            "HTTP error occurred while performing todo list operation");

    private static readonly Action<ILogger, Exception?> JsonParsingError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3202, nameof(JsonParsingError)),
            "JSON parsing error while processing todo list response");

    private static readonly Action<ILogger, Exception?> UnexpectedError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3203, nameof(UnexpectedError)),
            "Unexpected error occurred during todo list operation");

    // Public wrappers

    // Information
    public static void LogTodoListCreated(ILogger logger, int listId) =>
        TodoListCreated(logger, listId, null);

    public static void LogTodoListDeleted(ILogger logger, int listId) =>
        TodoListDeleted(logger, listId, null);

    public static void LogTodoListsRetrieved(ILogger logger, int count, int userId) =>
        TodoListsRetrieved(logger, count, userId, null);

    public static void LogTodoListsPageRetrieved(ILogger logger, int count, int pageNumber, int userId) =>
        TodoListsPageRetrieved(logger, count, pageNumber, userId, null);

    public static void LogTodoListUpdated(ILogger logger, int listId) =>
        TodoListUpdated(logger, listId, null);

    public static void LogTodoListRetrievedById(ILogger logger, int listId, int userId) =>
        TodoListRetrievedById(logger, listId, userId, null);

    public static void LogTodoListsRetrievedByAuthor(ILogger logger, int count, int userId) =>
        TodoListsRetrievedByAuthor(logger, count, userId, null);

    public static void LogTodoListsPageRetrievedByAuthor(ILogger logger, int count, int pageNumber, int userId) =>
        TodoListsPageRetrievedByAuthor(logger, count, pageNumber, userId, null);

    // Warning
    public static void LogNullResponse(ILogger logger, string operation) =>
        NullResponse(logger, operation, null);

    public static void LogTodoListNotFound(ILogger logger, int listId) =>
        TodoListNotFound(logger, listId, null);

    public static void LogTodoListFailed(ILogger logger, string operation, int listId, int statusCode, string error) =>
        TodoListFailed(logger, operation, listId, statusCode, error, null);

    // Error
    public static void LogHttpRequestError(ILogger logger, Exception ex) =>
        HttpRequestError(logger, ex);

    public static void LogJsonParsingError(ILogger logger, Exception ex) =>
        JsonParsingError(logger, ex);

    public static void LogUnexpectedError(ILogger logger, Exception ex) =>
        UnexpectedError(logger, ex);
}

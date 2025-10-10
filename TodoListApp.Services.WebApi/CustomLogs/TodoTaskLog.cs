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

    private static readonly Action<ILogger, int, int, Exception?> CommentCreated =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1314, nameof(CommentCreated)),
            "Comment {CommentId} created successfully for task {TaskId}");

    private static readonly Action<ILogger, int, Exception?> CommentUpdated =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1315, nameof(CommentUpdated)),
            "Comment {CommentId} updated successfully");

    private static readonly Action<ILogger, int, Exception?> CommentDeleted =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1316, nameof(CommentDeleted)),
            "Comment {CommentId} deleted successfully");

    private static readonly Action<ILogger, int, int, int, Exception?> CommentsRetrievedForTask =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1317, nameof(CommentsRetrievedForTask)),
            "Retrieved {Count} comments for task {TaskId} for user {UserId}");

    private static readonly Action<ILogger, int, int, int, int, Exception?> CommentsPageRetrievedForTask =
        LoggerMessage.Define<int, int, int, int>(
            LogLevel.Information,
            new EventId(1318, nameof(CommentsPageRetrievedForTask)),
            "Retrieved {Count} comments (page {PageNumber}) for task {TaskId} for user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> CommentCountRetrieved =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1319, nameof(CommentCountRetrieved)),
            "Retrieved comment count {Count} for task {TaskId}");

    private static readonly Action<ILogger, int, int, Exception?> CommentRetrieved =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1319, nameof(CommentRetrieved)),
            "Retrieved comment {CommentId} for task {TaskId}");

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

    private static readonly Action<ILogger, int, Exception?> CommentNotFound =
        LoggerMessage.Define<int>(
            LogLevel.Warning,
            new EventId(2304, nameof(CommentNotFound)),
            "Comment {CommentId} was not found");

    private static readonly Action<ILogger, string, int, int, string, Exception?> CommentOperationFailed =
        LoggerMessage.Define<string, int, int, string>(
            LogLevel.Warning,
            new EventId(2305, nameof(CommentOperationFailed)),
            "Failed to {Operation} comment {CommentId}. Status: {StatusCode}, Error: {Error}");

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

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorCreatingComment =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3304, nameof(UnexpectedErrorCreatingComment)),
            "Unexpected error occurred while creating comment for task {TaskId}");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorUpdatingComment =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3305, nameof(UnexpectedErrorUpdatingComment)),
            "Unexpected error occurred while updating comment {CommentId}");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorDeletingComment =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3306, nameof(UnexpectedErrorDeletingComment)),
            "Unexpected error occurred while deleting comment {CommentId}");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorRetrievingComments =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3307, nameof(UnexpectedErrorRetrievingComments)),
            "Unexpected error occurred while retrieving comments for task {TaskId}");

    // Public wrappers

    /// <summary>
    /// Logs that a to-do task has been successfully created.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the created task.</param>
    public static void LogTaskCreated(ILogger logger, int taskId) => TaskCreated(logger, taskId, null);

    /// <summary>
    /// Logs that a to-do task has been successfully deleted.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the deleted task.</param>
    public static void LogTaskDeleted(ILogger logger, int taskId) => TaskDeleted(logger, taskId, null);

    /// <summary>
    /// Logs that a to-do task has been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the retrieved task.</param>
    /// <param name="userId">The ID of the user who retrieved the task.</param>
    public static void LogTaskRetrieved(ILogger logger, int taskId, int userId) => TaskRetrieved(logger, taskId, userId, null);

    /// <summary>
    /// Logs that a to-do task has been successfully updated.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the updated task.</param>
    public static void LogTaskUpdated(ILogger logger, int taskId) => TaskUpdated(logger, taskId, null);

    /// <summary>
    /// Logs that to-do tasks for a specific list have been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks retrieved.</param>
    /// <param name="listId">The ID of the list.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogTodoTasksRetrievedByListId(ILogger logger, int count, int listId, int userId) =>
        TodoTasksRetrievedByListId(logger, count, listId, userId, null);

    /// <summary>
    /// Logs that a page of to-do tasks for a specific list has been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks retrieved.</param>
    /// <param name="pageNumber">The page number retrieved.</param>
    /// <param name="listId">The ID of the list.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogTodoTasksPageRetrievedByListId(ILogger logger, int count, int pageNumber, int listId, int userId) =>
        TodoTasksPageRetrievedByListId(logger, count, pageNumber, listId, userId, null);

    /// <summary>
    /// Logs that to-do tasks owned by a user have been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks retrieved.</param>
    /// <param name="userId">The ID of the owner user.</param>
    public static void LogTodoTasksRetrievedByOwner(ILogger logger, int count, int userId) =>
        TodoTasksRetrievedByOwner(logger, count, userId, null);

    /// <summary>
    /// Logs that a page of to-do tasks owned by a user has been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks retrieved.</param>
    /// <param name="pageNumber">The page number retrieved.</param>
    /// <param name="userId">The ID of the owner user.</param>
    public static void LogTodoTasksPageRetrievedByOwner(ILogger logger, int count, int pageNumber, int userId) =>
        TodoTasksPageRetrievedByOwner(logger, count, pageNumber, userId, null);

    /// <summary>
    /// Logs that to-do tasks for a user have been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks retrieved.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogTodoTasksRetrievedByUser(ILogger logger, int count, int userId) =>
        TodoTasksRetrievedByUser(logger, count, userId, null);

    /// <summary>
    /// Logs that a page of to-do tasks for a user has been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks retrieved.</param>
    /// <param name="pageNumber">The page number retrieved.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogTodoTasksPageRetrievedByUser(ILogger logger, int count, int pageNumber, int userId) =>
        TodoTasksPageRetrievedByUser(logger, count, pageNumber, userId, null);

    /// <summary>
    /// Logs that to-do tasks matching a search query have been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks retrieved.</param>
    /// <param name="userId">The ID of the user who performed the search.</param>
    public static void LogTodoTasksRetrievedBySearch(ILogger logger, int count, int userId) =>
        TodoTasksRetrievedBySearch(logger, count, userId, null);

    /// <summary>
    /// Logs that a page of to-do tasks matching a search query has been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks retrieved.</param>
    /// <param name="pageNumber">The page number retrieved.</param>
    /// <param name="userId">The ID of the user who performed the search.</param>
    public static void LogTodoTasksPageRetrievedBySearch(ILogger logger, int count, int pageNumber, int userId) =>
        TodoTasksPageRetrievedBySearch(logger, count, pageNumber, userId, null);

    /// <summary>
    /// Logs that a comment has been successfully created for a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the created comment.</param>
    /// <param name="taskId">The ID of the task.</param>
    public static void LogCommentCreated(ILogger logger, int commentId, int taskId) => CommentCreated(logger, commentId, taskId, null);

    /// <summary>
    /// Logs that a comment has been successfully updated.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the updated comment.</param>
    public static void LogCommentUpdated(ILogger logger, int commentId) => CommentUpdated(logger, commentId, null);

    /// <summary>
    /// Logs that a comment has been successfully deleted.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the deleted comment.</param>
    public static void LogCommentDeleted(ILogger logger, int commentId) => CommentDeleted(logger, commentId, null);

    /// <summary>
    /// Logs that comments for a task have been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of comments retrieved.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogCommentsRetrievedForTask(ILogger logger, int count, int taskId, int userId) =>
        CommentsRetrievedForTask(logger, count, taskId, userId, null);

    /// <summary>
    /// Logs that a page of comments for a task has been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of comments retrieved.</param>
    /// <param name="pageNumber">The page number retrieved.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogCommentsPageRetrievedForTask(ILogger logger, int count, int pageNumber, int taskId, int userId) =>
        CommentsPageRetrievedForTask(logger, count, pageNumber, taskId, userId, null);

    /// <summary>
    /// Logs that the comment count for a task has been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of comments.</param>
    /// <param name="taskId">The ID of the task.</param>
    public static void LogCommentCountRetrieved(ILogger logger, int count, int taskId) => CommentCountRetrieved(logger, count, taskId, null);

    /// <summary>
    /// Logs that a comment has been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the retrieved comment.</param>
    /// <param name="taskId">The ID of the task.</param>
    public static void LogCommentRetrieved(ILogger logger, int commentId, int taskId) => CommentRetrieved(logger, commentId, taskId, null);

    /// <summary>
    /// Logs that a null response was returned from a to-do task operation.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="operation">The operation name that returned null.</param>
    public static void LogNullResponse(ILogger logger, string operation) => NullResponse(logger, operation, null);

    /// <summary>
    /// Logs that a specific to-do task was not found.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task that was not found.</param>
    public static void LogTaskNotFound(ILogger logger, int taskId) => TaskNotFound(logger, taskId, null);

    /// <summary>
    /// Logs that a to-do task operation failed, including status code and error message.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="operation">The operation that failed (e.g., "update", "delete").</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="statusCode">The HTTP or internal status code returned.</param>
    /// <param name="error">The detailed error message.</param>
    public static void LogTaskFailed(ILogger logger, string operation, int taskId, int statusCode, string error) =>
        TaskFailed(logger, operation, taskId, statusCode, error, null);

    /// <summary>
    /// Logs that a specific comment was not found.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the comment that was not found.</param>
    public static void LogCommentNotFound(ILogger logger, int commentId) =>
        CommentNotFound(logger, commentId, null);

    /// <summary>
    /// Logs that an operation on a comment failed, including status code and error message.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="operation">The operation that failed (e.g., "create", "update", "delete").</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <param name="statusCode">The HTTP or internal status code returned.</param>
    /// <param name="error">The detailed error message.</param>
    public static void LogCommentOperationFailed(ILogger logger, string operation, int commentId, int statusCode, string error) =>
        CommentOperationFailed(logger, operation, commentId, statusCode, error, null);

    /// <summary>
    /// Logs that an HTTP request error occurred during a to-do task operation.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogHttpRequestError(ILogger logger, Exception ex) => HttpRequestError(logger, ex);

    /// <summary>
    /// Logs that a JSON parsing error occurred while processing a to-do task response.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="ex">The exception that occurred during JSON parsing.</param>
    public static void LogJsonParsingError(ILogger logger, Exception ex) => JsonParsingError(logger, ex);

    /// <summary>
    /// Logs that an unexpected error occurred during a to-do task operation.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogUnexpectedError(ILogger logger, Exception ex) => UnexpectedError(logger, ex);

    /// <summary>
    /// Logs that an unexpected error occurred while creating a comment for a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task associated with the comment.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogUnexpectedErrorCreatingComment(ILogger logger, int taskId, Exception ex) =>
        UnexpectedErrorCreatingComment(logger, taskId, ex);

    /// <summary>
    /// Logs that an unexpected error occurred while updating a comment.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the comment being updated.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogUnexpectedErrorUpdatingComment(ILogger logger, int commentId, Exception ex) =>
        UnexpectedErrorUpdatingComment(logger, commentId, ex);

    /// <summary>
    /// Logs that an unexpected error occurred while deleting a comment.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the comment being deleted.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogUnexpectedErrorDeletingComment(ILogger logger, int commentId, Exception ex) =>
        UnexpectedErrorDeletingComment(logger, commentId, ex);

    /// <summary>
    /// Logs that an unexpected error occurred while retrieving comments for a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task whose comments were being retrieved.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogUnexpectedErrorRetrievingComments(ILogger logger, int taskId, Exception ex) =>
        UnexpectedErrorRetrievingComments(logger, taskId, ex);
}

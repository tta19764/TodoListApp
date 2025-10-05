namespace TodoListApp.WebApi.CustomLogs;

/// <summary>
/// Custom logging class for TodoTasks related operations.
/// </summary>
internal static class TodoTasksLog
{
    // Warning level logs
    private static readonly Action<ILogger, int, int?, string, Exception?> TaskNotFoundForUser =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Warning,
            new EventId(1301, nameof(TaskNotFoundForUser)),
            "Task with ID {TaskId} not found for user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> ListNotFoundForUser =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Warning,
            new EventId(1302, nameof(ListNotFoundForUser)),
            "List with ID {ListId} not found for user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> UnauthorizedTaskAccess =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Warning,
            new EventId(1303, nameof(UnauthorizedTaskAccess)),
            "Unauthorized access attempt for task {TaskId} by user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> UnauthorizedListAccess =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Warning,
            new EventId(1304, nameof(UnauthorizedListAccess)),
            "Unauthorized access attempt for list {ListId} by user {UserId}: {Message}");

    private static readonly Action<ILogger, int?, string, Exception?> UnauthorizedTasksAccess =
        LoggerMessage.Define<int?, string>(
            LogLevel.Warning,
            new EventId(1305, nameof(UnauthorizedTasksAccess)),
            "Unauthorized access attempt by user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> UnauthorizedTaskDeletion =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Warning,
            new EventId(1306, nameof(UnauthorizedTaskDeletion)),
            "Unauthorized deletion attempt for task {TaskId} by user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> UnauthorizedTaskUpdate =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Warning,
            new EventId(1307, nameof(UnauthorizedTaskUpdate)),
            "Unauthorized update attempt for task {TaskId} by user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> UnauthorizedTaskStatusUpdate =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Warning,
            new EventId(1308, nameof(UnauthorizedTaskStatusUpdate)),
            "Unauthorized status update attempt for task {TaskId} by user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int, Exception?> InvalidPaginationParameters =
        LoggerMessage.Define<int, int>(
            LogLevel.Warning,
            new EventId(1309, nameof(InvalidPaginationParameters)),
            "Invalid pagination parameters: pageNumber={PageNumber}, rowCount={RowCount}");

    private static readonly Action<ILogger, string, Exception?> InvalidTaskDataProvided =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1310, nameof(InvalidTaskDataProvided)),
            "Invalid task data provided: {Message}");

    private static readonly Action<ILogger, string, Exception?> InvalidStatusDataProvided =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1311, nameof(InvalidStatusDataProvided)),
            "Invalid status data provided: {Message}");

    private static readonly Action<ILogger, string, Exception?> ReferencedEntityNotFound =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1312, nameof(ReferencedEntityNotFound)),
            "Referenced entity not found during task creation: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> TagNotFoundForUser =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Warning,
            new EventId(1313, nameof(TagNotFoundForUser)),
            "Tag with ID {TagId} not found for user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int, int?, string, Exception?> UnauthorizedTagOperation =
        LoggerMessage.Define<int, int, int?, string>(
            LogLevel.Warning,
            new EventId(1314, nameof(UnauthorizedTagOperation)),
            "Unauthorized tag operation for task {TaskId}, tag {TagId} by user {UserId}: {Message}");

    private static readonly Action<ILogger, string, Exception?> InvalidTagDataProvided =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1315, nameof(InvalidTagDataProvided)),
            "Invalid tag data provided: {Message}");

    // Error level logs
    private static readonly Action<ILogger, int, int?, string, Exception?> UnableToDeleteTask =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Error,
            new EventId(2301, nameof(UnableToDeleteTask)),
            "Unable to delete task {TaskId} for user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> UnableToUpdateTask =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Error,
            new EventId(2302, nameof(UnableToUpdateTask)),
            "Unable to update task {TaskId} for user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> UnableToUpdateTaskStatus =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Error,
            new EventId(2303, nameof(UnableToUpdateTaskStatus)),
            "Unable to update task status {TaskId} for user {UserId}: {Message}");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorRetrievingTask =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(2304, nameof(UnexpectedErrorRetrievingTask)),
            "Unexpected error occurred while retrieving task {TaskId}");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorRetrievingListTasks =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(2305, nameof(UnexpectedErrorRetrievingListTasks)),
            "Unexpected error occurred while retrieving tasks for list {ListId}");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorRetrievingPaginatedListTasks =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(2306, nameof(UnexpectedErrorRetrievingPaginatedListTasks)),
            "Unexpected error occurred while retrieving paginated tasks for list {ListId}");

    private static readonly Action<ILogger, int?, Exception?> UnexpectedErrorRetrievingUserTasks =
        LoggerMessage.Define<int?>(
            LogLevel.Error,
            new EventId(2307, nameof(UnexpectedErrorRetrievingUserTasks)),
            "Unexpected error occurred while retrieving tasks for user {UserId}");

    private static readonly Action<ILogger, int?, Exception?> UnexpectedErrorRetrievingPaginatedUserTasks =
        LoggerMessage.Define<int?>(
            LogLevel.Error,
            new EventId(2308, nameof(UnexpectedErrorRetrievingPaginatedUserTasks)),
            "Unexpected error occurred while retrieving paginated tasks for user {UserId}");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorDeletingTask =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(2309, nameof(UnexpectedErrorDeletingTask)),
            "Unexpected error occurred while deleting task {TaskId}");

    private static readonly Action<ILogger, Exception?> UnexpectedErrorCreatingTask =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(2310, nameof(UnexpectedErrorCreatingTask)),
            "Unexpected error occurred while creating task");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorUpdatingTask =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(2311, nameof(UnexpectedErrorUpdatingTask)),
            "Unexpected error occurred while updating task {TaskId}");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorUpdatingTaskStatus =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(2312, nameof(UnexpectedErrorUpdatingTaskStatus)),
            "Unexpected error occurred while updating task status {TaskId}");

    private static readonly Action<ILogger, int?, Exception?> UnexpectedErrorRetrievingTags =
        LoggerMessage.Define<int?>(
            LogLevel.Error,
            new EventId(2313, nameof(UnexpectedErrorRetrievingTags)),
            "Unexpected error occurred while retrieving tags for user {UserId}");

    private static readonly Action<ILogger, int, int?, Exception?> UnexpectedErrorRetrievingTaggedTasks =
        LoggerMessage.Define<int, int?>(
            LogLevel.Error,
            new EventId(2314, nameof(UnexpectedErrorRetrievingTaggedTasks)),
            "Unexpected error occurred while retrieving tasks for tag {TagId} and user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> UnexpectedErrorAddingTag =
        LoggerMessage.Define<int, int>(
            LogLevel.Error,
            new EventId(2315, nameof(UnexpectedErrorAddingTag)),
            "Unexpected error occurred while adding tag {TagId} to task {TaskId}");

    private static readonly Action<ILogger, int, int, Exception?> UnexpectedErrorRemovingTag =
        LoggerMessage.Define<int, int>(
            LogLevel.Error,
            new EventId(2316, nameof(UnexpectedErrorRemovingTag)),
            "Unexpected error occurred while removing tag {TagId} from task {TaskId}");

    // Information level logs
    private static readonly Action<ILogger, int, int?, Exception?> TaskDeletedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(3301, nameof(TaskDeletedSuccessfully)),
            "Task {TaskId} deleted successfully by user {UserId}");

    private static readonly Action<ILogger, int, int?, Exception?> TaskCreatedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(3302, nameof(TaskCreatedSuccessfully)),
            "Task {TaskId} created successfully by user {UserId}");

    private static readonly Action<ILogger, int, int?, Exception?> TaskUpdatedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(3303, nameof(TaskUpdatedSuccessfully)),
            "Task {TaskId} updated successfully by user {UserId}");

    private static readonly Action<ILogger, int, int?, Exception?> TaskStatusUpdatedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(3304, nameof(TaskStatusUpdatedSuccessfully)),
            "Task {TaskId} status updated successfully by user {UserId}");

    private static readonly Action<ILogger, int, int?, Exception?> TagsRetrievedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(3305, nameof(TagsRetrievedSuccessfully)),
            "Retrieved {Count} tags for user {UserId}");

    private static readonly Action<ILogger, int, int, int?, Exception?> TaggedTasksRetrievedSuccessfully =
        LoggerMessage.Define<int, int, int?>(
            LogLevel.Information,
            new EventId(3306, nameof(TaggedTasksRetrievedSuccessfully)),
            "Retrieved {Count} tasks for tag {TagId} and user {UserId}");

    private static readonly Action<ILogger, int, int, int?, Exception?> TagAddedToTask =
        LoggerMessage.Define<int, int, int?>(
            LogLevel.Information,
            new EventId(3307, nameof(TagAddedToTask)),
            "Tag {TagId} added to task {TaskId} by user {UserId}");

    private static readonly Action<ILogger, int, int, int?, Exception?> TagRemovedFromTask =
        LoggerMessage.Define<int, int, int?>(
            LogLevel.Information,
            new EventId(3308, nameof(TagRemovedFromTask)),
            "Tag {TagId} removed from task {TaskId} by user {UserId}");

    // Public methods for Warning level logs
    public static void LogTaskNotFoundForUser(ILogger logger, int taskId, int? userId, string message, Exception? exception = null) =>
       TaskNotFoundForUser(logger, taskId, userId, message, exception);

    public static void LogListNotFoundForUser(ILogger logger, int listId, int? userId, string message, Exception? exception = null) =>
        ListNotFoundForUser(logger, listId, userId, message, exception);

    public static void LogUnauthorizedTaskAccess(ILogger logger, int taskId, int? userId, string message, Exception? exception = null) =>
        UnauthorizedTaskAccess(logger, taskId, userId, message, exception);

    public static void LogUnauthorizedListAccess(ILogger logger, int listId, int? userId, string message, Exception? exception = null) =>
        UnauthorizedListAccess(logger, listId, userId, message, exception);

    public static void LogUnauthorizedTasksAccess(ILogger logger, int? userId, string message, Exception? exception = null) =>
        UnauthorizedTasksAccess(logger, userId, message, exception);

    public static void LogUnauthorizedTaskDeletion(ILogger logger, int taskId, int? userId, string message, Exception? exception = null) =>
        UnauthorizedTaskDeletion(logger, taskId, userId, message, exception);

    public static void LogUnauthorizedTaskUpdate(ILogger logger, int taskId, int? userId, string message, Exception? exception = null) =>
        UnauthorizedTaskUpdate(logger, taskId, userId, message, exception);

    public static void LogUnauthorizedTaskStatusUpdate(ILogger logger, int taskId, int? userId, string message, Exception? exception = null) =>
        UnauthorizedTaskStatusUpdate(logger, taskId, userId, message, exception);

    public static void LogInvalidPaginationParameters(ILogger logger, int pageNumber, int rowCount, Exception? exception = null) =>
        InvalidPaginationParameters(logger, pageNumber, rowCount, exception);

    public static void LogInvalidTaskDataProvided(ILogger logger, string message, Exception? exception = null) =>
        InvalidTaskDataProvided(logger, message, exception);

    public static void LogInvalidStatusDataProvided(ILogger logger, string message, Exception? exception = null) =>
        InvalidStatusDataProvided(logger, message, exception);

    public static void LogReferencedEntityNotFound(ILogger logger, string message, Exception? exception = null) =>
        ReferencedEntityNotFound(logger, message, exception);

    public static void LogTagNotFoundForUser(ILogger logger, int tagId, int? userId, string message, Exception? exception = null) =>
        TagNotFoundForUser(logger, tagId, userId, message, exception);

    public static void LogUnauthorizedTagOperation(ILogger logger, int taskId, int tagId, int? userId, string message, Exception? exception = null) =>
        UnauthorizedTagOperation(logger, taskId, tagId, userId, message, exception);

    public static void LogInvalidTagDataProvided(ILogger logger, string message, Exception? exception = null) =>
        InvalidTagDataProvided(logger, message, exception);

    // Public methods for Error level logs
    public static void LogUnableToDeleteTask(ILogger logger, int taskId, int? userId, string message, Exception? exception = null) =>
        UnableToDeleteTask(logger, taskId, userId, message, exception);

    public static void LogUnableToUpdateTask(ILogger logger, int taskId, int? userId, string message, Exception? exception = null) =>
        UnableToUpdateTask(logger, taskId, userId, message, exception);

    public static void LogUnableToUpdateTaskStatus(ILogger logger, int taskId, int? userId, string message, Exception? exception = null) =>
        UnableToUpdateTaskStatus(logger, taskId, userId, message, exception);

    public static void LogUnexpectedErrorRetrievingTask(ILogger logger, int taskId, Exception exception) =>
        UnexpectedErrorRetrievingTask(logger, taskId, exception);

    public static void LogUnexpectedErrorRetrievingListTasks(ILogger logger, int listId, Exception exception) =>
        UnexpectedErrorRetrievingListTasks(logger, listId, exception);

    public static void LogUnexpectedErrorRetrievingPaginatedListTasks(ILogger logger, int listId, Exception exception) =>
        UnexpectedErrorRetrievingPaginatedListTasks(logger, listId, exception);

    public static void LogUnexpectedErrorRetrievingUserTasks(ILogger logger, int? userId, Exception exception) =>
        UnexpectedErrorRetrievingUserTasks(logger, userId, exception);

    public static void LogUnexpectedErrorRetrievingPaginatedUserTasks(ILogger logger, int? userId, Exception exception) =>
        UnexpectedErrorRetrievingPaginatedUserTasks(logger, userId, exception);

    public static void LogUnexpectedErrorDeletingTask(ILogger logger, int taskId, Exception exception) =>
        UnexpectedErrorDeletingTask(logger, taskId, exception);

    public static void LogUnexpectedErrorCreatingTask(ILogger logger, Exception exception) =>
        UnexpectedErrorCreatingTask(logger, exception);

    public static void LogUnexpectedErrorUpdatingTask(ILogger logger, int taskId, Exception exception) =>
        UnexpectedErrorUpdatingTask(logger, taskId, exception);

    public static void LogUnexpectedErrorUpdatingTaskStatus(ILogger logger, int taskId, Exception exception) =>
        UnexpectedErrorUpdatingTaskStatus(logger, taskId, exception);

    public static void LogUnexpectedErrorRetrievingTags(ILogger logger, int? userId, Exception exception) =>
        UnexpectedErrorRetrievingTags(logger, userId, exception);

    public static void LogUnexpectedErrorRetrievingTaggedTasks(ILogger logger, int tagId, int? userId, Exception exception) =>
        UnexpectedErrorRetrievingTaggedTasks(logger, tagId, userId, exception);

    public static void LogUnexpectedErrorAddingTag(ILogger logger, int taskId, int tagId, Exception exception) =>
        UnexpectedErrorAddingTag(logger, taskId, tagId, exception);

    public static void LogUnexpectedErrorRemovingTag(ILogger logger, int taskId, int tagId, Exception exception) =>
        UnexpectedErrorRemovingTag(logger, taskId, tagId, exception);

    // Public methods for Information level logs
    public static void LogTaskDeletedSuccessfully(ILogger logger, int taskId, int? userId) =>
        TaskDeletedSuccessfully(logger, taskId, userId, null);

    public static void LogTaskCreatedSuccessfully(ILogger logger, int taskId, int? userId) =>
        TaskCreatedSuccessfully(logger, taskId, userId, null);

    public static void LogTaskUpdatedSuccessfully(ILogger logger, int taskId, int? userId) =>
        TaskUpdatedSuccessfully(logger, taskId, userId, null);

    public static void LogTaskStatusUpdatedSuccessfully(ILogger logger, int taskId, int? userId) =>
        TaskStatusUpdatedSuccessfully(logger, taskId, userId, null);

    public static void LogTagsRetrievedSuccessfully(ILogger logger, int count, int? userId) =>
        TagsRetrievedSuccessfully(logger, count, userId, null);

    public static void LogTaggedTasksRetrievedSuccessfully(ILogger logger, int count, int tagId, int? userId) =>
        TaggedTasksRetrievedSuccessfully(logger, count, tagId, userId, null);

    public static void LogTagAddedToTask(ILogger logger, int taskId, int tagId, int? userId) =>
        TagAddedToTask(logger, tagId, taskId, userId, null);

    public static void LogTagRemovedFromTask(ILogger logger, int taskId, int tagId, int? userId) =>
        TagRemovedFromTask(logger, tagId, taskId, userId, null);
}

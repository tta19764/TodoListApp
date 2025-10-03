namespace TodoListApp.WebApp.CustomLogs;

public static class TodoTasksLog
{
    private static readonly Action<ILogger, int, int, Exception?> TaskDetailsRetrieved =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1501, nameof(TaskDetailsRetrieved)),
            "Task {TaskId} details retrieved successfully for user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> TaskCreatedSuccessfully =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1502, nameof(TaskCreatedSuccessfully)),
            "Task created successfully in list {ListId} by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> TaskUpdatedSuccessfully =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1503, nameof(TaskUpdatedSuccessfully)),
            "Task {TaskId} updated successfully by user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TaskStatusUpdated =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1504, nameof(TaskStatusUpdated)),
            "Task {TaskId} status updated to {StatusId} by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> TaskDeletedSuccessfully =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1505, nameof(TaskDeletedSuccessfully)),
            "Task {TaskId} deleted successfully by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> AssignedTasksRetrieved =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1506, nameof(AssignedTasksRetrieved)),
            "Successfuly retrieved {Count} assigned tasks for user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> TaskNotFoundForUser =
        LoggerMessage.Define<int, int>(
            LogLevel.Warning,
            new EventId(2501, nameof(TaskNotFoundForUser)),
            "Task {TaskId} not found for user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> ListNotFoundForUser =
        LoggerMessage.Define<int, int>(
            LogLevel.Warning,
            new EventId(2502, nameof(ListNotFoundForUser)),
            "List {ListId} not found for user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> TaskNotFoundForEdit =
        LoggerMessage.Define<int, int>(
            LogLevel.Warning,
            new EventId(2503, nameof(TaskNotFoundForEdit)),
            "Task {TaskId} not found for edit by user {UserId}");

    private static readonly Action<ILogger, int, Exception?> ErrorRetrievingTaskDetails =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3501, nameof(ErrorRetrievingTaskDetails)),
            "Error retrieving task details for task {TaskId}");

    private static readonly Action<ILogger, int, Exception?> ErrorLoadingCreatePage =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3502, nameof(ErrorLoadingCreatePage)),
            "Error loading create task page for list {ListId}");

    private static readonly Action<ILogger, int, Exception?> ErrorCreatingTask =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3503, nameof(ErrorCreatingTask)),
            "Error creating task in list {ListId}");

    private static readonly Action<ILogger, int, Exception?> ErrorLoadingEditPage =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3504, nameof(ErrorLoadingEditPage)),
            "Error loading edit page for task {TaskId}");

    private static readonly Action<ILogger, int, Exception?> ErrorUpdatingTask =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3505, nameof(ErrorUpdatingTask)),
            "Error updating task {TaskId}");

    private static readonly Action<ILogger, int, Exception?> ErrorUpdatingTaskStatus =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3506, nameof(ErrorUpdatingTaskStatus)),
            "Error updating status for task {TaskId}");

    private static readonly Action<ILogger, int, Exception?> ErrorDeletingTask =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3507, nameof(ErrorDeletingTask)),
            "Error deleting task {TaskId}");

    private static readonly Action<ILogger, int, Exception?> CreatePageLoadedSuccessfully =
        LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(3508, nameof(CreatePageLoadedSuccessfully)),
            "Create task page loaded successfully for list {ListId}");

    private static readonly Action<ILogger, int, Exception?> EditPageLoadedSuccessfully =
        LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(3509, nameof(EditPageLoadedSuccessfully)),
            "Edit task page loaded successfully for task {TaskId}");

    private static readonly Action<ILogger, Exception?> ErrorLoadingAssignedTasks =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3510, nameof(ErrorLoadingAssignedTasks)),
            "Error loading assigned tasks");

    public static void LogTaskDetailsRetrieved(ILogger logger, int taskId, int userId)
        => TaskDetailsRetrieved(logger, taskId, userId, null);

    public static void LogTaskNotFoundForUser(ILogger logger, int taskId, int userId)
        => TaskNotFoundForUser(logger, taskId, userId, null);

    public static void LogListNotFoundForUser(ILogger logger, int listId, int userId)
        => ListNotFoundForUser(logger, listId, userId, null);

    public static void LogTaskCreatedSuccessfully(ILogger logger, int listId, int userId)
        => TaskCreatedSuccessfully(logger, listId, userId, null);

    public static void LogTaskUpdatedSuccessfully(ILogger logger, int taskId, int userId)
        => TaskUpdatedSuccessfully(logger, taskId, userId, null);

    public static void LogTaskStatusUpdated(ILogger logger, int taskId, int statusId, int userId)
        => TaskStatusUpdated(logger, taskId, statusId, userId, null);

    public static void LogTaskDeletedSuccessfully(ILogger logger, int taskId, int userId)
        => TaskDeletedSuccessfully(logger, taskId, userId, null);

    public static void LogAssignedTasksRetrieved(ILogger logger, int count, int userId)
        => AssignedTasksRetrieved(logger, count, userId, null);

    public static void LogErrorRetrievingTaskDetails(ILogger logger, int taskId, Exception ex)
        => ErrorRetrievingTaskDetails(logger, taskId, ex);

    public static void LogErrorLoadingCreatePage(ILogger logger, int listId, Exception ex)
        => ErrorLoadingCreatePage(logger, listId, ex);

    public static void LogErrorCreatingTask(ILogger logger, int listId, Exception ex)
        => ErrorCreatingTask(logger, listId, ex);

    public static void LogErrorLoadingEditPage(ILogger logger, int taskId, Exception ex)
        => ErrorLoadingEditPage(logger, taskId, ex);

    public static void LogErrorUpdatingTask(ILogger logger, int taskId, Exception ex)
        => ErrorUpdatingTask(logger, taskId, ex);

    public static void LogErrorUpdatingTaskStatus(ILogger logger, int taskId, Exception ex)
        => ErrorUpdatingTaskStatus(logger, taskId, ex);

    public static void LogErrorDeletingTask(ILogger logger, int taskId, Exception ex)
        => ErrorDeletingTask(logger, taskId, ex);

    public static void LogTaskNotFoundForEdit(ILogger logger, int taskId, int userId)
        => TaskNotFoundForEdit(logger, taskId, userId, null);

    public static void LogCreatePageLoadedSuccessfully(ILogger logger, int listId)
        => CreatePageLoadedSuccessfully(logger, listId, null);

    public static void LogEditPageLoadedSuccessfully(ILogger logger, int taskId)
        => EditPageLoadedSuccessfully(logger, taskId, null);

    public static void LogErrorLoadingAssignedTasks(ILogger logger, Exception ex)
        => ErrorLoadingAssignedTasks(logger, ex);
}

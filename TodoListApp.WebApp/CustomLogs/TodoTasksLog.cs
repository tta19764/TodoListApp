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

    private static readonly Action<ILogger, int, int, Exception?> SearchTasksRetrieved =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1507, nameof(SearchTasksRetrieved)),
            "Successfuly retrieved {Count} search tasks for user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> CommentAddedToTask =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1508, nameof(CommentAddedToTask)),
            "Comment {CommentId} added to task {TaskId} by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> CommentUpdated =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1509, nameof(CommentUpdated)),
            "Comment {CommentId} updated by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> CommentDeleted =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1510, nameof(CommentDeleted)),
            "Comment {CommentId} deleted by user {UserId}");

    private static readonly Action<ILogger, int, Exception?> AddCommentPageLoaded =
        LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(1511, nameof(AddCommentPageLoaded)),
            "Add comment page loaded for task {TaskId}");

    private static readonly Action<ILogger, int, Exception?> EditCommentPageLoaded =
        LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(1512, nameof(EditCommentPageLoaded)),
            "Edit comment page loaded for comment {CommentId}");

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

    private static readonly Action<ILogger, int, int, Exception?> CommentNotFoundForUser =
        LoggerMessage.Define<int, int>(
            LogLevel.Warning,
            new EventId(2504, nameof(CommentNotFoundForUser)),
            "Comment {CommentId} not found for user {UserId}");

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

    private static readonly Action<ILogger, Exception?> ErrorLoadingSearchedTasks =
            LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3511, nameof(ErrorLoadingSearchedTasks)),
            "Error loading searched tasks");

    private static readonly Action<ILogger, int, Exception?> ErrorLoadingAddCommentPage =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3512, nameof(ErrorLoadingAddCommentPage)),
            "Error loading add comment page for task {TaskId}");

    private static readonly Action<ILogger, int, Exception?> ErrorAddingComment =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3513, nameof(ErrorAddingComment)),
            "Error adding comment to task {TaskId}");

    private static readonly Action<ILogger, int, Exception?> ErrorLoadingEditCommentPage =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3514, nameof(ErrorLoadingEditCommentPage)),
            "Error loading edit comment page for comment {CommentId}");

    private static readonly Action<ILogger, int, Exception?> ErrorUpdatingComment =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3515, nameof(ErrorUpdatingComment)),
            "Error updating comment {CommentId}");

    private static readonly Action<ILogger, int, Exception?> ErrorDeletingComment =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3516, nameof(ErrorDeletingComment)),
            "Error deleting comment {CommentId}");

    /// <summary>
    /// Logs that task details have been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task whose details were retrieved.</param>
    /// <param name="userId">The ID of the user who retrieved the details.</param>
    public static void LogTaskDetailsRetrieved(ILogger logger, int taskId, int userId)
        => TaskDetailsRetrieved(logger, taskId, userId, null);

    /// <summary>
    /// Logs a warning that a task was not found for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task that was not found.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogTaskNotFoundForUser(ILogger logger, int taskId, int userId)
        => TaskNotFoundForUser(logger, taskId, userId, null);

    /// <summary>
    /// Logs a warning that a list was not found for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the list that was not found.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogListNotFoundForUser(ILogger logger, int listId, int userId)
        => ListNotFoundForUser(logger, listId, userId, null);

    /// <summary>
    /// Logs that a task has been successfully created in a list.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the list containing the new task.</param>
    /// <param name="userId">The ID of the user who created the task.</param>
    public static void LogTaskCreatedSuccessfully(ILogger logger, int listId, int userId)
        => TaskCreatedSuccessfully(logger, listId, userId, null);

    /// <summary>
    /// Logs that a task has been successfully updated.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the updated task.</param>
    /// <param name="userId">The ID of the user who updated the task.</param>
    public static void LogTaskUpdatedSuccessfully(ILogger logger, int taskId, int userId)
        => TaskUpdatedSuccessfully(logger, taskId, userId, null);

    /// <summary>
    /// Logs that a task's status has been successfully updated.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="statusId">The new status ID.</param>
    /// <param name="userId">The ID of the user who updated the status.</param>
    public static void LogTaskStatusUpdated(ILogger logger, int taskId, int statusId, int userId)
        => TaskStatusUpdated(logger, taskId, statusId, userId, null);

    /// <summary>
    /// Logs that a task has been successfully deleted.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the deleted task.</param>
    /// <param name="userId">The ID of the user who deleted the task.</param>
    public static void LogTaskDeletedSuccessfully(ILogger logger, int taskId, int userId)
        => TaskDeletedSuccessfully(logger, taskId, userId, null);

    /// <summary>
    /// Logs that assigned tasks have been successfully retrieved for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of assigned tasks retrieved.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogAssignedTasksRetrieved(ILogger logger, int count, int userId)
        => AssignedTasksRetrieved(logger, count, userId, null);

    /// <summary>
    /// Logs that search results for tasks have been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks found in search.</param>
    /// <param name="userId">The ID of the user who performed the search.</param>
    public static void LogSearchTasksRetrieved(ILogger logger, int count, int userId)
        => SearchTasksRetrieved(logger, count, userId, null);

    /// <summary>
    /// Logs that a comment has been successfully added to a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the added comment.</param>
    /// <param name="taskId">The ID of the task to which the comment was added.</param>
    /// <param name="userId">The ID of the user who added the comment.</param>
    public static void LogCommentAddedToTask(ILogger logger, int commentId, int taskId, int userId) =>
        CommentAddedToTask(logger, commentId, taskId, userId, null);

    /// <summary>
    /// Logs that a comment has been successfully updated.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the updated comment.</param>
    /// <param name="userId">The ID of the user who updated the comment.</param>
    public static void LogCommentUpdated(ILogger logger, int commentId, int userId) =>
        CommentUpdated(logger, commentId, userId, null);

    /// <summary>
    /// Logs that a comment has been successfully deleted.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the deleted comment.</param>
    /// <param name="userId">The ID of the user who deleted the comment.</param>
    public static void LogCommentDeleted(ILogger logger, int commentId, int userId) =>
        CommentDeleted(logger, commentId, userId, null);

    /// <summary>
    /// Logs that the add comment page has been loaded for a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task.</param>
    public static void LogAddCommentPageLoaded(ILogger logger, int taskId) =>
        AddCommentPageLoaded(logger, taskId, null);

    /// <summary>
    /// Logs that the edit comment page has been loaded.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the comment being edited.</param>
    public static void LogEditCommentPageLoaded(ILogger logger, int commentId) =>
        EditCommentPageLoaded(logger, commentId, null);

    /// <summary>
    /// Logs an error that occurred while retrieving task details.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorRetrievingTaskDetails(ILogger logger, int taskId, Exception ex)
        => ErrorRetrievingTaskDetails(logger, taskId, ex);

    /// <summary>
    /// Logs an error that occurred while loading the create task page.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the list.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorLoadingCreatePage(ILogger logger, int listId, Exception ex)
        => ErrorLoadingCreatePage(logger, listId, ex);

    /// <summary>
    /// Logs an error that occurred while creating a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the list.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorCreatingTask(ILogger logger, int listId, Exception ex)
        => ErrorCreatingTask(logger, listId, ex);

    /// <summary>
    /// Logs a warning that a comment was not found for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the comment that was not found.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogCommentNotFoundForUser(ILogger logger, int commentId, int userId) =>
        CommentNotFoundForUser(logger, commentId, userId, null);

    /// <summary>
    /// Logs an error that occurred while loading the edit task page.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorLoadingEditPage(ILogger logger, int taskId, Exception ex)
        => ErrorLoadingEditPage(logger, taskId, ex);

    /// <summary>
    /// Logs an error that occurred while updating a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task being updated.</param>
    /// <param name="ex">The exception that occurred.</param>s
    public static void LogErrorUpdatingTask(ILogger logger, int taskId, Exception ex)
        => ErrorUpdatingTask(logger, taskId, ex);

    /// <summary>
    /// Logs an error that occurred while updating a task's status.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="ex">The exception that occurred.</param>s
    public static void LogErrorUpdatingTaskStatus(ILogger logger, int taskId, Exception ex)
        => ErrorUpdatingTaskStatus(logger, taskId, ex);

    /// <summary>
    /// Logs an error that occurred while deleting a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task being deleted.</param>
    /// <param name="ex">The exception that occurred.</param>s
    public static void LogErrorDeletingTask(ILogger logger, int taskId, Exception ex)
        => ErrorDeletingTask(logger, taskId, ex);

    /// <summary>
    /// Logs a warning that a task was not found for editing.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task that was not found.</param>
    /// <param name="userId">The ID of the user attempting to edit.</param>
    public static void LogTaskNotFoundForEdit(ILogger logger, int taskId, int userId)
        => TaskNotFoundForEdit(logger, taskId, userId, null);

    /// <summary>
    /// Logs that the create task page has been loaded successfully.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the list.</param>
    public static void LogCreatePageLoadedSuccessfully(ILogger logger, int listId)
        => CreatePageLoadedSuccessfully(logger, listId, null);

    /// <summary>
    /// Logs that the edit task page has been loaded successfully.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task being edited.</param>
    public static void LogEditPageLoadedSuccessfully(ILogger logger, int taskId)
        => EditPageLoadedSuccessfully(logger, taskId, null);

    /// <summary>
    /// Logs an error that occurred while loading assigned tasks.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorLoadingAssignedTasks(ILogger logger, Exception ex)
        => ErrorLoadingAssignedTasks(logger, ex);

    /// <summary>
    /// Logs an error that occurred while loading searched tasks.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorLoadingSearchedTasks(ILogger logger, Exception ex)
        => ErrorLoadingSearchedTasks(logger, ex);

    /// <summary>
    /// Logs an error that occurred while loading the add comment page.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorLoadingAddCommentPage(ILogger logger, int taskId, Exception ex) =>
        ErrorLoadingAddCommentPage(logger, taskId, ex);

    /// <summary>
    /// Logs an error that occurred while adding a comment to a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorAddingComment(ILogger logger, int taskId, Exception ex) =>
        ErrorAddingComment(logger, taskId, ex);

    /// <summary>
    /// Logs an error that occurred while loading the edit comment page.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorLoadingEditCommentPage(ILogger logger, int commentId, Exception ex) =>
        ErrorLoadingEditCommentPage(logger, commentId, ex);

    /// <summary>
    /// Logs an error that occurred while updating a comment.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the comment being updated.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorUpdatingComment(ILogger logger, int commentId, Exception ex) =>
        ErrorUpdatingComment(logger, commentId, ex);

    /// <summary>
    /// Logs an error that occurred while deleting a comment.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="commentId">The ID of the comment being deleted.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorDeletingComment(ILogger logger, int commentId, Exception ex) =>
        ErrorDeletingComment(logger, commentId, ex);
}

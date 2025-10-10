namespace TodoListApp.WebApp.CustomLogs;

public static class TagsLog
{
    private static readonly Action<ILogger, int, int, Exception?> TagsRetrievedForUser =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1801, nameof(TagsRetrievedForUser)),
            "Retrieved {Count} tags for user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TasksRetrievedForTag =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1802, nameof(TasksRetrievedForTag)),
            "Retrieved {Count} tasks for tag {TagId} and user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TagAddedToTask =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1803, nameof(TagAddedToTask)),
            "Tag {TagId} added to task {TaskId} by user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TagRemovedFromTask =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(1804, nameof(TagRemovedFromTask)),
            "Tag {TagId} removed from task {TaskId} by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> AddTagPageLoaded =
        LoggerMessage.Define<int, int>(
            LogLevel.Debug,
            new EventId(4801, nameof(AddTagPageLoaded)),
            "Loaded Add Tag page for task {TaskId} with {Count} available tags");

    private static readonly Action<ILogger, int, Exception?> TagNotFound =
        LoggerMessage.Define<int>(
            LogLevel.Warning,
            new EventId(2801, nameof(TagNotFound)),
            "Tag {TagId} not found");

    private static readonly Action<ILogger, Exception?> ErrorLoadingTagsList =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3801, nameof(ErrorLoadingTagsList)),
            "Error loading tags list");

    private static readonly Action<ILogger, Exception?> ErrorLoadingTasksForTag =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3802, nameof(ErrorLoadingTasksForTag)),
            "Error loading tasks for a specific tag");

    private static readonly Action<ILogger, Exception?> ErrorLoadingAddTagPage =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3803, nameof(ErrorLoadingAddTagPage)),
            "Error loading Add Tag page");

    private static readonly Action<ILogger, Exception?> ErrorAddingTagToTask =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3804, nameof(ErrorAddingTagToTask)),
            "Error adding tag to task");

    private static readonly Action<ILogger, Exception?> ErrorRemovingTagFromTask =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3805, nameof(ErrorRemovingTagFromTask)),
            "Error removing tag from task");

    /// <summary>
    /// Logs that tags have been successfully retrieved for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tags retrieved.</param>
    /// <param name="userId">The ID of the user whose tags were retrieved.</param>
    public static void LogTagsRetrievedForUser(ILogger logger, int count, int userId)
        => TagsRetrievedForUser(logger, count, userId, null);

    /// <summary>
    /// Logs that tasks have been successfully retrieved for a specific tag.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks retrieved.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogTasksRetrievedForTag(ILogger logger, int count, int tagId, int userId)
        => TasksRetrievedForTag(logger, count, tagId, userId, null);

    /// <summary>
    /// Logs that a tag has been successfully added to a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag that was added.</param>
    /// <param name="taskId">The ID of the task to which the tag was added.</param>
    /// <param name="userId">The ID of the user who added the tag.</param>
    public static void LogTagAddedToTask(ILogger logger, int tagId, int taskId, int userId)
        => TagAddedToTask(logger, tagId, taskId, userId, null);

    /// <summary>
    /// Logs that a tag has been successfully removed from a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag that was removed.</param>
    /// <param name="taskId">The ID of the task from which the tag was removed.</param>
    /// <param name="userId">The ID of the user who removed the tag.</param>
    public static void LogTagRemovedFromTask(ILogger logger, int tagId, int taskId, int userId)
        => TagRemovedFromTask(logger, tagId, taskId, userId, null);

    /// <summary>
    /// Logs that the add tag page has been loaded for a specific task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="count">The number of available tags.</param>
    public static void LogAddTagPageLoaded(ILogger logger, int taskId, int count)
        => AddTagPageLoaded(logger, taskId, count, null);

    /// <summary>
    /// Logs a warning that a tag was not found.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag that was not found.</param>
    public static void LogTagNotFound(ILogger logger, int tagId)
        => TagNotFound(logger, tagId, null);

    /// <summary>
    /// Logs an error that occurred while loading the tags list.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorLoadingTagsList(ILogger logger, Exception ex)
        => ErrorLoadingTagsList(logger, ex);

    /// <summary>
    /// Logs an error that occurred while loading tasks for a specific tag.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorLoadingTasksForTag(ILogger logger, Exception ex)
        => ErrorLoadingTasksForTag(logger, ex);

    /// <summary>
    /// Logs an error that occurred while loading the add tag page.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorLoadingAddTagPage(ILogger logger, Exception ex)
        => ErrorLoadingAddTagPage(logger, ex);

    /// <summary>
    /// Logs an error that occurred while adding a tag to a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorAddingTagToTask(ILogger logger, Exception ex)
        => ErrorAddingTagToTask(logger, ex);

    /// <summary>
    /// Logs an error that occurred while removing a tag from a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorRemovingTagFromTask(ILogger logger, Exception ex)
        => ErrorRemovingTagFromTask(logger, ex);
}

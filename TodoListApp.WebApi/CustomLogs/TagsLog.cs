namespace TodoListApp.WebApi.CustomLogs;

/// <summary>
/// Custom logging class for Tag related operations.
/// </summary>
internal static class TagLog
{
    // Information level logs
    private static readonly Action<ILogger, int, Exception?> TagRetrieved =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1401, nameof(TagRetrieved)),
            "Tag {TagId} retrieved successfully");

    private static readonly Action<ILogger, int, Exception?> TagsRetrieved =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1402, nameof(TagsRetrieved)),
            "Retrieved {Count} tags successfully");

    private static readonly Action<ILogger, int, Exception?> TagCreated =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1403, nameof(TagCreated)),
            "Tag {TagId} created successfully");

    private static readonly Action<ILogger, int, Exception?> TagUpdated =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1404, nameof(TagUpdated)),
            "Tag {TagId} updated successfully");

    private static readonly Action<ILogger, int, Exception?> TagDeleted =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1405, nameof(TagDeleted)),
            "Tag {TagId} deleted successfully");

    private static readonly Action<ILogger, int, int, Exception?> AvailableTagsRetrieved =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1406, nameof(AvailableTagsRetrieved)),
            "Retrieved {Count} available tags for task {TaskId}");

    // Warning level logs
    private static readonly Action<ILogger, int, Exception?> TagNotFound =
        LoggerMessage.Define<int>(
            LogLevel.Warning,
            new EventId(2401, nameof(TagNotFound)),
            "Tag with ID {TagId} not found");

    private static readonly Action<ILogger, int, int, Exception?> TaskNotFoundForAvailableTags =
        LoggerMessage.Define<int, int>(
            LogLevel.Warning,
            new EventId(2402, nameof(TaskNotFoundForAvailableTags)),
            "Task {TaskId} not found when retrieving available tags. Count: {Count}");

    private static readonly Action<ILogger, int, int, Exception?> InvalidPaginationParameters =
        LoggerMessage.Define<int, int>(
            LogLevel.Warning,
            new EventId(2403, nameof(InvalidPaginationParameters)),
            "Invalid pagination parameters: pageNumber={PageNumber}, rowCount={RowCount}");

    private static readonly Action<ILogger, string, Exception?> InvalidTagData =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2404, nameof(InvalidTagData)),
            "Invalid tag data provided: {Message}");

    private static readonly Action<ILogger, int, Exception?> TagAlreadyExists =
        LoggerMessage.Define<int>(
            LogLevel.Warning,
            new EventId(2405, nameof(TagAlreadyExists)),
            "Tag with ID {TagId} already exists");

    // Error level logs
    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorRetrievingTag =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3401, nameof(UnexpectedErrorRetrievingTag)),
            "Unexpected error occurred while retrieving tag {TagId}");

    private static readonly Action<ILogger, Exception?> UnexpectedErrorRetrievingTags =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3402, nameof(UnexpectedErrorRetrievingTags)),
            "Unexpected error occurred while retrieving tags");

    private static readonly Action<ILogger, Exception?> UnexpectedErrorCreatingTag =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3403, nameof(UnexpectedErrorCreatingTag)),
            "Unexpected error occurred while creating tag");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorUpdatingTag =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3404, nameof(UnexpectedErrorUpdatingTag)),
            "Unexpected error occurred while updating tag {TagId}");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorDeletingTag =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3405, nameof(UnexpectedErrorDeletingTag)),
            "Unexpected error occurred while deleting tag {TagId}");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorRetrievingAvailableTags =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3406, nameof(UnexpectedErrorRetrievingAvailableTags)),
            "Unexpected error occurred while retrieving available tags for task {TaskId}");

    private static readonly Action<ILogger, int, string, Exception?> UnableToDeleteTag =
        LoggerMessage.Define<int, string>(
            LogLevel.Error,
            new EventId(3407, nameof(UnableToDeleteTag)),
            "Unable to delete tag {TagId}: {Message}");

    private static readonly Action<ILogger, int, string, Exception?> UnableToUpdateTag =
        LoggerMessage.Define<int, string>(
            LogLevel.Error,
            new EventId(3408, nameof(UnableToUpdateTag)),
            "Unable to update tag {TagId}: {Message}");

    // Public methods for Information level logs

    /// <summary>
    /// Logs that a tag has been successfully retrieved.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag that was retrieved.</param>
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
    /// Logs that a tag has been successfully created.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the created tag.</param>
    public static void LogTagCreated(ILogger logger, int tagId) =>
        TagCreated(logger, tagId, null);

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
    /// Logs a warning that a task was not found when retrieving available tags.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task that was not found.</param>
    /// <param name="count">The count of tags retrieved.</param>
    public static void LogTaskNotFoundForAvailableTags(ILogger logger, int taskId, int count) =>
        TaskNotFoundForAvailableTags(logger, taskId, count, null);

    /// <summary>
    /// Logs a warning about invalid pagination parameters.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="pageNumber">The invalid page number.</param>
    /// <param name="rowCount">The invalid row count.</param>
    public static void LogInvalidPaginationParameters(ILogger logger, int pageNumber, int rowCount) =>
        InvalidPaginationParameters(logger, pageNumber, rowCount, null);

    /// <summary>
    /// Logs a warning about invalid tag data provided.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="message">Description of the invalid data.</param>
    public static void LogInvalidTagData(ILogger logger, string message) =>
        InvalidTagData(logger, message, null);

    /// <summary>
    /// Logs a warning that a tag already exists.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag that already exists.</param>
    public static void LogTagAlreadyExists(ILogger logger, int tagId) =>
        TagAlreadyExists(logger, tagId, null);

    // Public methods for Error level logs

    /// <summary>
    /// Logs an error that an unexpected exception occurred while retrieving a tag.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag being retrieved.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorRetrievingTag(ILogger logger, int tagId, Exception exception) =>
        UnexpectedErrorRetrievingTag(logger, tagId, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred while retrieving tags.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorRetrievingTags(ILogger logger, Exception exception) =>
        UnexpectedErrorRetrievingTags(logger, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred while creating a tag.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorCreatingTag(ILogger logger, Exception exception) =>
        UnexpectedErrorCreatingTag(logger, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred while updating a tag.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag being updated.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorUpdatingTag(ILogger logger, int tagId, Exception exception) =>
        UnexpectedErrorUpdatingTag(logger, tagId, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred while deleting a tag.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag being deleted.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorDeletingTag(ILogger logger, int tagId, Exception exception) =>
        UnexpectedErrorDeletingTag(logger, tagId, exception);

    /// <summary>
    /// Logs an error that an unexpected exception occurred while retrieving available tags for a task.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="exception">The exception that occurred.</param>
    public static void LogUnexpectedErrorRetrievingAvailableTags(ILogger logger, int taskId, Exception exception) =>
        UnexpectedErrorRetrievingAvailableTags(logger, taskId, exception);

    /// <summary>
    /// Logs an error that a tag could not be deleted.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag that could not be deleted.</param>
    /// <param name="message">Description of why the deletion failed.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogUnableToDeleteTag(ILogger logger, int tagId, string message, Exception? exception = null) =>
        UnableToDeleteTag(logger, tagId, message, exception);

    /// <summary>
    /// Logs an error that a tag could not be updated.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="tagId">The ID of the tag that could not be updated.</param>
    /// <param name="message">Description of why the update failed.</param>
    /// <param name="exception">Optional exception that occurred.</param>
    public static void LogUnableToUpdateTag(ILogger logger, int tagId, string message, Exception? exception = null) =>
        UnableToUpdateTag(logger, tagId, message, exception);
}

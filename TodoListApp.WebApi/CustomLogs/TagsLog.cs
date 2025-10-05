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
    public static void LogTagRetrieved(ILogger logger, int tagId) =>
        TagRetrieved(logger, tagId, null);

    public static void LogTagsRetrieved(ILogger logger, int count) =>
        TagsRetrieved(logger, count, null);

    public static void LogTagCreated(ILogger logger, int tagId) =>
        TagCreated(logger, tagId, null);

    public static void LogTagUpdated(ILogger logger, int tagId) =>
        TagUpdated(logger, tagId, null);

    public static void LogTagDeleted(ILogger logger, int tagId) =>
        TagDeleted(logger, tagId, null);

    public static void LogAvailableTagsRetrieved(ILogger logger, int count, int taskId) =>
        AvailableTagsRetrieved(logger, count, taskId, null);

    // Public methods for Warning level logs
    public static void LogTagNotFound(ILogger logger, int tagId) =>
        TagNotFound(logger, tagId, null);

    public static void LogTaskNotFoundForAvailableTags(ILogger logger, int taskId, int count) =>
        TaskNotFoundForAvailableTags(logger, taskId, count, null);

    public static void LogInvalidPaginationParameters(ILogger logger, int pageNumber, int rowCount) =>
        InvalidPaginationParameters(logger, pageNumber, rowCount, null);

    public static void LogInvalidTagData(ILogger logger, string message) =>
        InvalidTagData(logger, message, null);

    public static void LogTagAlreadyExists(ILogger logger, int tagId) =>
        TagAlreadyExists(logger, tagId, null);

    // Public methods for Error level logs
    public static void LogUnexpectedErrorRetrievingTag(ILogger logger, int tagId, Exception exception) =>
        UnexpectedErrorRetrievingTag(logger, tagId, exception);

    public static void LogUnexpectedErrorRetrievingTags(ILogger logger, Exception exception) =>
        UnexpectedErrorRetrievingTags(logger, exception);

    public static void LogUnexpectedErrorCreatingTag(ILogger logger, Exception exception) =>
        UnexpectedErrorCreatingTag(logger, exception);

    public static void LogUnexpectedErrorUpdatingTag(ILogger logger, int tagId, Exception exception) =>
        UnexpectedErrorUpdatingTag(logger, tagId, exception);

    public static void LogUnexpectedErrorDeletingTag(ILogger logger, int tagId, Exception exception) =>
        UnexpectedErrorDeletingTag(logger, tagId, exception);

    public static void LogUnexpectedErrorRetrievingAvailableTags(ILogger logger, int taskId, Exception exception) =>
        UnexpectedErrorRetrievingAvailableTags(logger, taskId, exception);

    public static void LogUnableToDeleteTag(ILogger logger, int tagId, string message, Exception? exception = null) =>
        UnableToDeleteTag(logger, tagId, message, exception);

    public static void LogUnableToUpdateTag(ILogger logger, int tagId, string message, Exception? exception = null) =>
        UnableToUpdateTag(logger, tagId, message, exception);
}

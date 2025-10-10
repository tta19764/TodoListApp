namespace TodoListApp.WebApp.CustomLogs;

public static class TodoListsLog
{
    private static readonly Action<ILogger, int, int, Exception?> ListsRetrievedForUser =
        LoggerMessage.Define<int, int>(
            LogLevel.Debug,
            new EventId(4001, nameof(ListsRetrievedForUser)),
            "Retrieved {Count} todo lists for user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TasksRetrievedForList =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Debug,
            new EventId(4002, nameof(TasksRetrievedForList)),
            "Retrieved {Count} tasks for list {ListId} by user {UserId}");

    private static readonly Action<ILogger, int, Exception?> CreatePageLoaded =
        LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(4003, nameof(CreatePageLoaded)),
            "Create list page loaded by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> ListCreatedSuccessfully =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(4001, nameof(ListCreatedSuccessfully)),
            "Todo list {ListId} created successfully by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> ListLoadedForEdit =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1002, nameof(ListLoadedForEdit)),
            "Todo list {ListId} loaded for edit by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> ListUpdatedSuccessfully =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1003, nameof(ListUpdatedSuccessfully)),
            "Todo list {ListId} updated successfully by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> ListDeletedSuccessfully =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(1004, nameof(ListDeletedSuccessfully)),
            "Todo list {ListId} deleted successfully by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> ListNotFoundForEdit =
        LoggerMessage.Define<int, int>(
            LogLevel.Warning,
            new EventId(2001, nameof(ListNotFoundForEdit)),
            "Todo list {ListId} not found for edit by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> ListNotFoundForUser =
        LoggerMessage.Define<int, int>(
            LogLevel.Warning,
            new EventId(2002, nameof(ListNotFoundForUser)),
            "List {ListId} not found for user {UserId}");

    private static readonly Action<ILogger, int, Exception?> ErrorLoadingEditPage =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3001, nameof(ErrorLoadingEditPage)),
            "Error loading edit page for list {ListId}");

    private static readonly Action<ILogger, int, Exception?> ErrorUpdatingList =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3002, nameof(ErrorUpdatingList)),
            "Error updating todo list {ListId}");

    private static readonly Action<ILogger, int, Exception?> ErrorDeletingList =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3003, nameof(ErrorDeletingList)),
            "Error deleting todo list {ListId}");

    private static readonly Action<ILogger, Exception?> FailedToLoadTodoLists =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3004, nameof(FailedToLoadTodoLists)),
            "Failed to load todo lists page");

    private static readonly Action<ILogger, Exception?> FailedToCreateTodoList =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3005, nameof(FailedToCreateTodoList)),
            "Failed to create todo list");

    /// <summary>
    /// Logs an error that occurred while loading the to-do lists page.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogFailedToLoadTodoLists(ILogger logger, Exception ex)
        => FailedToLoadTodoLists(logger, ex);

    /// <summary>
    /// Logs an error that occurred while loading the create to-do list page.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogFailedToLoadCreateTodoList(ILogger logger, Exception ex)
        => FailedToCreateTodoList(logger, ex);

    /// <summary>
    /// Logs that a to-do list has been successfully created.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the created list.</param>
    /// <param name="userId">The ID of the user who created the list.</param>
    public static void LogListCreatedSuccessfully(ILogger logger, int listId, int userId)
        => ListCreatedSuccessfully(logger, listId, userId, null);

    /// <summary>
    /// Logs that a to-do list has been loaded for editing.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the list being edited.</param>
    /// <param name="userId">The ID of the user editing the list.</param>
    public static void LogListLoadedForEdit(ILogger logger, int listId, int userId)
        => ListLoadedForEdit(logger, listId, userId, null);

    /// <summary>
    /// Logs a warning that a to-do list was not found for editing.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the list that was not found.</param>
    /// <param name="userId">The ID of the user attempting to edit.</param>
    public static void LogListNotFoundForEdit(ILogger logger, int listId, int userId)
        => ListNotFoundForEdit(logger, listId, userId, null);

    /// <summary>
    /// Logs an error that occurred while loading the edit page for a list.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the list.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorLoadingEditPage(ILogger logger, int listId, Exception ex)
        => ErrorLoadingEditPage(logger, listId, ex);

    /// <summary>
    /// Logs that a to-do list has been successfully updated.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the updated list.</param>
    /// <param name="userId">The ID of the user who updated the list.</param>
    public static void LogListUpdatedSuccessfully(ILogger logger, int listId, int userId)
        => ListUpdatedSuccessfully(logger, listId, userId, null);

    /// <summary>
    /// Logs an error that occurred while updating a to-do list.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the list being updated.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorUpdatingList(ILogger logger, int listId, Exception ex)
        => ErrorUpdatingList(logger, listId, ex);

    /// <summary>
    /// Logs that a to-do list has been successfully deleted.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the deleted list.</param>
    /// <param name="userId">The ID of the user who deleted the list.</param>
    public static void LogListDeletedSuccessfully(ILogger logger, int listId, int userId)
        => ListDeletedSuccessfully(logger, listId, userId, null);

    /// <summary>
    /// Logs an error that occurred while deleting a to-do list.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the list being deleted.</param>
    /// <param name="ex">The exception that occurred.</param>
    public static void LogErrorDeletingList(ILogger logger, int listId, Exception ex)
        => ErrorDeletingList(logger, listId, ex);

    /// <summary>
    /// Logs that to-do lists have been successfully retrieved for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of lists retrieved.</param>
    /// <param name="userId">The ID of the user whose lists were retrieved.</param>
    public static void LogListsRetrievedForUser(ILogger logger, int count, int userId)
        => ListsRetrievedForUser(logger, count, userId, null);

    /// <summary>
    /// Logs that tasks have been successfully retrieved for a specific list.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="count">The number of tasks retrieved.</param>
    /// <param name="listId">The ID of the list.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogTasksRetrievedForList(ILogger logger, int count, int listId, int userId)
        => TasksRetrievedForList(logger, count, listId, userId, null);

    /// <summary>
    /// Logs that the create to-do list page has been loaded.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The ID of the user loading the page.</param>
    public static void LogCreatePageLoaded(ILogger logger, int userId)
        => CreatePageLoaded(logger, userId, null);

    /// <summary>
    /// Logs a warning that a list was not found for a user.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="listId">The ID of the list that was not found.</param>
    /// <param name="userId">The ID of the user.</param>
    public static void LogListNotFoundForUser(ILogger logger, int listId, int userId)
        => ListNotFoundForUser(logger, listId, userId, null);
}

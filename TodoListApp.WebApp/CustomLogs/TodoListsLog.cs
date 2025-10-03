namespace TodoListApp.WebApp.CustomLogs;

public static class TodoListsLog
{
    private static readonly Action<ILogger, Exception?> FailedToLoadTodoLists =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(2001, nameof(FailedToLoadTodoLists)),
            "Failed to load todo lists page");

    private static readonly Action<ILogger, Exception?> FailedToCreateTodoList =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(2002, nameof(FailedToCreateTodoList)),
            "Failed to create todo list");

    private static readonly Action<ILogger, int, int, Exception?> ListCreatedSuccessfully =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(2003, nameof(ListCreatedSuccessfully)),
            "Todo list {ListId} created successfully by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> ListLoadedForEdit =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(2004, nameof(ListLoadedForEdit)),
            "Todo list {ListId} loaded for edit by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> ListNotFoundForEdit =
        LoggerMessage.Define<int, int>(
            LogLevel.Warning,
            new EventId(2005, nameof(ListNotFoundForEdit)),
            "Todo list {ListId} not found for edit by user {UserId}");

    private static readonly Action<ILogger, int, Exception?> ErrorLoadingEditPage =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(2006, nameof(ErrorLoadingEditPage)),
            "Error loading edit page for list {ListId}");

    private static readonly Action<ILogger, int, int, Exception?> ListUpdatedSuccessfully =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(2007, nameof(ListUpdatedSuccessfully)),
            "Todo list {ListId} updated successfully by user {UserId}");

    private static readonly Action<ILogger, int, Exception?> ErrorUpdatingList =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(2008, nameof(ErrorUpdatingList)),
            "Error updating todo list {ListId}");

    private static readonly Action<ILogger, int, int, Exception?> ListDeletedSuccessfully =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(2009, nameof(ListDeletedSuccessfully)),
            "Todo list {ListId} deleted successfully by user {UserId}");

    private static readonly Action<ILogger, int, Exception?> ErrorDeletingList =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(2010, nameof(ErrorDeletingList)),
            "Error deleting todo list {ListId}");

    private static readonly Action<ILogger, int, int, Exception?> ListsRetrievedForUser =
        LoggerMessage.Define<int, int>(
            LogLevel.Debug,
            new EventId(2011, nameof(ListsRetrievedForUser)),
            "Retrieved {Count} todo lists for user {UserId}");

    private static readonly Action<ILogger, int, int, int, Exception?> TasksRetrievedForList =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Debug,
            new EventId(2012, nameof(TasksRetrievedForList)),
            "Retrieved {Count} tasks for list {ListId} by user {UserId}");

    private static readonly Action<ILogger, int, Exception?> CreatePageLoaded =
        LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(2013, nameof(CreatePageLoaded)),
            "Create list page loaded by user {UserId}");

    private static readonly Action<ILogger, int, int, Exception?> ListNotFoundForUser =
        LoggerMessage.Define<int, int>(
            LogLevel.Warning,
            new EventId(2014, nameof(ListNotFoundForUser)),
            "List {ListId} not found for user {UserId}");

    public static void LogFailedToLoadTodoLists(ILogger logger, Exception ex)
        => FailedToLoadTodoLists(logger, ex);

    public static void LogFailedToLoadCreateTodoList(ILogger logger, Exception ex)
        => FailedToCreateTodoList(logger, ex);

    public static void LogListCreatedSuccessfully(ILogger logger, int listId, int userId)
        => ListCreatedSuccessfully(logger, listId, userId, null);

    public static void LogListLoadedForEdit(ILogger logger, int listId, int userId)
        => ListLoadedForEdit(logger, listId, userId, null);

    public static void LogListNotFoundForEdit(ILogger logger, int listId, int userId)
        => ListNotFoundForEdit(logger, listId, userId, null);

    public static void LogErrorLoadingEditPage(ILogger logger, int listId, Exception ex)
        => ErrorLoadingEditPage(logger, listId, ex);

    public static void LogListUpdatedSuccessfully(ILogger logger, int listId, int userId)
        => ListUpdatedSuccessfully(logger, listId, userId, null);

    public static void LogErrorUpdatingList(ILogger logger, int listId, Exception ex)
        => ErrorUpdatingList(logger, listId, ex);

    public static void LogListDeletedSuccessfully(ILogger logger, int listId, int userId)
        => ListDeletedSuccessfully(logger, listId, userId, null);

    public static void LogErrorDeletingList(ILogger logger, int listId, Exception ex)
        => ErrorDeletingList(logger, listId, ex);

    public static void LogListsRetrievedForUser(ILogger logger, int count, int userId)
        => ListsRetrievedForUser(logger, count, userId, null);

    public static void LogTasksRetrievedForList(ILogger logger, int count, int listId, int userId)
        => TasksRetrievedForList(logger, count, listId, userId, null);

    public static void LogCreatePageLoaded(ILogger logger, int userId)
        => CreatePageLoaded(logger, userId, null);

    public static void LogListNotFoundForUser(ILogger logger, int listId, int userId)
        => ListNotFoundForUser(logger, listId, userId, null);
}

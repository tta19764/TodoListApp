namespace TodoListApp.WebApi.CustomLogs;

/// <summary>
/// Custom logging class for TodoLists operations.
/// </summary>
internal static class TodoListsLog
{
    // Information level logs
    private static readonly Action<ILogger, int, int?, Exception?> ListDeletedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(1201, nameof(ListDeletedSuccessfully)),
            "List {ListId} deleted successfully by user {UserId}");

    private static readonly Action<ILogger, int, int?, Exception?> ListCreatedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(1202, nameof(ListCreatedSuccessfully)),
            "List {ListId} created successfully by user {UserId}");

    private static readonly Action<ILogger, int, int?, Exception?> ListUpdatedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(1203, nameof(ListUpdatedSuccessfully)),
            "List {ListId} updated successfully by user {UserId}");

    private static readonly Action<ILogger, int, int?, Exception?> ListRetrievedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(1204, nameof(ListRetrievedSuccessfully)),
            "List {ListId} retrieved successfully by user {UserId}");

    private static readonly Action<ILogger, int, int?, Exception?> ListsRetrievedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(1205, nameof(ListsRetrievedSuccessfully)),
            "Retrieved {Count} lists successfully for user {UserId}");

    private static readonly Action<ILogger, int, int, int, int?, Exception?> PaginatedListsRetrievedSuccessfully =
        LoggerMessage.Define<int, int, int, int?>(
            LogLevel.Information,
            new EventId(2206, nameof(PaginatedListsRetrievedSuccessfully)),
            "Retrieved {Count} paginated lists (page {PageNumber}, rows {RowCount}) successfully for user {UserId}");

    private static readonly Action<ILogger, int, int?, Exception?> UserListsCountRetrievedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(1207, nameof(UserListsCountRetrievedSuccessfully)),
            "{Count} user lists number retrieved successfully for user {UserId}");

    private static readonly Action<ILogger, int, int?, Exception?> AuthorListsCountRetrievedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(1208, nameof(AuthorListsCountRetrievedSuccessfully)),
            "{Count} author lists number retrieved successfully for user {UserId}");

    private static readonly Action<ILogger, int, int?, Exception?> SharedListsCountRetrievedSuccessfully =
        LoggerMessage.Define<int, int?>(
            LogLevel.Information,
            new EventId(1209, nameof(SharedListsCountRetrievedSuccessfully)),
            "{Count} chared lists number retrieved successfully for user {UserId}");

    // Warning level logs
    private static readonly Action<ILogger, int, int?, string, Exception?> ListNotFoundForUser =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Warning,
            new EventId(2203, nameof(ListNotFoundForUser)),
            "List with ID {ListId} not found for user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> UnauthorizedListAccess =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Warning,
            new EventId(2204, nameof(UnauthorizedListAccess)),
            "Unauthorized access attempt for list {ListId} by user {UserId}: {Message}");

    private static readonly Action<ILogger, int?, string, Exception?> UnauthorizedListsAccess =
        LoggerMessage.Define<int?, string>(
            LogLevel.Warning,
            new EventId(2205, nameof(UnauthorizedListsAccess)),
            "Unauthorized access attempt by user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> UnauthorizedListDeletion =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Warning,
            new EventId(2206, nameof(UnauthorizedListDeletion)),
            "Unauthorized deletion attempt for list {ListId} by user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> UnauthorizedListUpdate =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Warning,
            new EventId(2207, nameof(UnauthorizedListUpdate)),
            "Unauthorized update attempt for list {ListId} by user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int, Exception?> InvalidPaginationParameters =
        LoggerMessage.Define<int, int>(
            LogLevel.Warning,
            new EventId(2208, nameof(InvalidPaginationParameters)),
            "Invalid pagination parameters: pageNumber={PageNumber}, rowCount={RowCount}");

    private static readonly Action<ILogger, string, Exception?> InvalidListDataProvided =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2209, nameof(InvalidListDataProvided)),
            "Invalid list data provided: {Message}");

    private static readonly Action<ILogger, string, Exception?> ReferencedEntityNotFound =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2210, nameof(ReferencedEntityNotFound)),
            "Referenced entity not found during list creation: {Message}");

    private static readonly Action<ILogger, int?, Exception?> InvalidUserIdentifier =
        LoggerMessage.Define<int?>(
            LogLevel.Warning,
            new EventId(2211, nameof(InvalidUserIdentifier)),
            "Invalid user identifier for user {UserId}");

    // Error level logs
    private static readonly Action<ILogger, int, int?, string, Exception?> UnableToDeleteList =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Error,
            new EventId(3201, nameof(UnableToDeleteList)),
            "Unable to delete list {ListId} for user {UserId}: {Message}");

    private static readonly Action<ILogger, int, int?, string, Exception?> UnableToUpdateList =
        LoggerMessage.Define<int, int?, string>(
            LogLevel.Error,
            new EventId(3202, nameof(UnableToUpdateList)),
            "Unable to update list {ListId} for user {UserId}: {Message}");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorRetrievingList =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3203, nameof(UnexpectedErrorRetrievingList)),
            "Unexpected error occurred while retrieving list {ListId}");

    private static readonly Action<ILogger, int?, Exception?> UnexpectedErrorRetrievingLists =
        LoggerMessage.Define<int?>(
            LogLevel.Error,
            new EventId(3204, nameof(UnexpectedErrorRetrievingLists)),
            "Unexpected error occurred while retrieving lists for user {UserId}");

    private static readonly Action<ILogger, int?, Exception?> UnexpectedErrorRetrievingPaginatedLists =
        LoggerMessage.Define<int?>(
            LogLevel.Error,
            new EventId(3205, nameof(UnexpectedErrorRetrievingPaginatedLists)),
            "Unexpected error occurred while retrieving paginated lists for user {UserId}");

    private static readonly Action<ILogger, int?, Exception?> UnexpectedErrorRetrievingUserLists =
        LoggerMessage.Define<int?>(
            LogLevel.Error,
            new EventId(3206, nameof(UnexpectedErrorRetrievingUserLists)),
            "Unexpected error occurred while retrieving user lists for user {UserId}");

    private static readonly Action<ILogger, int?, Exception?> UnexpectedErrorRetrievingPaginatedUserLists =
        LoggerMessage.Define<int?>(
            LogLevel.Error,
            new EventId(3207, nameof(UnexpectedErrorRetrievingPaginatedUserLists)),
            "Unexpected error occurred while retrieving paginated user lists for user {UserId}");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorDeletingList =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3208, nameof(UnexpectedErrorDeletingList)),
            "Unexpected error occurred while deleting list {ListId}");

    private static readonly Action<ILogger, Exception?> UnexpectedErrorCreatingList =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3209, nameof(UnexpectedErrorCreatingList)),
            "Unexpected error occurred while creating list");

    private static readonly Action<ILogger, int, Exception?> UnexpectedErrorUpdatingList =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3210, nameof(UnexpectedErrorUpdatingList)),
            "Unexpected error occurred while updating list {ListId}");

    // Public methods for Warning level logs
    public static void LogListNotFoundForUser(ILogger logger, int listId, int? userId, string message, Exception? exception = null) =>
        ListNotFoundForUser(logger, listId, userId, message, exception);

    public static void LogUnauthorizedListAccess(ILogger logger, int listId, int? userId, string message, Exception? exception = null) =>
        UnauthorizedListAccess(logger, listId, userId, message, exception);

    public static void LogUnauthorizedListsAccess(ILogger logger, int? userId, string message, Exception? exception = null) =>
        UnauthorizedListsAccess(logger, userId, message, exception);

    public static void LogUnauthorizedListDeletion(ILogger logger, int listId, int? userId, string message, Exception? exception = null) =>
        UnauthorizedListDeletion(logger, listId, userId, message, exception);

    public static void LogUnauthorizedListUpdate(ILogger logger, int listId, int? userId, string message, Exception? exception = null) =>
        UnauthorizedListUpdate(logger, listId, userId, message, exception);

    public static void LogInvalidPaginationParameters(ILogger logger, int pageNumber, int rowCount, Exception? exception = null) =>
        InvalidPaginationParameters(logger, pageNumber, rowCount, exception);

    public static void LogInvalidListDataProvided(ILogger logger, string message, Exception? exception = null) =>
        InvalidListDataProvided(logger, message, exception);

    public static void LogReferencedEntityNotFound(ILogger logger, string message, Exception? exception = null) =>
        ReferencedEntityNotFound(logger, message, exception);

    public static void LogInvalidUserIdentifier(ILogger logger, int? userId, Exception? exception = null) =>
        InvalidUserIdentifier(logger, userId, exception);

    public static void LogUserListsCountRetrievedSuccessfully(ILogger logger, int count, int? userId) =>
        UserListsCountRetrievedSuccessfully(logger, count, userId, null);

    public static void LogAuthorListsCountRetrievedSuccessfully(ILogger logger, int count, int? userId) =>
        AuthorListsCountRetrievedSuccessfully(logger, count, userId, null);

    public static void LogSharedListsCountRetrievedSuccessfully(ILogger logger, int count, int? userId) =>
        SharedListsCountRetrievedSuccessfully(logger, count, userId, null);

    // Public methods for Error level logs
    public static void LogUnableToDeleteList(ILogger logger, int listId, int? userId, string message, Exception? exception = null) =>
        UnableToDeleteList(logger, listId, userId, message, exception);

    public static void LogUnableToUpdateList(ILogger logger, int listId, int? userId, string message, Exception? exception = null) =>
        UnableToUpdateList(logger, listId, userId, message, exception);

    public static void LogUnexpectedErrorRetrievingList(ILogger logger, int listId, Exception exception) =>
        UnexpectedErrorRetrievingList(logger, listId, exception);

    public static void LogUnexpectedErrorRetrievingLists(ILogger logger, int? userId, Exception exception) =>
        UnexpectedErrorRetrievingLists(logger, userId, exception);

    public static void LogUnexpectedErrorRetrievingPaginatedLists(ILogger logger, int? userId, Exception exception) =>
        UnexpectedErrorRetrievingPaginatedLists(logger, userId, exception);

    public static void LogUnexpectedErrorRetrievingUserLists(ILogger logger, int? userId, Exception exception) =>
        UnexpectedErrorRetrievingUserLists(logger, userId, exception);

    public static void LogUnexpectedErrorRetrievingPaginatedUserLists(ILogger logger, int? userId, Exception exception) =>
        UnexpectedErrorRetrievingPaginatedUserLists(logger, userId, exception);

    public static void LogUnexpectedErrorDeletingList(ILogger logger, int listId, Exception exception) =>
        UnexpectedErrorDeletingList(logger, listId, exception);

    public static void LogUnexpectedErrorCreatingList(ILogger logger, Exception exception) =>
        UnexpectedErrorCreatingList(logger, exception);

    public static void LogUnexpectedErrorUpdatingList(ILogger logger, int listId, Exception exception) =>
        UnexpectedErrorUpdatingList(logger, listId, exception);

    // Public methods for Information level logs
    public static void LogListDeletedSuccessfully(ILogger logger, int listId, int? userId) =>
        ListDeletedSuccessfully(logger, listId, userId, null);

    public static void LogListCreatedSuccessfully(ILogger logger, int listId, int? userId) =>
        ListCreatedSuccessfully(logger, listId, userId, null);

    public static void LogListUpdatedSuccessfully(ILogger logger, int listId, int? userId) =>
        ListUpdatedSuccessfully(logger, listId, userId, null);

    public static void LogListRetrievedSuccessfully(ILogger logger, int listId, int? userId) =>
        ListRetrievedSuccessfully(logger, listId, userId, null);

    public static void LogListsRetrievedSuccessfully(ILogger logger, int count, int? userId) =>
        ListsRetrievedSuccessfully(logger, count, userId, null);

    public static void LogPaginatedListsRetrievedSuccessfully(ILogger logger, int count, int pageNumber, int rowCount, int? userId) =>
        PaginatedListsRetrievedSuccessfully(logger, count, pageNumber, rowCount, userId, null);
}

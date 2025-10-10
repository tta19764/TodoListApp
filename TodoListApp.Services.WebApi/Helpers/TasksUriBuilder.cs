using System.Globalization;
using TodoListApp.Services.Enums;

namespace TodoListApp.Services.WebApi.Helpers;

/// <summary>
/// Helper class for building URIs for task-related operations.
/// </summary>
internal static class TasksUriBuilder
{
    /// <summary>
    /// Builds a URI with sorting and filtering parameters.
    /// </summary>
    /// <param name="baseUri">The base URI.</param>
    /// <param name="filter">The ststus filter.</param>
    /// <param name="sortBy">The sorting property.</param>
    /// <param name="sortOrder">The sorting direction.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks on the page.</param>
    /// <returns>The constructed URI.</returns>
    public static Uri BuildSortingUri(Uri baseUri, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null)
    {
        Uri uri;
        if (pageNumber.HasValue && rowCount.HasValue)
        {
            uri = new Uri(baseUri + $"{pageNumber.Value}/{rowCount.Value}", UriKind.Absolute);
        }
        else
        {
            uri = baseUri;
        }

        var builder = new UriBuilder(uri);
        var query = System.Web.HttpUtility.ParseQueryString(builder.Query);
        query["filter"] = filter.ToString();
        if (sortBy != null)
        {
            query["sortBy"] = sortBy;
        }

        if (sortOrder != null)
        {
            query["sortOrder"] = sortOrder;
        }

        builder.Query = query.ToString();

        return builder.Uri;
    }

    /// <summary>
    /// Builds a URI with search parameters.
    /// </summary>
    /// <param name="baseUri">The base URI.</param>
    /// <param name="title">The task title.</param>
    /// <param name="creationDate">The task creation date.</param>
    /// <param name="dueDate">The task due date.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks on the page.</param>
    /// <returns>The constructed URI.</returns>
    public static Uri BuildSearchUri(Uri baseUri, string? title, DateTime? creationDate, DateTime? dueDate, int? pageNumber = null, int? rowCount = null)
    {
        Uri uri;
        if (pageNumber.HasValue && rowCount.HasValue)
        {
            uri = new Uri(baseUri + $"{pageNumber.Value}/{rowCount.Value}", UriKind.Absolute);
        }
        else
        {
            uri = baseUri;
        }

        var builder = new UriBuilder(uri);
        var query = System.Web.HttpUtility.ParseQueryString(builder.Query);
        if (title != null)
        {
            query["title"] = title;
        }

        if (creationDate != null)
        {
            query["creationDate"] = creationDate.Value.ToString("o", CultureInfo.InvariantCulture);
        }

        if (dueDate != null)
        {
            query["dueDate"] = dueDate.Value.ToString("o", CultureInfo.InvariantCulture);
        }

        builder.Query = query.ToString();

        return builder.Uri;
    }
}

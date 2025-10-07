using Microsoft.VisualBasic;
using System.Globalization;
using TodoListApp.Services.Enums;

namespace TodoListApp.Services.WebApi.Helpers;
internal static class TasksUriBuilder
{
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

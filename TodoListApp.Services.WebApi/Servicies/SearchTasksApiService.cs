using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.Services.WebApi.CustomLogs;
using TodoListApp.Services.WebApi.Helpers;
using TodoListApp.WebApi.Models.Dtos.Read;

namespace TodoListApp.Services.WebApi.Servicies;

/// <summary>
/// Provides API-based task search data retrieval.
/// </summary>
public class SearchTasksApiService : ISearchTasksService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<SearchTasksApiService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTasksApiService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to send API requests.</param>
    /// <param name="logger">The logger instance for structured logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClient"/> or <paramref name="logger"/> is null.</exception>
    public SearchTasksApiService(HttpClient httpClient, ILogger<SearchTasksApiService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves the count of tasks that match the specified search criteria.
    /// </summary>
    /// <param name="userId">The ID of the user performing the search.</param>
    /// <param name="title">Optional task title to search for (partial match).</param>
    /// <param name="creationDate">Optional creation date to filter by.</param>
    /// <param name="dueDate">Optional due date to filter by.</param>
    /// <returns>The number of tasks matching the search criteria.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task<int> GetAllSearchCountAsync(int userId, string? title = null, DateTime? creationDate = null, DateTime? dueDate = null)
    {
        try
        {
            var uri = new Uri($"{this.httpClient.BaseAddress}Count", UriKind.Absolute);
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

            using var response = await this.httpClient.GetAsync(builder.Uri);
            if (response.IsSuccessStatusCode)
            {
                var countString = await response.Content.ReadAsStringAsync();
                if (int.TryParse(countString, out int count))
                {
                    return count;
                }

                TodoListLog.LogNullResponse(this.logger, "get all by serach count");
                return 0;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "get all by serach count", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve todo tasks by serach count: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoTaskLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TodoTaskLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Searches for tasks based on specified criteria with optional pagination.
    /// </summary>
    /// <param name="userId">The ID of the user performing the search.</param>
    /// <param name="title">Optional task title to search for (partial match).</param>
    /// <param name="creationDate">Optional creation date to filter by.</param>
    /// <param name="dueDate">Optional due date to filter by.</param>
    /// <param name="pageNumber">Optional page number for pagination (1-based).</param>
    /// <param name="rowCount">Optional number of tasks per page.</param>
    /// <returns>A read-only list of <see cref="TodoTaskModel"/> representing tasks matching the search criteria.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no tasks are found matching the criteria.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public async Task<IReadOnlyList<TodoTaskModel>> SearchTasksAsync(int userId, string? title = null, DateTime? creationDate = null, DateTime? dueDate = null, int? pageNumber = null, int? rowCount = null)
    {
        try
        {
            Uri uri = TasksUriBuilder.BuildSearchUri(this.httpClient.BaseAddress!, title, creationDate, dueDate, pageNumber, rowCount);

            using var response = await this.httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var tasks = await response.Content.ReadFromJsonAsync<List<TodoTaskDto>>();

                if (tasks != null)
                {
                    if (pageNumber != null && rowCount != null)
                    {
                        TodoTaskLog.LogTodoTasksPageRetrievedBySearch(this.logger, tasks.Count, pageNumber.Value, userId);
                    }
                    else
                    {
                        TodoTaskLog.LogTodoTasksRetrievedBySearch(this.logger, tasks.Count, userId);
                    }

                    return tasks.Select(t => MapToModel.MapToTodoTaskModel(t)).ToList();
                }

                TodoTaskLog.LogNullResponse(this.logger, "get all by search");
                throw new InvalidOperationException($"Serach todo tasks for user {userId} were not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "get all by search", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve serach todo tasks for user: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoTaskLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (JsonException ex)
        {
            TodoTaskLog.LogJsonParsingError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TodoTaskLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }
}

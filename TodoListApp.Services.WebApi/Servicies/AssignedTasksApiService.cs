using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TodoListApp.Services.Enums;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.Services.WebApi.CustomLogs;
using TodoListApp.Services.WebApi.Helpers;
using TodoListApp.WebApi.Models.Dtos.Read;

namespace TodoListApp.Services.WebApi.Servicies;
public class AssignedTasksApiService : IAssignedTasksService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<AssignedTasksApiService> logger;

    public AssignedTasksApiService(HttpClient httpClient, ILogger<AssignedTasksApiService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IReadOnlyList<TodoTaskModel>> GetAllAssignedAsync(int userId, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null)
    {
        try
        {
            Uri uri = TasksUriBuilder.BuildSortingUri(this.httpClient.BaseAddress!, filter, sortBy, sortOrder, pageNumber, rowCount);

            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var tasks = await response.Content.ReadFromJsonAsync<List<TodoTaskDto>>();
                if (tasks != null)
                {
                    if (pageNumber != null && rowCount != null)
                    {
                        TodoTaskLog.LogTodoTasksPageRetrievedByOwner(this.logger, tasks.Count, pageNumber.Value, userId);
                    }
                    else
                    {
                        TodoTaskLog.LogTodoTasksRetrievedByOwner(this.logger, tasks.Count, userId);
                    }

                    return tasks.Select(t => MapToModel.MapToTodoTaskModel(t)).ToList();
                }

                TodoTaskLog.LogNullResponse(this.logger, "get all by assigne");
                throw new InvalidOperationException($"Todo tasks for user {userId} were not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "get all by assigne", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve todo tasks for assigne: {response.StatusCode}");
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

    public async Task<int> GetAllAssignedCountAsync(int userId, TaskFilter filter = TaskFilter.Active)
    {
        try
        {
            var uri = new Uri($"{this.httpClient.BaseAddress}Count", UriKind.Absolute);
            var builder = new UriBuilder(uri);
            var query = System.Web.HttpUtility.ParseQueryString(builder.Query);
            query["filter"] = filter.ToString();

            builder.Query = query.ToString();
            using var response = await this.httpClient.GetAsync(builder.Uri);
            if (response.IsSuccessStatusCode)
            {
                var countString = await response.Content.ReadAsStringAsync();
                if (int.TryParse(countString, out int count))
                {
                    return count;
                }

                TodoListLog.LogNullResponse(this.logger, "get all by user assigned count");
                return 0;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "get all by user assigned count", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve todo tasks by user assigned count: {response.StatusCode}");
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
}

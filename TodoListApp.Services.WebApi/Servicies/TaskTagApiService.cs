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
/// Provides API-based CRUD operations and data retrieval for task tags.
/// </summary>
public class TaskTagApiService : ITaskTagService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<TaskTagApiService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskTagApiService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to send API requests.</param>
    /// <param name="logger">The logger instance for structured logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClient"/> or <paramref name="logger"/> is null.</exception>
    public TaskTagApiService(HttpClient httpClient, ILogger<TaskTagApiService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Adds a tag to a specific task.
    /// </summary>
    /// <param name="userId">The ID of the user performing the operation.</param>
    /// <param name="taskId">The ID of the task to add the tag to.</param>
    /// <param name="tagId">The ID of the tag to add.</param>
    /// <returns>A <see cref="TodoTaskModel"/> representing the updated task with the new tag.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the task or tag is not found.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public async Task<TodoTaskModel> AddTaskTag(int userId, int taskId, int tagId)
    {
        try
        {
            var uri = new Uri($"Tasks/{taskId}/Tags/{tagId}", UriKind.Relative);
            using var response = await this.httpClient.PostAsync(uri, null);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TodoTaskDto>();
                if (result != null)
                {
                    TaskTagLog.LogTagAddedToTask(this.logger, tagId, taskId);
                    return MapToModel.MapToTodoTaskModel(result);
                }

                TaskTagLog.LogNullResponse(this.logger, "add tag to task");
                throw new InvalidOperationException($"Failed to add tag {tagId} to task {taskId} - null response");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException($"Task with ID {taskId} or Tag with ID {tagId} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TaskTagLog.LogOperationFailed(this.logger, "add tag to task", taskId, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to add tag to task: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TaskTagLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (JsonException ex)
        {
            TaskTagLog.LogJsonParsingError(this.logger, ex);
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            TaskTagLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all tasks for a user that are tagged with a specific tag.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="tagId">The ID of the tag to filter by.</param>
    /// <param name="pageNumber">Optional page number for pagination (1-based).</param>
    /// <param name="rowCount">Optional number of tasks per page.</param>
    /// <returns>A read-only list of <see cref="TodoTaskModel"/> representing tasks with the specified tag.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public async Task<IReadOnlyList<TodoTaskModel>> GetAllUserTagTasksAsync(int userId, int tagId, int? pageNumber = null, int? rowCount = null)
    {
        try
        {
            Uri uri;
            if (pageNumber.HasValue && rowCount.HasValue)
            {
                uri = new Uri($"{tagId}/Tasks/{pageNumber.Value}/{rowCount.Value}", UriKind.Relative);
            }
            else
            {
                uri = new Uri($"{tagId}/Tasks", UriKind.Relative);
            }

            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var tasks = await response.Content.ReadFromJsonAsync<List<TodoTaskDto>>();
                if (tasks != null)
                {
                    TaskTagLog.LogTaggedTasksRetrieved(this.logger, tasks.Count, tagId, userId);
                    return tasks.Select(MapToModel.MapToTodoTaskModel).ToList();
                }

                TaskTagLog.LogNullResponse(this.logger, "get user tagged tasks");
                return new List<TodoTaskModel>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TaskTagLog.LogOperationFailed(this.logger, "user tagged tasks", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve user tagged tasks: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TagLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (JsonException ex)
        {
            TagLog.LogJsonParsingError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all unique tags that are associated with any of the user's tasks.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="pageNumber">Optional page number for pagination (1-based).</param>
    /// <param name="rowCount">Optional number of tags per page.</param>
    /// <returns>A read-only list of <see cref="TagModel"/> representing all tags used in the user's tasks.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public async Task<IReadOnlyList<TagModel>> GetAllUserTaskTagsAsync(int userId, int? pageNumber = null, int? rowCount = null)
    {
        try
        {
            Uri uri;
            if (pageNumber.HasValue && rowCount.HasValue)
            {
                uri = new Uri($"{pageNumber.Value}/{rowCount.Value}", UriKind.Relative);
            }
            else
            {
                uri = this.httpClient.BaseAddress!;
            }

            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var tags = await response.Content.ReadFromJsonAsync<List<TagDto>>();
                if (tags != null)
                {
                    TaskTagLog.LogUserTaskTagsRetrieved(this.logger, tags.Count, userId);
                    return tags.Select(MapToModel.MapToTagModel).ToList();
                }

                TaskTagLog.LogNullResponse(this.logger, "get user task tags");
                return new List<TagModel>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TaskTagLog.LogOperationFailed(this.logger, "user task tags", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve user task tags: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TaskTagLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (JsonException ex)
        {
            TaskTagLog.LogJsonParsingError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TaskTagLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves the count of unique tags associated with the user's tasks.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The number of unique tags used in the user's tasks.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task<int> GetAllUserTaskTagsCount(int userId)
    {
        try
        {
            var uri = new Uri("Count", UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var countString = await response.Content.ReadAsStringAsync();
                if (int.TryParse(countString, out int count))
                {
                    return count;
                }

                TagLog.LogNullResponse(this.logger, "get user task tags count");
                return 0;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TaskTagLog.LogOperationFailed(this.logger, "get user task tags count", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve user task tags count: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TaskTagLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TaskTagLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves the count of tasks that are tagged with a specific tag for a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>The number of tasks tagged with the specified tag.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task<int> GetTagTasksCount(int userId, int tagId)
    {
        try
        {
            var uri = new Uri($"{tagId}/Tasks/Count", UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var countString = await response.Content.ReadAsStringAsync();
                if (int.TryParse(countString, out int count))
                {
                    return count;
                }

                TagLog.LogNullResponse(this.logger, "get tag tasks count");
                return 0;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TaskTagLog.LogOperationFailed(this.logger, "get tag tasks count", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve tag tasks count: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TaskTagLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TaskTagLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Removes a tag from a specific task.
    /// </summary>
    /// <param name="userId">The ID of the user performing the operation.</param>
    /// <param name="taskId">The ID of the task to remove the tag from.</param>
    /// <param name="tagId">The ID of the tag to remove.</param>
    /// <returns>A <see cref="TodoTaskModel"/> representing the updated task without the removed tag.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the task or tag is not found.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public async Task<TodoTaskModel> RemoveTaskTag(int userId, int taskId, int tagId)
    {
        try
        {
            var uri = new Uri($"Tasks/{taskId}/Tags/{tagId}", UriKind.Relative);
            using var response = await this.httpClient.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TodoTaskDto>();
                if (result != null)
                {
                    TaskTagLog.LogTagRemovedFromTask(this.logger, tagId, taskId);
                    return MapToModel.MapToTodoTaskModel(result);
                }

                TaskTagLog.LogNullResponse(this.logger, "remove tag from task");
                throw new InvalidOperationException($"Failed to remove tag {tagId} from task {taskId} - null response");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException($"Task with ID {taskId} or Tag with ID {tagId} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TaskTagLog.LogOperationFailed(this.logger, "remove tag from task", taskId, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to remove tag from task: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TaskTagLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (JsonException ex)
        {
            TaskTagLog.LogJsonParsingError(this.logger, ex);
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            TaskTagLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }
}

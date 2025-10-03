using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TodoListApp.Services.Enums;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.Services.WebApi.CustomLogs;
using TodoListApp.WebApi.Models.Dtos.Create;
using TodoListApp.WebApi.Models.Dtos.Read;
using TodoListApp.WebApi.Models.Dtos.Update;

namespace TodoListApp.Services.WebApi.Servicies;
public class TodoTaskApiService : ITodoTaskService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<TodoTaskApiService> logger;

    public TodoTaskApiService(HttpClient httpClient, ILogger<TodoTaskApiService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TodoTaskModel> AddAsync(TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        try
        {
            var dto = new CreateTodoTaskDto(
                model.Title,
                model.Description,
                model.CreationDate,
                model.DueDate,
                model.StatusId,
                model.OwnerUserId,
                model.ListId);

            using var response = await this.httpClient.PutAsJsonAsync(this.httpClient.BaseAddress, dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TodoTaskDto>();
                if (result != null)
                {
                    TodoTaskLog.LogTaskCreated(this.logger, result.Id);
                    return MapToModel(result);
                }

                TodoTaskLog.LogNullResponse(this.logger, "create");
                throw new InvalidOperationException("Failed to create todo task - null response");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "create", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to create todo task: {response.StatusCode}");
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

    public async Task DeleteAsync(int userId, int id)
    {
        try
        {
            var uri = new Uri(id.ToString(CultureInfo.InvariantCulture), UriKind.Relative);
            using var response = await this.httpClient.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                TodoTaskLog.LogTaskDeleted(this.logger, id);
                return;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "delete", id, (int)response.StatusCode, errorContent);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException($"Todo task with ID {id} was not found");
            }

            throw new HttpRequestException($"Failed to delete todo task: {response.StatusCode}");
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

    public async Task<IReadOnlyList<TodoTaskModel>> GetAllAsync(int userId)
    {
        try
        {
            var uri = new Uri(userId.ToString(CultureInfo.InvariantCulture), UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var tasks = await response.Content.ReadFromJsonAsync<List<TodoTaskDto>>();
                if (tasks != null)
                {
                    TodoTaskLog.LogTodoTasksRetrievedByUser(this.logger, tasks.Count, userId);
                    return tasks.Select(t => MapToModel(t)).ToList();
                }

                TodoTaskLog.LogNullResponse(this.logger, "get all by user");
                throw new InvalidOperationException($"Todo tasks for user {userId} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "get all by user", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve user's todo tasks: {response.StatusCode}");
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

    public async Task<IReadOnlyList<TodoTaskModel>> GetAllAsync(int userId, int pageNumber, int rowCount)
    {
        try
        {
            var uri = new Uri($"{userId.ToString(CultureInfo.InvariantCulture)}/{pageNumber}/{rowCount}", UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var tasks = await response.Content.ReadFromJsonAsync<List<TodoTaskDto>>();
                if (tasks != null)
                {
                    TodoTaskLog.LogTodoTasksPageRetrievedByUser(this.logger, tasks.Count, pageNumber, userId);
                    return tasks.Select(t => MapToModel(t)).ToList();
                }

                TodoTaskLog.LogNullResponse(this.logger, "get all by user");
                throw new InvalidOperationException($"Todo tasks for user {userId} were not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "get all by user", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve user's todo tasks: {response.StatusCode}");
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

    public async Task<IReadOnlyList<TodoTaskModel>> GetAllByAuthorAsync(int userId, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null)
    {
        try
        {
            Uri baseUri = new Uri(this.httpClient.BaseAddress + "Assigned/");
            Uri uri = BuildUri(baseUri, filter, sortBy, sortOrder, pageNumber, rowCount);

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

                    return tasks.Select(t => MapToModel(t)).ToList();
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

    public async Task<IReadOnlyList<TodoTaskModel>> GetAllByListIdAsync(int id, int userId, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null)
    {
        try
        {
            Uri baseUri = new Uri(this.httpClient.BaseAddress + $"Lists/{id}/");
            Uri uri = BuildUri(baseUri, filter, sortBy, sortOrder, pageNumber, rowCount);

            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var tasks = await response.Content.ReadFromJsonAsync<List<TodoTaskDto>>();
                if (tasks != null)
                {
                    if (pageNumber != null && rowCount != null)
                    {
                        TodoTaskLog.LogTodoTasksPageRetrievedByListId(this.logger, tasks.Count, pageNumber.Value, id, userId);
                    }
                    else
                    {
                        TodoTaskLog.LogTodoTasksRetrievedByListId(this.logger, tasks.Count, id, userId);
                    }

                    return tasks.Select(t => MapToModel(t)).ToList();
                }

                TodoTaskLog.LogNullResponse(this.logger, "get all by list");
                throw new InvalidOperationException($"Todo tasks for list {userId} were not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "get all by list", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve todo tasks for list: {response.StatusCode}");
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

    public async Task<TodoTaskModel> GetByIdAsync(int userId, int id)
    {
        try
        {
            var uri = new Uri(id.ToString(CultureInfo.InvariantCulture), UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var task = await response.Content.ReadFromJsonAsync<TodoTaskDto>();
                if (task != null)
                {
                    TodoTaskLog.LogTaskRetrieved(this.logger, id, userId);
                    return MapToModel(task);
                }

                TodoTaskLog.LogNullResponse(this.logger, "get by id");
                throw new InvalidOperationException($"Todo task with ID {id} was not found");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TodoTaskLog.LogTaskNotFound(this.logger, id);
                throw new InvalidOperationException($"Todo task with ID {id} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "get by id", id, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve todo task: {response.StatusCode}");
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
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            TodoTaskLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    public async Task<TodoTaskModel> UpdateAsync(int userId, TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        try
        {
            var dto = new UpdateTodoTaskDto(
                model.Id,
                model.Title,
                model.Description,
                model.DueDate,
                model.StatusId,
                model.OwnerUserId,
                model.ListId);

            using var response = await this.httpClient.PostAsJsonAsync(this.httpClient.BaseAddress, dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TodoTaskDto>();
                if (result != null)
                {
                    TodoTaskLog.LogTaskUpdated(this.logger, model.Id);
                    return MapToModel(result);
                }

                TodoTaskLog.LogNullResponse(this.logger, "update");
                throw new InvalidOperationException($"Failed to update todo task {model.Id} - null response");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TodoTaskLog.LogTaskNotFound(this.logger, model.Id);
                throw new InvalidOperationException($"Todo task with ID {model.Id} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "update", model.Id, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to update todo task: {response.StatusCode}");
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
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            TodoTaskLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    public async Task<TodoTaskModel> UpdateTaskStatusAsync(int userId, int taskId, int statusId)
    {
        try
        {
            var dto = new UpdateStatusDto(
                statusId,
                taskId);

            using var content = JsonContent.Create(dto);
            using var response = await this.httpClient.PatchAsync(this.httpClient.BaseAddress, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TodoTaskDto>();
                if (result != null)
                {
                    TodoTaskLog.LogTaskUpdated(this.logger, taskId);
                    return MapToModel(result);
                }

                TodoTaskLog.LogNullResponse(this.logger, "update");
                throw new InvalidOperationException($"Failed to update todo task status {taskId} - null response");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TodoTaskLog.LogTaskNotFound(this.logger, taskId);
                throw new InvalidOperationException($"Todo task with ID {taskId} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "update status", taskId, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to update todo task status: {response.StatusCode}");
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
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            TodoTaskLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    private static TodoTaskModel MapToModel(TodoTaskDto dto)
    {
        return new TodoTaskModel(
            id: dto.Id,
            title: dto.Title,
            description: dto.Description ?? string.Empty,
            creationDate: dto.CreationDate,
            dueDate: dto.DueDate,
            statusId: 0,
            ownerUserId: dto.AssigneeId,
            listId: dto.ListId,
            owner: new UserModel(dto.AssigneeId, dto.AssigneeFirstName, dto.AssigneeLastName),
            status: new StatusModel(0, dto.Status));
    }

    private static Uri BuildUri(Uri baseUri, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null)
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
}

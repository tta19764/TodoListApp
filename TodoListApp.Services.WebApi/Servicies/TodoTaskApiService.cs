using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TodoListApp.Services.Enums;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.Services.WebApi.CustomLogs;
using TodoListApp.Services.WebApi.Helpers;
using TodoListApp.WebApi.Models.Dtos.Create;
using TodoListApp.WebApi.Models.Dtos.Read;
using TodoListApp.WebApi.Models.Dtos.Update;

namespace TodoListApp.Services.WebApi.Servicies;

/// <summary>
/// Provides API-based CRUD and status management operations for to-do tasks.
/// </summary>
public class TodoTaskApiService : ITodoTaskService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<TodoTaskApiService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTaskApiService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to communicate with the to-do task API.</param>
    /// <param name="logger">The logger instance used for structured logging of task operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClient"/> or <paramref name="logger"/> is null.</exception>
    public TodoTaskApiService(HttpClient httpClient, ILogger<TodoTaskApiService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new to-do task using the remote API.
    /// </summary>
    /// <param name="model">The to-do task model to create.</param>
    /// <returns>The created <see cref="TodoTaskModel"/> returned by the API.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
    /// <exception cref="HttpRequestException">Thrown when the API request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the API returns a null response.</exception>
    /// <exception cref="JsonException">Thrown when the API response cannot be parsed.</exception>
    public Task<TodoTaskModel> AddAsync(TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return this.AddInternalAsync(model);
    }

    /// <summary>
    /// Deletes a to-do task by its identifier.
    /// </summary>
    /// <param name="userId">The ID of the user performing the operation.</param>
    /// <param name="id">The ID of the task to delete.</param>
    /// <exception cref="HttpRequestException">Thrown when the delete operation fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the task does not exist.</exception>
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

    /// <summary>
    /// Retrieves all to-do tasks for the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user whose tasks to retrieve.</param>
    /// <returns>A read-only list of <see cref="TodoTaskModel"/> objects.</returns>
    /// <exception cref="HttpRequestException">Thrown when the API request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no tasks are found or the response is null.</exception>
    /// <exception cref="JsonException">Thrown when the response cannot be deserialized.</exception>
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
                    return tasks.Select(t => MapToModel.MapToTodoTaskModel(t)).ToList();
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

    /// <summary>
    /// Retrieves a paged collection of to-do tasks for the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user whose tasks to retrieve.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="rowCount">The number of rows per page.</param>
    /// <returns>A read-only list of <see cref="TodoTaskModel"/> objects corresponding to the requested page.</returns>
    /// <exception cref="HttpRequestException">Thrown when the API request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no tasks are found or the response is null.</exception>
    /// <exception cref="JsonException">Thrown when the response cannot be deserialized.</exception>
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
                    return tasks.Select(t => MapToModel.MapToTodoTaskModel(t)).ToList();
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

    /// <summary>
    /// Retrieves the count of tasks in a specific list based on the specified filter.
    /// </summary>
    /// <param name="id">The ID of the list.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="filter">The task status filter to apply (default is Active).</param>
    /// <returns>The number of tasks in the list matching the filter criteria.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task<int> GetAllByListIdCountAsync(int id, int userId, TaskFilter filter = TaskFilter.Active)
    {
        try
        {
            var uri = new Uri($"{this.httpClient.BaseAddress}Lists/{id}/Count", UriKind.Absolute);
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

                TodoListLog.LogNullResponse(this.logger, "get all by list id count");
                return 0;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "get all by list id count", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve todo tasks by list id count: {response.StatusCode}");
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
    /// Retrieves all tasks from a specific list with filtering, sorting, and optional pagination.
    /// </summary>
    /// <param name="id">The ID of the list.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="filter">The task status filter to apply (default is Active).</param>
    /// <param name="sortBy">The property to sort by (default is "DueDate").</param>
    /// <param name="sortOrder">The sort order - "asc" for ascending or "desc" for descending (default is "asc").</param>
    /// <param name="pageNumber">Optional page number for pagination (1-based).</param>
    /// <param name="rowCount">Optional number of tasks per page.</param>
    /// <returns>A read-only list of <see cref="TodoTaskModel"/> representing tasks in the list.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no tasks are found.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public async Task<IReadOnlyList<TodoTaskModel>> GetAllByListIdAsync(int id, int userId, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null)
    {
        try
        {
            Uri baseUri = new Uri(this.httpClient.BaseAddress + $"Lists/{id}/");
            Uri uri = TasksUriBuilder.BuildSortingUri(baseUri, filter, sortBy, sortOrder, pageNumber, rowCount);

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

                    return tasks.Select(t => MapToModel.MapToTodoTaskModel(t)).ToList();
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

    /// <summary>
    /// Retrieves a specific to-do task by its identifier.
    /// </summary>
    /// <param name="userId">The ID of the user performing the request.</param>
    /// <param name="id">The ID of the to-do task to retrieve.</param>
    /// <returns>The matching <see cref="TodoTaskModel"/> if found.</returns>
    /// <exception cref="HttpRequestException">Thrown when the API request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the task does not exist or returns null.</exception>
    /// <exception cref="JsonException">Thrown when the response cannot be parsed.</exception>
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
                    return MapToModel.MapToTodoTaskModel(task);
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

    /// <summary>
    /// Updates an existing to-do task via the API.
    /// </summary>
    /// <param name="userId">The ID of the user performing the update.</param>
    /// <param name="model">The updated task model.</param>
    /// <returns>The updated <see cref="TodoTaskModel"/> returned by the API.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
    /// <exception cref="HttpRequestException">Thrown when the update request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the task is not found or the response is null.</exception>
    /// <exception cref="JsonException">Thrown when the response cannot be deserialized.</exception>
    public Task<TodoTaskModel> UpdateAsync(int userId, TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return this.UpdateIntrenalAsync(model);
    }

    /// <summary>
    /// Updates the status of a specific to-do task.
    /// </summary>
    /// <param name="userId">The ID of the user performing the update.</param>
    /// <param name="taskId">The ID of the task to update.</param>
    /// <param name="statusId">The new status identifier to set.</param>
    /// <returns>The updated <see cref="TodoTaskModel"/> with the new status.</returns>
    /// <exception cref="HttpRequestException">Thrown when the request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the task does not exist or the response is null.</exception>
    /// <exception cref="JsonException">Thrown when the response cannot be parsed.</exception>
    public async Task<TodoTaskModel> UpdateTaskStatusAsync(int userId, int taskId, int statusId)
    {
        try
        {
            var dto = new UpdateStatusDto()
            {
                StatusId = statusId,
                TaskId = taskId,
            };

            using var content = JsonContent.Create(dto);
            using var response = await this.httpClient.PatchAsync(this.httpClient.BaseAddress, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TodoTaskDto>();
                if (result != null)
                {
                    TodoTaskLog.LogTaskUpdated(this.logger, taskId);
                    return MapToModel.MapToTodoTaskModel(result);
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

    /// <summary>
    /// Retrieves the count of comments for a specific task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="userId">The ID of the user requesting the count.</param>
    /// <returns>The number of comments associated with the task.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task<int> GetTaskCommentsCountAsync(int taskId, int userId)
    {
        try
        {
            var uri = new Uri($"{taskId}/Comments/Count", UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var countString = await response.Content.ReadAsStringAsync();
                if (int.TryParse(countString, out int count))
                {
                    TodoTaskLog.LogCommentCountRetrieved(this.logger, count, taskId);
                    return count;
                }

                TodoTaskLog.LogNullResponse(this.logger, "get comment count");
                return 0;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "get comment count", taskId, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve comment count for task {taskId}: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoTaskLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TodoTaskLog.LogUnexpectedErrorRetrievingComments(this.logger, taskId, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all comments for a specific task with optional pagination.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="userId">The ID of the user requesting the comments.</param>
    /// <param name="pageNumber">Optional page number for pagination (1-based).</param>
    /// <param name="rowCount">Optional number of comments per page.</param>
    /// <returns>A read-only list of <see cref="CommentModel"/> representing the task's comments.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public async Task<IReadOnlyList<CommentModel>> GetTaskCommentsAsync(int taskId, int userId, int? pageNumber = null, int? rowCount = null)
    {
        try
        {
            Uri uri;
            if (pageNumber.HasValue && rowCount.HasValue)
            {
                uri = new Uri($"{taskId}/Comments/{pageNumber.Value}/{rowCount.Value}", UriKind.Relative);
            }
            else
            {
                uri = new Uri($"{taskId}/Comments", UriKind.Relative);
            }

            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var comments = await response.Content.ReadFromJsonAsync<List<CommentDto>>();
                if (comments != null)
                {
                    if (pageNumber.HasValue && rowCount.HasValue)
                    {
                        TodoTaskLog.LogCommentsPageRetrievedForTask(this.logger, comments.Count, pageNumber.Value, taskId, userId);
                    }
                    else
                    {
                        TodoTaskLog.LogCommentsRetrievedForTask(this.logger, comments.Count, taskId, userId);
                    }

                    return comments.Select(MapToModel.MapToCommentModel).ToList();
                }

                TodoTaskLog.LogNullResponse(this.logger, "get comments");
                return new List<CommentModel>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "get comments", taskId, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve comments for task {taskId}: {response.StatusCode}");
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
            TodoTaskLog.LogUnexpectedErrorRetrievingComments(this.logger, taskId, ex);
            throw;
        }
    }

    /// <summary>
    /// Adds a new comment to a specific task.
    /// </summary>
    /// <param name="commentModel">The comment model containing the comment text and task association.</param>
    /// <returns>A <see cref="CommentModel"/> representing the created comment.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="commentModel"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the comment creation fails.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public Task<CommentModel> AddTaskCommentAsync(CommentModel commentModel)
    {
        ArgumentNullException.ThrowIfNull(commentModel);

        return this.AddTaskCommentIntrenalAsync(commentModel);
    }

    /// <summary>
    /// Updates an existing comment on a task.
    /// </summary>
    /// <param name="userId">The ID of the user performing the update.</param>
    /// <param name="commentModel">The comment model containing the updated comment information.</param>
    /// <returns>A <see cref="CommentModel"/> representing the updated comment.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="commentModel"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the comment is not found or update fails.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public Task<CommentModel> UpdateTaskCommentAsync(int userId, CommentModel commentModel)
    {
        ArgumentNullException.ThrowIfNull(commentModel);

        return this.UpdateTaskCommentIntrenalAsync(commentModel);
    }

    /// <summary>
    /// Removes a comment from a task.
    /// </summary>
    /// <param name="userId">The ID of the user performing the operation.</param>
    /// <param name="taskId">The ID of the task containing the comment.</param>
    /// <param name="commentId">The ID of the comment to remove.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the comment is not found.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task RemoveTaskCommentAsync(int userId, int taskId, int commentId)
    {
        try
        {
            var uri = new Uri($"{taskId}/Comments/{commentId}", UriKind.Relative);

            using var response = await this.httpClient.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                TodoTaskLog.LogCommentDeleted(this.logger, commentId);
                return;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TodoTaskLog.LogCommentNotFound(this.logger, commentId);
                throw new InvalidOperationException($"Comment with ID {commentId} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogCommentOperationFailed(this.logger, "delete", commentId, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to delete comment: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoTaskLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TodoTaskLog.LogUnexpectedErrorDeletingComment(this.logger, commentId, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves a specific comment by its ID for a given task.
    /// </summary>
    /// <param name="userId">The ID of the user requesting the comment.</param>
    /// <param name="taskId">The ID of the task containing the comment.</param>
    /// <param name="commentId">The ID of the comment to retrieve.</param>
    /// <returns>A <see cref="CommentModel"/> representing the retrieved comment.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the comment is not found.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public async Task<CommentModel> GetCommentByIdAsync(int userId, int taskId, int commentId)
    {
        try
        {
            var uri = new Uri($"{taskId}/Comments/{commentId}", UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CommentDto>();
                if (result != null)
                {
                    TodoTaskLog.LogCommentRetrieved(this.logger, commentId, taskId);
                    return MapToModel.MapToCommentModel(result);
                }

                TodoTaskLog.LogNullResponse(this.logger, "get by id");
                throw new InvalidOperationException($"Comment with ID {commentId} was not found");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TodoTaskLog.LogTaskNotFound(this.logger, commentId);
                throw new InvalidOperationException($"Comment with ID {commentId} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogTaskFailed(this.logger, "get comment by id", commentId, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve comment: {response.StatusCode}");
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

    private async Task<TodoTaskModel> AddInternalAsync(TodoTaskModel model)
    {
        try
        {
            var dto = new CreateTodoTaskDto()
            {
                Title = model.Title,
                Description = model.Description,
                CreationDate = model.CreationDate,
                DueDate = model.DueDate,
                StatusId = model.StatusId,
                AssigneeId = model.OwnerUserId,
                ListId = model.ListId,
            };

            using var response = await this.httpClient.PutAsJsonAsync(this.httpClient.BaseAddress, dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TodoTaskDto>();
                if (result != null)
                {
                    TodoTaskLog.LogTaskCreated(this.logger, result.Id);
                    return MapToModel.MapToTodoTaskModel(result);
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

    private async Task<TodoTaskModel> UpdateIntrenalAsync(TodoTaskModel model)
    {
        try
        {
            var dto = new UpdateTodoTaskDto()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                StatusId = model.StatusId,
                AssigneeId = model.OwnerUserId,
                ListId = model.ListId,
            };

            using var response = await this.httpClient.PostAsJsonAsync(this.httpClient.BaseAddress, dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TodoTaskDto>();
                if (result != null)
                {
                    TodoTaskLog.LogTaskUpdated(this.logger, model.Id);
                    return MapToModel.MapToTodoTaskModel(result);
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

    private async Task<CommentModel> AddTaskCommentIntrenalAsync(CommentModel commentModel)
    {
        try
        {
            var dto = new CreateCommentDto() { Text = commentModel.Text };
            var uri = new Uri($"{commentModel.TaskId}/Comments", UriKind.Relative);

            using var response = await this.httpClient.PostAsJsonAsync(uri, dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CommentDto>();
                if (result != null)
                {
                    TodoTaskLog.LogCommentCreated(this.logger, result.Id, commentModel.TaskId);
                    return MapToModel.MapToCommentModel(result);
                }

                TodoTaskLog.LogNullResponse(this.logger, "create comment");
                throw new InvalidOperationException("Failed to create comment - null response");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogCommentOperationFailed(this.logger, "create", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to create comment: {response.StatusCode}");
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
            TodoTaskLog.LogUnexpectedErrorCreatingComment(this.logger, commentModel.TaskId, ex);
            throw;
        }
    }

    private async Task<CommentModel> UpdateTaskCommentIntrenalAsync(CommentModel commentModel)
    {
        try
        {
            var dto = new UpdateCommentDto()
            {
                Id = commentModel.Id,
                Text = commentModel.Text,
            };
            var uri = new Uri($"{commentModel.TaskId}/Comments/{commentModel.Id}", UriKind.Relative);

            using var response = await this.httpClient.PutAsJsonAsync(uri, dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CommentDto>();
                if (result != null)
                {
                    TodoTaskLog.LogCommentUpdated(this.logger, commentModel.Id);
                    return MapToModel.MapToCommentModel(result);
                }

                TodoTaskLog.LogNullResponse(this.logger, "update comment");
                throw new InvalidOperationException($"Failed to update comment {commentModel.Id} - null response");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TodoTaskLog.LogCommentNotFound(this.logger, commentModel.Id);
                throw new InvalidOperationException($"Comment with ID {commentModel.Id} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoTaskLog.LogCommentOperationFailed(this.logger, "update", commentModel.Id, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to update comment: {response.StatusCode}");
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
            TodoTaskLog.LogUnexpectedErrorUpdatingComment(this.logger, commentModel.Id, ex);
            throw;
        }
    }
}

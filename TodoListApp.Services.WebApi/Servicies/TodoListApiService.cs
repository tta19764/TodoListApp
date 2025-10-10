using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.Services.WebApi.CustomLogs;
using TodoListApp.Services.WebApi.Helpers;
using TodoListApp.WebApi.Models.Dtos.Create;
using TodoListApp.WebApi.Models.Dtos.Read;
using TodoListApp.WebApi.Models.Dtos.Update;

namespace TodoListApp.Services.WebApi.Servicies;

/// <summary>
/// Provides API-based CRUD operations and data retrieval for to-do lists.
/// </summary>
public class TodoListApiService : ITodoListService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<TodoListApiService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListApiService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to send API requests.</param>
    /// <param name="logger">The logger instance for structured logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClient"/> or <paramref name="logger"/> is null.</exception>
    public TodoListApiService(HttpClient httpClient, ILogger<TodoListApiService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new to-do list through the API.
    /// </summary>
    /// <param name="model">The to-do list model to create.</param>
    /// <returns>The created <see cref="TodoListModel"/> instance returned from the API.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
    /// <exception cref="HttpRequestException">Thrown when the API request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the API returns a null or invalid response.</exception>
    /// <exception cref="JsonException">Thrown when the API response cannot be deserialized.</exception>
    public async Task<TodoListModel> AddAsync(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        try
        {
            var dto = new CreateTodoListDto(
                model.Title,
                model.OwnerId,
                model.Description);

            using var response = await this.httpClient.PutAsJsonAsync(this.httpClient.BaseAddress, dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TodoListDto>();
                if (result != null)
                {
                    TodoListLog.LogTodoListCreated(this.logger, result.Id);
                    return MapToModel.MapToTodoListModel(result);
                }

                TodoListLog.LogNullResponse(this.logger, "create");
                throw new InvalidOperationException("Failed to create todo list - null response");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoListLog.LogTodoListFailed(this.logger, "create", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to create todo list: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoListLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (JsonException ex)
        {
            TodoListLog.LogJsonParsingError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TodoListLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Deletes a specific to-do list by its identifier.
    /// </summary>
    /// <param name="userId">The ID of the user performing the delete operation.</param>
    /// <param name="id">The ID of the to-do list to delete.</param>
    /// <exception cref="HttpRequestException">Thrown when the delete request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the specified to-do list is not found.</exception>
    public async Task DeleteAsync(int userId, int id)
    {
        try
        {
            var uri = new Uri(id.ToString(CultureInfo.InvariantCulture), UriKind.Relative);
            using var response = await this.httpClient.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                TodoListLog.LogTodoListDeleted(this.logger, id);
                return;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoListLog.LogTodoListFailed(this.logger, "delete", id, (int)response.StatusCode, errorContent);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException($"Todo list with ID {id} was not found");
            }

            throw new HttpRequestException($"Failed to delete todo list: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoListLog.LogHttpRequestError(this.logger, ex);
        }
        catch (Exception ex)
        {
            TodoListLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all to-do lists accessible by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A read-only list of <see cref="TodoListModel"/> objects representing all accessible lists.</returns>
    /// <exception cref="HttpRequestException">Thrown when the retrieval request fails.</exception>
    /// <exception cref="JsonException">Thrown when the response cannot be deserialized.</exception>
    public async Task<IReadOnlyList<TodoListModel>> GetAllAsync(int userId)
    {
        try
        {
            using var response = await this.httpClient.GetAsync(this.httpClient.BaseAddress);

            if (response.IsSuccessStatusCode)
            {
                var lists = await response.Content.ReadFromJsonAsync<List<TodoListDto>>();
                if (lists != null)
                {
                    TodoListLog.LogTodoListsRetrieved(this.logger, lists.Count, userId);
                    return lists.Select(MapToModel.MapToTodoListModel).ToList();
                }

                TodoListLog.LogNullResponse(this.logger, "get all");
                return new List<TodoListModel>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoListLog.LogTodoListFailed(this.logger, "get all", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve todo lists: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoListLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (JsonException ex)
        {
            TodoListLog.LogJsonParsingError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TodoListLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves a paginated list of all to-do lists accessible by a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="rowCount">The number of items per page.</param>
    /// <returns>A read-only paginated list of <see cref="TodoListModel"/> objects.</returns>
    /// <exception cref="HttpRequestException">Thrown when the retrieval request fails.</exception>
    /// <exception cref="JsonException">Thrown when the response cannot be deserialized.</exception>
    public async Task<IReadOnlyList<TodoListModel>> GetAllAsync(int userId, int pageNumber, int rowCount)
    {
        try
        {
            var uri = new Uri($"{pageNumber}/{rowCount}", UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var lists = await response.Content.ReadFromJsonAsync<List<TodoListDto>>();
                if (lists != null)
                {
                    TodoListLog.LogTodoListsPageRetrieved(this.logger, lists.Count, pageNumber, userId);
                    return lists.Select(MapToModel.MapToTodoListModel).ToList();
                }

                TodoListLog.LogNullResponse(this.logger, "get paginated");
                return new List<TodoListModel>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoListLog.LogTodoListFailed(this.logger, "get paginated", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve paginated todo lists: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoListLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (JsonException ex)
        {
            TodoListLog.LogJsonParsingError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TodoListLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all to-do lists created by a specific author.
    /// </summary>
    /// <param name="authorId">The ID of the author (owner).</param>
    /// <param name="pageNumber">The optional page number for pagination.</param>
    /// <param name="rowCount">The optional number of items per page.</param>
    /// <returns>A read-only list of <see cref="TodoListModel"/> objects representing the author's lists.</returns>
    /// <exception cref="HttpRequestException">Thrown when the retrieval request fails.</exception>
    /// <exception cref="JsonException">Thrown when the response cannot be deserialized.</exception>
    public async Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId, int? pageNumber = null, int? rowCount = null)
    {
        try
        {
            Uri uri;
            int page = 0;
            int row = 0;
            if (pageNumber != null && rowCount != null)
            {
                page = pageNumber > 0 ? (int)pageNumber : 1;
                row = rowCount > 0 ? (int)rowCount : 1;

                uri = new Uri($"UserLists/{page}/{row}", UriKind.Relative);
            }
            else
            {
                uri = new Uri("UserLists", UriKind.Relative);
            }

            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var lists = await response.Content.ReadFromJsonAsync<List<TodoListDto>>();
                if (lists != null)
                {
                    if (pageNumber != null && rowCount != null)
                    {
                        TodoListLog.LogTodoListsPageRetrievedByAuthor(this.logger, lists.Count, page, authorId);
                    }
                    else
                    {
                        TodoListLog.LogTodoListsRetrievedByAuthor(this.logger, lists.Count, authorId);
                    }

                    return lists.Select(MapToModel.MapToTodoListModel).ToList();
                }

                TodoListLog.LogNullResponse(this.logger, "get paginated by author");
                return new List<TodoListModel>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoListLog.LogTodoListFailed(this.logger, "get paginated by author", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve paginated todo lists by author: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoListLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (JsonException ex)
        {
            TodoListLog.LogJsonParsingError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TodoListLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all to-do lists shared with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user with whom the lists are shared.</param>
    /// <param name="pageNumber">The optional page number for pagination.</param>
    /// <param name="rowCount">The optional number of items per page.</param>
    /// <returns>A read-only list of <see cref="TodoListModel"/> objects representing shared lists.</returns>
    /// <exception cref="HttpRequestException">Thrown when the retrieval request fails.</exception>
    /// <exception cref="JsonException">Thrown when the response cannot be deserialized.</exception>
    public async Task<IReadOnlyList<TodoListModel>> GetAllSharedAsync(int userId, int? pageNumber = null, int? rowCount = null)
    {
        try
        {
            Uri uri;
            int page = 0;
            int row = 0;
            if (pageNumber != null && rowCount != null)
            {
                page = pageNumber > 0 ? (int)pageNumber : 1;
                row = rowCount > 0 ? (int)rowCount : 1;

                uri = new Uri($"Shared/{page}/{row}", UriKind.Relative);
            }
            else
            {
                uri = new Uri("Shared", UriKind.Relative);
            }

            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var lists = await response.Content.ReadFromJsonAsync<List<TodoListDto>>();
                if (lists != null)
                {
                    if (pageNumber != null && rowCount != null)
                    {
                        TodoListLog.LogTodoListsPageRetrievedShared(this.logger, lists.Count, page, userId);
                    }
                    else
                    {
                        TodoListLog.LogTodoListsRetrievedShared(this.logger, lists.Count, userId);
                    }

                    return lists.Select(MapToModel.MapToTodoListModel).ToList();
                }

                TodoListLog.LogNullResponse(this.logger, "get paginated shared");
                return new List<TodoListModel>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoListLog.LogTodoListFailed(this.logger, "get paginated shared", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve paginated shared todo lists: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoListLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (JsonException ex)
        {
            TodoListLog.LogJsonParsingError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TodoListLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves a specific to-do list by its identifier.
    /// </summary>
    /// <param name="userId">The ID of the user performing the operation.</param>
    /// <param name="id">The ID of the to-do list to retrieve.</param>
    /// <returns>The <see cref="TodoListModel"/> that matches the specified identifier.</returns>
    /// <exception cref="HttpRequestException">Thrown when the retrieval request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the list is not found or response is null.</exception>
    /// <exception cref="JsonException">Thrown when the response cannot be deserialized.</exception>
    public async Task<TodoListModel> GetByIdAsync(int userId, int id)
    {
        try
        {
            var uri = new Uri(id.ToString(CultureInfo.InvariantCulture), UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var list = await response.Content.ReadFromJsonAsync<TodoListDto>();
                if (list != null)
                {
                    TodoListLog.LogTodoListRetrievedById(this.logger, id, userId);
                    return MapToModel.MapToTodoListModel(list);
                }

                TodoListLog.LogNullResponse(this.logger, "get by id");
                throw new InvalidOperationException($"Todo list with ID {id} was not found");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TodoListLog.LogTodoListNotFound(this.logger, id);
                throw new InvalidOperationException($"Todo list with ID {id} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoListLog.LogTodoListFailed(this.logger, "get by id", id, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve todo list: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoListLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (JsonException ex)
        {
            TodoListLog.LogJsonParsingError(this.logger, ex);
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            TodoListLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing to-do list through the API.
    /// </summary>
    /// <param name="userId">The ID of the user performing the update.</param>
    /// <param name="model">The updated to-do list model.</param>
    /// <returns>The updated <see cref="TodoListModel"/> returned by the API.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
    /// <exception cref="HttpRequestException">Thrown when the update request fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the list is not found or response is null.</exception>
    /// <exception cref="JsonException">Thrown when the response cannot be deserialized.</exception>
    public async Task<TodoListModel> UpdateAsync(int userId, TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        try
        {
            var dto = new UpdateTodoListDto(
                model.Id,
                model.Title,
                model.Description,
                model.OwnerId);

            using var response = await this.httpClient.PostAsJsonAsync(this.httpClient.BaseAddress, dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TodoListDto>();
                if (result != null)
                {
                    TodoListLog.LogTodoListUpdated(this.logger, model.Id);
                    return MapToModel.MapToTodoListModel(result);
                }

                TodoListLog.LogNullResponse(this.logger, "update");
                throw new InvalidOperationException($"Failed to update todo list {model.Id} - null response");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TodoListLog.LogTodoListNotFound(this.logger, model.Id);
                throw new InvalidOperationException($"Todo list with ID {model.Id} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoListLog.LogTodoListFailed(this.logger, "update", model.Id, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to update todo list: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoListLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (JsonException ex)
        {
            TodoListLog.LogJsonParsingError(this.logger, ex);
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            TodoListLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves the count of all to-do lists accessible by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The number of to-do lists accessible by the user.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task<int> AllByUserCount(int userId)
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

                TodoListLog.LogNullResponse(this.logger, "get all by user count");
                return 0;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoListLog.LogTodoListFailed(this.logger, "get all by user count", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve todo lists count: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoListLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TodoListLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves the count of to-do lists owned/created by a specific author.
    /// </summary>
    /// <param name="authorId">The ID of the author/owner.</param>
    /// <returns>The number of to-do lists owned by the author.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task<int> AllByAuthorCount(int authorId)
    {
        try
        {
            var uri = new Uri("UserLists/Count", UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var countString = await response.Content.ReadAsStringAsync();
                if (int.TryParse(countString, out int count))
                {
                    return count;
                }

                TodoListLog.LogNullResponse(this.logger, "get all by author count");
                return 0;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoListLog.LogTodoListFailed(this.logger, "get all by author count", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve todo lists by author count: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoListLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TodoListLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves the count of to-do lists that are shared with a specific user (lists they don't own).
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The number of to-do lists shared with the user.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task<int> AllSharedCount(int userId)
    {
        try
        {
            var uri = new Uri("Shared/Count", UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var countString = await response.Content.ReadAsStringAsync();
                if (int.TryParse(countString, out int count))
                {
                    return count;
                }

                TodoListLog.LogNullResponse(this.logger, "get all shared count");
                return 0;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TodoListLog.LogTodoListFailed(this.logger, "get all shared count", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve shared todo lists count: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TodoListLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TodoListLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }
}

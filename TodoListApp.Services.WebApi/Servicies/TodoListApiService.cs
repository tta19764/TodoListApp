using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.Services.WebApi.CustomLogs;
using TodoListApp.WebApi.Models.Dtos.Create;
using TodoListApp.WebApi.Models.Dtos.Read;
using TodoListApp.WebApi.Models.Dtos.Update;

namespace TodoListApp.Services.WebApi.Servicies;
public class TodoListApiService : ITodoListService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<TodoListApiService> logger;

    public TodoListApiService(HttpClient httpClient, ILogger<TodoListApiService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

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
                    return MapToModel(result);
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
                    return lists.Select(MapToModel).ToList();
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
                    return lists.Select(MapToModel).ToList();
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

                    return lists.Select(MapToModel).ToList();
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

                    return lists.Select(MapToModel).ToList();
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
                    return MapToModel(list);
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
                    return MapToModel(result);
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

    private static TodoListModel MapToModel(TodoListDto dto)
    {
        return new TodoListModel(
            id: dto.Id,
            ownerId: dto.ListOwnerId,
            title: dto.Title,
            description: dto.Description ?? string.Empty)
        {
            UserRole = dto.ListRole,
            OwnerFullName = dto.OwnerName,
            ActiveTasks = dto.PendingTasks,
        };
    }
}

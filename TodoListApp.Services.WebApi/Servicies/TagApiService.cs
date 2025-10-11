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
/// Provides API-based CRUD operations and data retrieval for tags.
/// </summary>
public class TagApiService : ITagService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<TagApiService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagApiService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to send API requests.</param>
    /// <param name="logger">The logger instance for structured logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClient"/> or <paramref name="logger"/> is null.</exception>
    public TagApiService(ILogger<TagApiService> logger, HttpClient httpClient)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Creates a new tag.
    /// </summary>
    /// <param name="model">The tag model containing the tag information to create.</param>
    /// <returns>A <see cref="TagModel"/> representing the created tag.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the creation fails or returns a null response.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public Task<TagModel> AddAsync(TagModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return this.AddInternalAsync(model);
    }

    /// <summary>
    /// Deletes a tag by its ID.
    /// </summary>
    /// <param name="id">The ID of the tag to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the tag is not found.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task DeleteAsync(int id)
    {
        try
        {
            var uri = new Uri(id.ToString(CultureInfo.InvariantCulture), UriKind.Relative);
            using var response = await this.httpClient.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                TagLog.LogTagDeleted(this.logger, id);
                return;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TagLog.LogOperationFailed(this.logger, "delete", id, (int)response.StatusCode, errorContent);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException($"Tag with ID {id} was not found");
            }

            throw new HttpRequestException($"Failed to delete tag: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TagLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all tags from the system.
    /// </summary>
    /// <returns>A read-only list of <see cref="TagModel"/> representing all tags.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public async Task<IReadOnlyList<TagModel>> GetAllAsync()
    {
        try
        {
            using var response = await this.httpClient.GetAsync(this.httpClient.BaseAddress);

            if (response.IsSuccessStatusCode)
            {
                var tags = await response.Content.ReadFromJsonAsync<List<TagDto>>();
                if (tags != null)
                {
                    TagLog.LogTagsRetrieved(this.logger, tags.Count);
                    return tags.Select(MapToModel.MapToTagModel).ToList();
                }

                TagLog.LogNullResponse(this.logger, "get all");
                return new List<TagModel>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TagLog.LogOperationFailed(this.logger, "get all", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve tags: {response.StatusCode}");
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
    /// Retrieves a paginated list of tags.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="rowCount">The number of tags per page.</param>
    /// <returns>A read-only list of <see cref="TagModel"/> for the specified page.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public async Task<IReadOnlyList<TagModel>> GetAllAsync(int pageNumber, int rowCount)
    {
        try
        {
            var uri = new Uri($"{pageNumber}/{rowCount}", UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var tags = await response.Content.ReadFromJsonAsync<List<TagDto>>();
                if (tags != null)
                {
                    TagLog.LogTagsPageRetrieved(this.logger, tags.Count, pageNumber);
                    return tags.Select(MapToModel.MapToTagModel).ToList();
                }

                TagLog.LogNullResponse(this.logger, "get paginated");
                return new List<TagModel>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TagLog.LogOperationFailed(this.logger, "get paginated", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve paginated tags: {response.StatusCode}");
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
    /// Retrieves all available tags that can be added to a specific task (tags not already associated with the task).
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="pageNumber">Optional page number for pagination (1-based).</param>
    /// <param name="rowCount">Optional number of tags per page.</param>
    /// <returns>A read-only list of <see cref="TagModel"/> representing available tags for the task.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public async Task<IReadOnlyList<TagModel>> GetAvailableTaskTagsAsync(int taskId, int? pageNumber = null, int? rowCount = null)
    {
        try
        {
            Uri uri;
            if (pageNumber.HasValue && rowCount.HasValue)
            {
                uri = new Uri($"Available/{taskId}/{pageNumber.Value}/{rowCount.Value}", UriKind.Relative);
            }
            else
            {
                uri = new Uri($"Available/{taskId}", UriKind.Relative);
            }

            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var tags = await response.Content.ReadFromJsonAsync<List<TagDto>>();
                if (tags != null)
                {
                    TagLog.LogAvailableTagsRetrieved(this.logger, tags.Count, taskId);
                    return tags.Select(MapToModel.MapToTagModel).ToList();
                }

                TagLog.LogNullResponse(this.logger, "get available tags");
                return new List<TagModel>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TagLog.LogOperationFailed(this.logger, "get available tags", taskId, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve available tags: {response.StatusCode}");
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
    /// Retrieves the count of available tags that can be added to a specific task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <returns>The number of available tags for the task.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task<int> GetAvilableTaskTagsCountAsync(int taskId)
    {
        try
        {
            var uri = new Uri($"Available/{taskId}/Count", UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var countString = await response.Content.ReadAsStringAsync();
                if (int.TryParse(countString, out int count))
                {
                    return count;
                }

                TagLog.LogNullResponse(this.logger, "get available tags count");
                return 0;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TagLog.LogOperationFailed(this.logger, "get available tags count", taskId, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve available tags count: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            TagLog.LogHttpRequestError(this.logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves a tag by its ID.
    /// </summary>
    /// <param name="id">The ID of the tag to retrieve.</param>
    /// <returns>A <see cref="TagModel"/> representing the retrieved tag.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the tag is not found.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public async Task<TagModel> GetByIdAsync(int id)
    {
        try
        {
            var uri = new Uri(id.ToString(CultureInfo.InvariantCulture), UriKind.Relative);
            using var response = await this.httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var tag = await response.Content.ReadFromJsonAsync<TagDto>();
                if (tag != null)
                {
                    TagLog.LogTagRetrieved(this.logger, id);
                    return MapToModel.MapToTagModel(tag);
                }

                TagLog.LogNullResponse(this.logger, "get by id");
                throw new InvalidOperationException($"Tag with ID {id} was not found");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TagLog.LogTagNotFound(this.logger, id);
                throw new InvalidOperationException($"Tag with ID {id} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TagLog.LogOperationFailed(this.logger, "get by id", id, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to retrieve tag: {response.StatusCode}");
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
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing tag.
    /// </summary>
    /// <param name="model">The tag model containing the updated tag information.</param>
    /// <returns>A <see cref="TagModel"/> representing the updated tag.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the tag is not found or update fails.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="JsonException">Thrown when JSON parsing fails.</exception>
    public Task<TagModel> UpdateAsync(TagModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return this.UpdateInternalAsync(model);
    }

    private async Task<TagModel> AddInternalAsync(TagModel model)
    {
        try
        {
            var dto = new CreateTagDto { Title = model.Title };

            using var response = await this.httpClient.PutAsJsonAsync(this.httpClient.BaseAddress, dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TagDto>();
                if (result != null)
                {
                    TagLog.LogTagCreated(this.logger, result.Id);
                    return MapToModel.MapToTagModel(result);
                }

                TagLog.LogNullResponse(this.logger, "create");
                throw new InvalidOperationException("Failed to create tag - null response");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TagLog.LogOperationFailed(this.logger, "create", 0, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to create tag: {response.StatusCode}");
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

    private async Task<TagModel> UpdateInternalAsync(TagModel model)
    {
        try
        {
            var dto = new UpdateTagDto { Id = model.Id, Title = model.Title };

            using var response = await this.httpClient.PostAsJsonAsync(this.httpClient.BaseAddress, dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TagDto>();
                if (result != null)
                {
                    TagLog.LogTagUpdated(this.logger, model.Id);
                    return MapToModel.MapToTagModel(result);
                }

                TagLog.LogNullResponse(this.logger, "update");
                throw new InvalidOperationException($"Failed to update tag {model.Id} - null response");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TagLog.LogTagNotFound(this.logger, model.Id);
                throw new InvalidOperationException($"Tag with ID {model.Id} was not found");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TagLog.LogOperationFailed(this.logger, "update", model.Id, (int)response.StatusCode, errorContent);
            throw new HttpRequestException($"Failed to update tag: {response.StatusCode}");
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
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedError(this.logger, ex);
            throw;
        }
    }
}

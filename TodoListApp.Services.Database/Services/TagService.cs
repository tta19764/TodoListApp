using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Helpers;
using TodoListApp.Services.Database.Repositories;
using TodoListApp.Services.Database.ServiceExeptions;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Services;

/// <summary>
/// Provides CRUD and query operations for managing tag entities.
/// </summary>
public class TagService : ITagService
{
    private readonly TagRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagService"/> class using the provided database context.
    /// </summary>
    /// <param name="context">The <see cref="TodoListDbContext"/> used to access the database.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <c>null</c>.</exception>
    public TagService(TodoListDbContext context)
    {
        this.repository = new TagRepository(context);
    }

    /// <summary>
    /// Adds a new tag to the database.
    /// </summary>
    /// <param name="model">The tag model containing the data to create.</param>
    /// <returns>The created <see cref="TagModel"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is <c>null</c>.</exception>
    /// <exception cref="EntityWithIdExistsException">Thrown when a tag with the same ID already exists.</exception>
    /// <exception cref="UnableToCreateException">Thrown when the database operation to create the tag fails.</exception>
    public Task<TagModel> AddAsync(TagModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return this.AddInternalAsync(model);
    }

    /// <summary>
    /// Deletes an existing tag from the database by its identifier.
    /// </summary>
    /// <param name="id">The ID of the tag to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the tag with the specified ID does not exist.</exception>
    /// <exception cref="UnableToDeleteException">Thrown when the database operation to delete the tag fails.</exception>
    public async Task DeleteAsync(int id)
    {
        Tag? existing = await this.repository.GetByIdAsync(id);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), id);
        }

        try
        {
            bool succes = await this.repository.DeleteByIdAsync(id);
            if (!succes)
            {
                throw new UnableToDeleteException(nameof(existing), id);
            }
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToDeleteException(nameof(existing), id, ex);
        }
    }

    /// <summary>
    /// Retrieves all tags from the database.
    /// </summary>
    /// <returns>A read-only list of <see cref="TagModel"/> representing all tags.</returns>
    public async Task<IReadOnlyList<TagModel>> GetAllAsync()
    {
        var lists = await this.repository.GetAllAsync();
        return lists
            .Select(l =>
            EntityToModelConverter.ToTagModel(l))
            .ToList();
    }

    /// <summary>
    /// Retrieves a paginated subset of tags from the database.
    /// </summary>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="rowCount">The number of records to include per page.</param>
    /// <returns>A read-only list of <see cref="TagModel"/> for the specified page.</returns>
    public async Task<IReadOnlyList<TagModel>> GetAllAsync(int pageNumber, int rowCount)
    {
        var lists = await this.repository.GetAllAsync(pageNumber, rowCount);
        return lists
            .Select(l =>
            EntityToModelConverter.ToTagModel(l))
            .ToList();
    }

    /// <summary>
    /// Retrieves a tag by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the tag to retrieve.</param>
    /// <returns>The corresponding <see cref="TagModel"/> if found.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the tag with the specified ID does not exist.</exception>
    public async Task<TagModel> GetByIdAsync(int id)
    {
        var entity = await this.repository.GetByIdAsync(id);

        if (entity is null)
        {
            throw new EntityNotFoundException(nameof(entity), id);
        }

        return EntityToModelConverter.ToTagModel(entity);
    }

    /// <summary>
    /// Updates an existing tag in the database.
    /// </summary>
    /// <param name="model">The tag model containing the updated data.</param>
    /// <returns>The updated <see cref="TagModel"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is <c>null</c>.</exception>
    /// <exception cref="EntityNotFoundException">Thrown when the tag to update does not exist.</exception>
    /// <exception cref="UnableToUpdateException">Thrown when the database operation to update the tag fails.</exception>
    public Task<TagModel> UpdateAsync(TagModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return this.UpdateInternalAsync(model);
    }

    /// <summary>
    /// Retrieves the count of tags that are not associated with a specific task.
    /// </summary>
    /// <param name="taskId">The ID of the task to check associations against.</param>
    /// <returns>The number of tags that are not currently assigned to the specified task.</returns>
    public async Task<int> GetAvilableTaskTagsCountAsync(int taskId)
    {
        var tags = await this.repository.GetAllAsync();
        return tags.Count(t => t.TaskTags.All(tt => tt.TaskId != taskId));
    }

    /// <summary>
    /// Retrieves the tags that are not associated with a specific task.
    /// </summary>
    /// <param name="taskId">The ID of the task to check associations against.</param>
    /// <param name="pageNumber">Optional page number (1-based) for pagination.</param>
    /// <param name="rowCount">Optional number of rows per page for pagination.</param>
    /// <returns>
    /// A read-only list of <see cref="TagModel"/> representing the available tags not linked to the specified task.
    /// </returns>
    public async Task<IReadOnlyList<TagModel>> GetAvailableTaskTagsAsync(int taskId, int? pageNumber = null, int? rowCount = null)
    {
        var tags = await this.repository.GetAllAsync();
        var availableTags = tags.Where(t => t.TaskTags.All(tt => tt.TaskId != taskId)).AsEnumerable();
        if (pageNumber != null && rowCount != null)
        {
            int page = pageNumber > 0 ? (int)pageNumber : 1;
            int row = rowCount > 0 ? (int)rowCount : 1;
            availableTags = availableTags
                .OrderBy(x => x.Id)
                .Skip((page - 1) * row)
                .Take(row);
        }

        return availableTags
            .Select(t => EntityToModelConverter.ToTagModel(t))
            .ToList();
    }

    private async Task<TagModel> AddInternalAsync(TagModel model)
    {
        Tag? existing = await this.repository.GetByIdAsync(model.Id);
        if (existing is not null)
        {
            throw new EntityWithIdExistsException(nameof(Tag), model.Id);
        }

        try
        {
            Tag newTag = await this.repository.AddAsync(ModelToEntityConverter.ToTagEntity(model));
            return EntityToModelConverter.ToTagModel(newTag);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToCreateException(nameof(Tag), ex, model.Id);
        }
    }

    private async Task<TagModel> UpdateInternalAsync(TagModel model)
    {
        Tag? existing = await this.repository.GetByIdAsync(model.Id);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), model.Id);
        }

        try
        {
            Tag? updated = await this.repository.UpdateAsync(ModelToEntityConverter.ToTagEntity(model));
            return updated is null ?
                throw new UnableToUpdateException(nameof(existing), model.Id) :
                EntityToModelConverter.ToTagModel(updated);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToUpdateException(nameof(existing), model.Id, ex);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Helpers;
using TodoListApp.Services.Database.Repositories;
using TodoListApp.Services.Database.ServiceExeptions;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Services;
public class TagSerivice : ITagService
{
    private readonly TagRepository repository;

    public TagSerivice(TodoListDbContext context)
    {
        this.repository = new TagRepository(context);
    }

    public async Task<TagModel> AddAsync(TagModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        Tag? existing = await this.repository.GetByIdAsync(model.Id);
        if (existing is not null)
        {
            throw new EntityWithIdExistsException(nameof(TodoList), model.Id);
        }

        try
        {
            Tag newTag = await this.repository.AddAsync(ModelToEntity(model));
            return EntityToModelConverter.ToTagModel(newTag);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToCreateException(nameof(TodoList), ex, model.Id);
        }
    }

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

    public async Task<IReadOnlyList<TagModel>> GetAllAsync()
    {
        var lists = await this.repository.GetAllAsync();
        return lists
            .Select(l =>
            EntityToModelConverter.ToTagModel(l))
            .ToList();
    }

    public async Task<IReadOnlyList<TagModel>> GetAllAsync(int pageNumber, int rowCount)
    {
        var lists = await this.repository.GetAllAsync(pageNumber, rowCount);
        return lists
            .Select(l =>
            EntityToModelConverter.ToTagModel(l))
            .ToList();
    }

    public async Task<TagModel> GetByIdAsync(int id)
    {
        var entity = await this.repository.GetByIdAsync(id);

        if (entity is null)
        {
            throw new EntityNotFoundException(nameof(entity), id);
        }

        return EntityToModelConverter.ToTagModel(entity);
    }

    public async Task<TagModel> UpdateAsync(TagModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        Tag? existing = await this.repository.GetByIdAsync(model.Id);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), model.Id);
        }

        try
        {
            Tag? updated = await this.repository.UpdateAsync(ModelToEntity(model));
            return updated is null ?
                throw new UnableToUpdateException(nameof(existing), model.Id) :
                EntityToModelConverter.ToTagModel(updated);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToUpdateException(nameof(existing), model.Id, ex);
        }
    }

    public async Task<int> GetAvilableTaskTagsCountAsync(int taskId)
    {
        var tags = await this.repository.GetAllAsync();
        return tags.Count(t => t.TaskTags.All(tt => tt.TaskId != taskId));
    }

    public async Task<IReadOnlyList<TagModel>> GetAvilableTaskTagsAsync(int taskId, int? pageNumber = null, int? rowCount = null)
    {
        var tags = await this.repository.GetAllAsync();
        var availableTags = tags.Where(t => t.TaskTags.All(tt => tt.TaskId != taskId)).AsEnumerable();
        if (pageNumber != null && rowCount != null)
        {
            int page = pageNumber > 0 ? (int)pageNumber : 1;
            int row = rowCount > 0 ? (int)rowCount : 1;
            availableTags = availableTags
                .Skip((page - 1) * row)
                .Take(row);
        }

        return availableTags
            .Select(t => EntityToModelConverter.ToTagModel(t))
            .ToList();
    }

    private static Tag ModelToEntity(TagModel model)
    {
        return new Tag
        {
            Id = model.Id,
            Label = model.Title,
        };
    }
}

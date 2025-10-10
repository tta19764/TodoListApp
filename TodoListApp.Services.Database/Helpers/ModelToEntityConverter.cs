using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Helpers;

/// <summary>
/// Converts service models to database entities.
/// </summary>
public static class ModelToEntityConverter
{
    /// <summary>
    /// Converts a <see cref="TodoTaskModel"/> to a <see cref="TodoTask"/> entity.
    /// </summary>
    /// <param name="model">The task model.</param>
    /// <returns>The corresponding task entity.</returns>
    public static TodoTask ToTaskEntity(TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var entity = new TodoTask()
        {
            Id = model.Id,
            ListId = model.ListId,
            DueDate = model.DueDate,
            Description = model.Description,
            CreationDate = model.CreationDate ?? DateTime.UtcNow,
            OwnerUserId = model.OwnerUserId,
            StatusId = model.StatusId,
            Title = model.Title,
        };

        return entity;
    }

    /// <summary>
    /// Converts a <see cref="CommentModel"/> to a <see cref="Comment"/> entity.
    /// </summary>
    /// <param name="model">The comment model.</param>
    /// <returns>The corresponding comment entity.</returns>
    public static Comment ToCommentEntity(CommentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var entity = new Comment()
        {
            Id = model.Id,
            TaskId = model.TaskId,
            UserId = model.UserId,
            Text = model.Text,
        };

        return entity;
    }

    /// <summary>
    /// Converts a <see cref="TagModel"/> to a <see cref="Tag"/> entity.
    /// </summary>
    /// <param name="model">The list model.</param>
    /// <returns>The corresponding list entity.</returns>
    public static TodoList ToListEntity(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var entity = new TodoList()
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description ?? string.Empty,
            OwnerId = model.OwnerId,
        };

        return entity;
    }

    /// <summary>
    /// Converts a <see cref="TagModel"/> to a <see cref="Tag"/> entity.
    /// </summary>
    /// <param name="model">The tag model.</param>
    /// <returns>The corresponding tag entity.</returns>
    public static Tag ToTagEntity(TagModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return new Tag
        {
            Id = model.Id,
            Label = model.Title,
        };
    }
}

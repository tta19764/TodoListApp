using System.Collections.ObjectModel;
using TodoListApp.Services.Models;
using TodoListApp.WebApi.Models.Dtos.Read;

namespace TodoListApp.Services.WebApi.Helpers;

/// <summary>
/// Maps Data Transfer Objects (DTOs) to Service Models.
/// </summary>
internal static class MapToModel
{
    /// <summary>
    /// Maps a <see cref="TodoTaskDto"/> to a <see cref="TodoTaskModel"/>.
    /// </summary>
    /// <param name="dto">The task data transfer object.</param>
    /// <returns>The mapped task model.</returns>
    public static TodoTaskModel MapToTodoTaskModel(TodoTaskDto dto)
    {
        return new TodoTaskModel(dto.Id)
        {
            Title = dto.Title,
            Description = dto.Description ?? string.Empty,
            CreationDate = dto.CreationDate,
            DueDate = dto.DueDate,
            StatusId = 0,
            OwnerUserId = dto.AssigneeId,
            ListId = dto.ListId,
            OwnerUser = new UserModel(dto.AssigneeId, dto.AssigneeFirstName, dto.AssigneeLastName),
            UsersTags = new ReadOnlyCollection<TagModel>(dto.Tags.Select(t => new TagModel(t.Id, t.Title)).ToList()),
            Status = new StatusModel(0, dto.Status),
        };
    }

    /// <summary>
    /// Maps a <see cref="TagDto"/> to a <see cref="TagModel"/>.
    /// </summary>
    /// <param name="dto">The tag data transfer object.</param>
    /// <returns>The mapped tag model.</returns>
    public static TagModel MapToTagModel(TagDto dto)
    {
        return new TagModel(dto.Id, dto.Title);
    }

    /// <summary>
    /// Maps a <see cref="CommentDto"/> to a <see cref="CommentModel"/>.
    /// </summary>
    /// <param name="dto">The comment data transfer object.</param>
    /// <returns>The mapped comment model.</returns>
    public static CommentModel MapToCommentModel(CommentDto dto)
    {
        return new CommentModel(dto.Id, dto.Text, dto.TaskId, dto.UserId, new UserModel(dto.UserId, dto.UserFirstName, dto.UserLastName));
    }

    /// <summary>
    /// Maps a <see cref="TodoListDto"/> to a <see cref="TodoListModel"/>.
    /// </summary>
    /// <param name="dto">To-do list data transfer object.</param>
    /// <returns>The mapped to-do list model.</returns>
    public static TodoListModel MapToTodoListModel(TodoListDto dto)
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

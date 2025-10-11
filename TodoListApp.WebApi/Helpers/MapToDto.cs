using System.Collections.ObjectModel;
using TodoListApp.Services.Models;
using TodoListApp.WebApi.Models.Dtos.Read;

namespace TodoListApp.WebApi.Helpers;

/// <summary>
/// Maps service models to DTOs.
/// </summary>
internal static class MapToDto
{
    /// <summary>
    /// Maps a TodoTaskModel to a TodoTaskDto.
    /// </summary>
    /// <param name="model">The task model.</param>
    /// <returns>The mapped task DTO.</returns>
    public static TodoTaskDto ToTodoTaskDto(TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return new TodoTaskDto()
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            CreationDate = model.CreationDate ?? DateTime.MinValue,
            DueDate = model.DueDate,
            AssigneeFirstName = model.OwnerUser!.FirstName,
            AssigneeLastName = model.OwnerUser!.LastName,
            AssigneeId = model.OwnerUser!.Id,
            Status = model.Status?.StatusTitle ?? "Unknown",
            ListId = model.ListId,
            Tags = new ReadOnlyCollection<TagDto>(model.UsersTags.Select(ut => new TagDto() { Id = ut.Id, Title = ut.Title }).ToList()),
        };
    }

    /// <summary>
    /// Maps a TagModel to a TagDto.
    /// </summary>
    /// <param name="model">The tag model.</param>
    /// <returns>The mapped tag DTO.</returns>
    public static TagDto ToTagDto(TagModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return new TagDto
        {
            Id = model.Id,
            Title = model.Title,
        };
    }

    /// <summary>
    /// Maps a CommentModel to a CommentDto.
    /// </summary>
    /// <param name="model">The comment model.</param>
    /// <returns>The mapped comment DTO.</returns>
    public static CommentDto ToCommentDto(CommentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return new CommentDto
        {
            Id = model.Id,
            Text = model.Text,
            TaskId = model.TaskId,
            UserId = model.UserId,
            UserFirstName = model.User?.FirstName ?? string.Empty,
            UserLastName = model.User?.LastName ?? string.Empty,
        };
    }

    /// <summary>
    /// Maps a TodoListModel to a TodoListDto.
    /// </summary>
    /// <param name="model">The list model.</param>
    /// <returns>The mapped list DTO.</returns>
    public static TodoListDto ToTodoListDto(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return new TodoListDto()
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            OwnerName = model.OwnerFullName ?? "N/A",
            ListRole = model.UserRole,
            ListOwnerId = model.OwnerId,
            PendingTasks = model.ActiveTasks,
        };
    }
}

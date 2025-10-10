using TodoListApp.Services.Models;
using TodoListApp.WebApp.Models.Comment;
using TodoListApp.WebApp.Models.List;
using TodoListApp.WebApp.Models.Tag;
using TodoListApp.WebApp.Models.Task;

namespace TodoListApp.WebApp.Helpers;

/// <summary>
/// Helper class for mapping service models to view models.
/// </summary>
public static class MapToViewModel
{
    /// <summary>
    /// Maps a TodoTaskModel to a TodoTaskViewModel.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="model">The task model.</param>
    /// <returns>The mapped task view model.</returns>
    public static TodoTaskViewModel ToTask(int userId, TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return new TodoTaskViewModel
        {
            TaskId = model.Id,
            Title = model.Title,
            Description = model.Description,
            CreationDate = model.CreationDate ?? DateTime.MinValue,
            DueDate = model.DueDate,
            Status = model.Status?.StatusTitle ?? "Unknown",
            ListId = model.ListId,
            OwnerName = Formaters.FormatOwnerName(model.OwnerUser?.FirstName, model.OwnerUser?.LastName),
            Tags = model.UsersTags?.Select(tag => tag.Title) ?? Enumerable.Empty<string>(),
            IsAssignee = model.OwnerUserId == userId,
        };
    }

    /// <summary>
    /// Maps a TodoListModel to a TodoListViewModel.
    /// </summary>
    /// <param name="model">The list model.</param>
    /// <returns>The mapped list view model.</returns>
    public static TodoListViewModel ToList(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return new TodoListViewModel
        {
            ListId = model.Id,
            Title = model.Title,
            Description = model.Description,
            UserRole = Formaters.StringToRoleEnum(model.UserRole),
            PendingTasks = model.ActiveTasks,
        };
    }

    /// <summary>
    /// Maps a TagModel to a TagViewModel.
    /// </summary>
    /// <param name="model">The tag model.</param>
    /// <returns>The mapped tag view model.</returns>
    public static TagViewModel ToTag(TagModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return new TagViewModel
        {
            Id = model.Id,
            Title = model.Title,
        };
    }

    /// <summary>
    /// Maps a CommentModel to a CommentViewModel.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="model">The comment model.</param>
    /// <returns>The mapped comment view model.</returns>
    public static CommentViewModel ToComment(int userId, CommentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return new CommentViewModel
        {
            Id = model.Id,
            Text = model.Text,
            TaskId = model.TaskId,
            UserId = model.UserId,
            UserName = Formaters.FormatOwnerName(model.User?.FirstName, model.User?.LastName),
            CanEdit = model.UserId == userId,
        };
    }
}

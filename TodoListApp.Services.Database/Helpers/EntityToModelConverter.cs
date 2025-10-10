using System.Collections.ObjectModel;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Helpers;

/// <summary>
/// Converts database entities to service models.
/// </summary>
public static class EntityToModelConverter
{
    /// <summary>
    /// Converts a TodoTask entity to a TodoTaskModel.
    /// </summary>
    /// <param name="entity">The task entity.</param>
    /// <returns>The corresponding task model.</returns>
    public static TodoTaskModel ToTodoTaskModel(TodoTask entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var taskTags = new List<TagModel>();
        if (entity.TaskTags != null)
        {
            foreach (var taskTag in entity.TaskTags)
            {
                if (!taskTags.Any(t => t.Title == taskTag.Tag.Label))
                {
                    taskTags.Add(new TagModel(taskTag.TagId, taskTag.Tag.Label));
                }
            }
        }

        var taskComments = new List<CommentModel>();
        if (entity.Comments != null)
        {
            foreach (var taskComment in entity.Comments)
            {
                UserModel? author = null;
                if (taskComment.Author != null)
                {
                    author = new UserModel(taskComment.Author.Id, taskComment.Author.FirstName, taskComment.Author.LastName);
                }

                taskComments.Add(new CommentModel(taskComment.Id, taskComment.Text, taskComment.TaskId, taskComment.UserId, author));
            }
        }

        var model = new TodoTaskModel(
            entity.Id,
            entity.Title,
            entity.Description,
            entity.CreationDate,
            entity.DueDate,
            entity.StatusId,
            entity.OwnerUserId,
            entity.ListId,
            new UserModel(entity.OwnerUserId, entity.OwnerUser.FirstName, entity.OwnerUser.LastName),
            new StatusModel(entity.StatusId, entity.Status.StatusTitle),
            new ReadOnlyCollection<TagModel>(taskTags),
            new ReadOnlyCollection<CommentModel>(taskComments));

        return model;
    }

    /// <summary>
    /// Converts a Tag entity to a TagModel.
    /// </summary>
    /// <param name="entity">The tag entity.</param>
    /// <returns>The corresponding tag model.</returns>
    public static TagModel ToTagModel(Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new TagModel(entity.Id, entity.Label);
    }

    /// <summary>
    /// Converts a Comment entity to a CommentModel.
    /// </summary>
    /// <param name="entity">The comment entity.</param>
    /// <returns>The corresponding comment model.</returns>
    public static CommentModel ToCommentModel(Comment entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CommentModel(entity.Id, entity.Text, entity.TaskId, entity.UserId, new UserModel(entity.Author.Id, entity.Author.FirstName, entity.Author.LastName));
    }

    /// <summary>
    /// Converts a TodoList entity to a TodoListModel, determining the user's role in the list.
    /// </summary>
    /// <param name="entity">The list entity.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>The corresponding list model.</returns>
    public static TodoListModel ToListModel(TodoList entity, int userId)
    {
        ArgumentNullException.ThrowIfNull(entity);

        string? userRole = null;

        if (entity.OwnerId == userId)
        {
            userRole = "Owner";
        }
        else
        {
            var role = entity.TodoListUserRoles.FirstOrDefault(r => r.UserId == userId);
            if (role != null)
            {
                userRole = role.ListRole.RoleName;
            }
            else
            {
                userRole = "No role";
            }
        }

        var activeTasks = 0;
        if (entity.TodoTasks != null)
        {
            activeTasks = (from task in entity.TodoTasks
                           where task.Status != null && task.Status.StatusTitle != "Completed"
                           select task).Count();
        }

        string userLastName = string.Empty;
        if (entity.ListOwner != null && !string.IsNullOrEmpty(entity.ListOwner.LastName))
        {
            userLastName = $" {entity.ListOwner.LastName[0]}.";
        }

        var userFullName = (entity.ListOwner is null) ?
            null :
            string.Concat(entity.ListOwner.FirstName, userLastName);

        return new TodoListModel(entity.Id, entity.OwnerId, entity.Title, entity.Description, activeTasks, userFullName, userRole);
    }
}

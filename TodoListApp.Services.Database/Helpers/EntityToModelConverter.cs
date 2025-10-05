using System.Collections.ObjectModel;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Helpers;
public static class EntityToModelConverter
{
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

        var taskCommetns = new List<CommentModel>();
        if (entity.Comments != null)
        {
            foreach (var taskComment in entity.Comments)
            {
                taskCommetns.Add(new CommentModel(taskComment.Id, taskComment.Text, taskComment.TaskId, taskComment.UserId));
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
            new ReadOnlyCollection<CommentModel>(taskCommetns));

        return model;
    }

    public static TagModel ToTagModel(Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new TagModel(entity.Id, entity.Label);
    }
}

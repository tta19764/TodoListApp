using System.Collections.ObjectModel;
using TodoListApp.Services.Models;
using TodoListApp.WebApi.Models.Dtos.Read;

namespace TodoListApp.Services.WebApi.Helpers;
internal static class MapToModel
{
    public static TodoTaskModel MapToTodoTaskModel(TodoTaskDto dto)
    {
        return new TodoTaskModel(
            id: dto.Id,
            title: dto.Title,
            description: dto.Description ?? string.Empty,
            creationDate: dto.CreationDate,
            dueDate: dto.DueDate,
            statusId: 0,
            ownerUserId: dto.AssigneeId,
            listId: dto.ListId,
            owner: new UserModel(dto.AssigneeId, dto.AssigneeFirstName, dto.AssigneeLastName),
            tags: new ReadOnlyCollection<TagModel>(dto.Tags.Select(t => new TagModel(t.Id, t.Title)).ToList()),
            status: new StatusModel(0, dto.Status));
    }
}

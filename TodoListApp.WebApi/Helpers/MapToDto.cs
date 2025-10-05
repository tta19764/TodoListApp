using System.Collections.ObjectModel;
using TodoListApp.Services.Models;
using TodoListApp.WebApi.Models.Dtos.Read;

namespace TodoListApp.WebApi.Helpers;

internal static class MapToDto
{
    public static TodoTaskDto ToTodoTaskDto(TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return new TodoTaskDto(
            model.Id,
            model.Title,
            model.Description,
            model.CreationDate ?? DateTime.MinValue,
            model.DueDate,
            model.OwnerUser!.FirstName,
            model.OwnerUser!.LastName,
            model.OwnerUser!.Id,
            model.Status?.StatusTitle ?? "Unknown",
            model.ListId,
            new ReadOnlyCollection<TagDto>(model.UsersTags.Select(ut => new TagDto() { Id = ut.Id, Title = ut.Title }).ToList()),
            new ReadOnlyCollection<string>(model.UserComments.Select(uc => uc.Text).ToList()));
    }

    public static TagDto ToTagDto(TagModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return new TagDto
        {
            Id = model.Id,
            Title = model.Title,
        };
    }
}

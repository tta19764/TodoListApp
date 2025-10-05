using TodoListApp.Services.Enums;

namespace TodoListApp.WebApp.Models.Task;

public class TodoTasksListViewModel
{
    public IEnumerable<TodoTaskViewModel> Tasks { get; set; } = Enumerable.Empty<TodoTaskViewModel>();

    public ListRole Role { get; set; } = ListRole.None;
}

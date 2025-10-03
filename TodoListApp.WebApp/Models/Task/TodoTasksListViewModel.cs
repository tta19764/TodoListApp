namespace TodoListApp.WebApp.Models.Task;

public class TodoTasksListViewModel
{
    public IEnumerable<TodoTaskViewModel> Tasks { get; set; } = Enumerable.Empty<TodoTaskViewModel>();

    public string RoleName { get; set; } = string.Empty;
}

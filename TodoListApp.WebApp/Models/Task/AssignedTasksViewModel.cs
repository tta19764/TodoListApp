namespace TodoListApp.WebApp.Models.Task;

public class AssignedTasksViewModel
{
    public IEnumerable<TodoTaskViewModel> Tasks { get; set; } = Enumerable.Empty<TodoTaskViewModel>();

    public string TaskFilter { get; set; } = "active";

    public string SortBy { get; set; } = "Id";

    public string SortOrder { get; set; } = "asc";

    public int TotalTasks { get; set; }
}

namespace TodoListApp.WebApp.Models.Task;

public class AssignedTasksViewModel
{
    public IEnumerable<TodoTaskViewModel> Tasks { get; set; } = Enumerable.Empty<TodoTaskViewModel>();

    public string TaskFilter { get; set; } = "active";

    public string SortBy { get; set; } = "Id";

    public string SortOrder { get; set; } = "asc";


    /// <summary>
    /// Gets or sets the task page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of rows to display per task page.
    /// </summary>
    public int RowCount { get; set; } = 10;

    /// <summary>
    /// Gets or sets the total number of task pages available based on the current filters and pagination settings.
    /// </summary>
    public int TotalPages { get; set; }

    public int TotalTasks { get; set; }
}

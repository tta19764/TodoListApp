namespace TodoListApp.WebApp.Models.Task;

public class SearchTasksViewModel
{
    /// <summary>
    /// Gets or sets the collection of tasks to display.
    /// </summary>
    public IEnumerable<TodoTaskViewModel> Tasks { get; set; } = Enumerable.Empty<TodoTaskViewModel>();

    /// <summary>
    /// Gets or sets the title to filter tasks by.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the creation date to filter tasks by.
    /// </summary>
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the due date to filter tasks by.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Gets or sets the task page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of rows to display per task page.
    /// </summary>
    public int RowCount { get; set; } = 10;

    /// <summary>
    /// Gets or sets the total number of task pages available based on the current search criteria and pagination settings.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the total number of tasks matching the current search criteria.
    /// </summary>
    public int TotalTasks { get; set; }
}

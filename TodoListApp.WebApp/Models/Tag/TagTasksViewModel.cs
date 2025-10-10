using TodoListApp.WebApp.Models.Task;

namespace TodoListApp.WebApp.Models.Tag;

/// <summary>
/// ViewModel for displaying tasks associated with a specific tag, including pagination details.
/// </summary>
public class TagTasksViewModel
{
    /// <summary>
    /// Gets or sets the identifier of the tag whose tasks are being displayed.
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// Gets or sets the title of the tag whose tasks are being displayed.
    /// </summary>
    public string TagTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of tasks associated with the tag to display.
    /// </summary>
    public IEnumerable<TodoTaskViewModel> Tasks { get; set; } = Enumerable.Empty<TodoTaskViewModel>();

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

    /// <summary>
    /// Gets or sets the total number of tasks matching the current filters.
    /// </summary>
    public int TotalTasks { get; set; }

    /// <summary>
    /// Gets or sets the return URL to navigate back after adding the tag.
    /// </summary>
    public Uri ReturnUrl { get; set; } = new Uri("~/", UriKind.Relative);
}

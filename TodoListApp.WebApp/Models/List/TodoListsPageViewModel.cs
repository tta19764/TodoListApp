using TodoListApp.WebApp.Models.Task;

namespace TodoListApp.WebApp.Models.List;

/// <summary>
/// View model for the main To-do Lists page with two panels.
/// </summary>
public class TodoListsPageViewModel
{
    /// <summary>
    /// Gets or sets the collection of to-do lists to display in the left panel.
    /// </summary>
    public IEnumerable<TodoListViewModel> TodoLists { get; set; } = new List<TodoListViewModel>();

    /// <summary>
    /// Gets or sets the current page number for list pagination.
    /// </summary>
    public int ListPageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of rows to display per list page.
    /// </summary>
    public int ListRowCount { get; set; } = 5;

    /// <summary>
    /// Gets or sets the total number of list pages available based on the current filters and pagination settings.
    /// </summary>
    public int ListTotalPages { get; set; }

    /// <summary>
    /// Gets or sets the currently selected list ID.
    /// </summary>
    public int? SelectedListId { get; set; }

    /// <summary>
    /// Gets or sets the selected list details.
    /// </summary>
    public TodoListViewModel? SelectedList { get; set; }

    /// <summary>
    /// Gets or sets the tasks for the selected list.
    /// </summary>
    public IEnumerable<TodoTaskViewModel> Tasks { get; set; } = new List<TodoTaskViewModel>();

    /// <summary>
    /// Gets or sets the list filter (all, owned, shared).
    /// </summary>
    public string? ListFilter { get; set; }

    /// <summary>
    /// Gets or sets the task filter (active, notstarted, inprogress, completed, all).
    /// </summary>
    public string? TaskFilter { get; set; }

    /// <summary>
    /// Gets or sets the task page number.
    /// </summary>
    public int TaskPageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of rows to display per task page.
    /// </summary>
    public int TaskRowCount { get; set; } = 10;

    /// <summary>
    /// Gets or sets the total number of task pages available based on the current filters and pagination settings.
    /// </summary>
    public int TaskTotalPages { get; set; }
}

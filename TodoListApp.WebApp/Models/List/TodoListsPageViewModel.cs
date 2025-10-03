using TodoListApp.WebApp.Models.Task;

namespace TodoListApp.WebApp.Models.List;

/// <summary>
/// View model for the main Todo Lists page with two panels.
/// </summary>
public class TodoListsPageViewModel
{
    /// <summary>
    /// Gets or sets the collection of todo lists to display in the left panel.
    /// </summary>
    public IEnumerable<TodoListViewModel> TodoLists { get; set; } = new List<TodoListViewModel>();

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
}

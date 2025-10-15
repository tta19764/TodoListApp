using TodoListApp.Services.Enums;

namespace TodoListApp.WebApp.Models.Task;

/// <summary>
/// View model representing a list of to-do tasks along with the user's role in the list.
/// </summary>
public class TodoTasksListViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the to-do list.
    /// </summary>
    public int ListId { get; set; }

    /// <summary>
    /// Gets or sets the collection of to-do tasks.
    /// </summary>
    public IEnumerable<TodoTaskViewModel> Tasks { get; set; } = Enumerable.Empty<TodoTaskViewModel>();

    /// <summary>
    /// Gets or sets the unique identifier of the list.
    /// </summary>
    public ListRole Role { get; set; } = ListRole.None;
}

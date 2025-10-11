namespace TodoListApp.WebApi.Models.Dtos.Create;

/// <summary>
/// Data transfer object for creating a to-do task.
/// </summary>
public class CreateTodoTaskDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTodoTaskDto"/> class.
    /// </summary>
    public CreateTodoTaskDto()
    {
    }

    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date of task creation.
    /// </summary>
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the task due date.
    /// </summary>
    public DateTime DueDate { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets or sets the unique identifier of the task status.
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the task assignee.
    /// </summary>
    public int AssigneeId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the list.
    /// </summary>
    public int ListId { get; set; }
}

namespace TodoListApp.WebApi.Models.Dtos.Update;

/// <summary>
/// Data Transfer Object for updating a to-do task.
/// </summary>
public class UpdateTodoTaskDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTodoTaskDto"/> class.
    /// </summary>
    public UpdateTodoTaskDto()
    {
    }

    /// <summary>
    /// Gets or sets the task id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string? Description { get; set; }

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

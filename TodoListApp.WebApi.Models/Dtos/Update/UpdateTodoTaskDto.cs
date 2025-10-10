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
    /// Initializes a new instance of the <see cref="UpdateTodoTaskDto"/> class.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="title">The task title.</param>
    /// <param name="description">The task description.</param>
    /// <param name="dueDate">The task due date.</param>
    /// <param name="statusId">The task status ID.</param>
    /// <param name="assigneeId">The task assignee ID.</param>
    /// <param name="listId">The task list ID.</param>
    public UpdateTodoTaskDto(int id, string title, string? description, DateTime dueDate, int statusId, int assigneeId, int listId)
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this.DueDate = dueDate;
        this.StatusId = statusId;
        this.AssigneeId = assigneeId;
        this.ListId = listId;
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

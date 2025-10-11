using System.Text.Json.Serialization;

namespace TodoListApp.WebApi.Models.Dtos.Read;

/// <summary>
/// Data transfer object for reading task information.
/// </summary>
public class TodoTaskDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTaskDto"/> class.
    /// </summary>
    public TodoTaskDto()
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
    /// Gets or sets the date of task creation.
    /// </summary>
    public DateTime CreationDate { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets or sets the task due date.
    /// </summary>
    public DateTime DueDate { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets or sets the assigne first name.
    /// </summary>
    public string AssigneeFirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the assigne last name.
    /// </summary>
    public string AssigneeLastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the task assignee.
    /// </summary>
    public int AssigneeId { get; set; }

    /// <summary>
    /// Gets or sets the task status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the list containing the task.
    /// </summary>
    public int ListId { get; set; }

    /// <summary>
    /// Gets or sets the name of the list containing the task.
    /// </summary>
    [JsonInclude]
    public IEnumerable<TagDto> Tags { get; set; } = new List<TagDto>();
}

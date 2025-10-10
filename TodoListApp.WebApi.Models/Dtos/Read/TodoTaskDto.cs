using System.Collections.ObjectModel;
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
    /// Initializes a new instance of the <see cref="TodoTaskDto"/> class.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="title">The task title.</param>
    /// <param name="description">The task description.</param>
    /// <param name="creationDate">The task creation date.</param>
    /// <param name="dueDate">The task due date.</param>
    /// <param name="assigneeFirstName">The task assignee first name.</param>
    /// <param name="assigneeLastName">The task assignee last name.</param>
    /// <param name="assigneeId">The task assignee ID.</param>
    /// <param name="status">The task status.</param>
    /// <param name="listId">The task list ID.</param>
    /// <param name="tags">The list of task tags.</param>
    public TodoTaskDto(int id, string title, string? description, DateTime creationDate, DateTime dueDate, string assigneeFirstName, string assigneeLastName, int assigneeId, string status, int listId, ReadOnlyCollection<TagDto>? tags = null)
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this.CreationDate = creationDate;
        this.DueDate = dueDate;
        this.AssigneeFirstName = assigneeFirstName;
        this.AssigneeLastName = assigneeLastName;
        this.AssigneeId = assigneeId;
        this.Status = status;
        this.ListId = listId;
        if (tags != null)
        {
            foreach (var tag in tags)
            {
                this.Tags.Add(tag);
            }
        }
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
    /// Gets user's tags.
    /// </summary>
    [JsonInclude]
    public IList<TagDto> Tags { get; private set; } = new List<TagDto>();
}

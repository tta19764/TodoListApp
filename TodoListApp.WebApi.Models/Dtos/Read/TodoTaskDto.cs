using System.Collections.ObjectModel;

namespace TodoListApp.WebApi.Models.Dtos.Read;
public class TodoTaskDto
{
    public TodoTaskDto()
    {
    }

    public TodoTaskDto(int id, string title, string? description, DateTime creationDate, DateTime dueDate, string assigneeFirstName, string assigneeLastName, int assigneeId, string status, int listId, ReadOnlyCollection<string>? tags = null, ReadOnlyCollection<string>? comments = null)
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

        if (comments != null)
        {
            foreach (var comment in comments)
            {
                this.Comments.Add(comment);
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
    public IList<string> Tags { get; private set; } = new List<string>();

    /// <summary>
    /// Gets user's tags.
    /// </summary>
    public IList<string> Comments { get; private set; } = new List<string>();
}

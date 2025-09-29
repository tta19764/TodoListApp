using System.Collections.ObjectModel;

namespace TodoListApp.WebApi.Models.Dtos.Read;
public class TodoTaskDto
{
    public TodoTaskDto(int id, string title, string? description, DateTime creationDate, DateTime dueDate, string ownerUserName, string status, ReadOnlyCollection<string>? tags = null, ReadOnlyCollection<string>? comments = null)
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this.CreationDate = creationDate;
        this.DueDate = dueDate;
        this.OwnerUserName = ownerUserName;
        this.Status = status;
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
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date of task creation.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the task due date.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the task owner.
    /// </summary>
    public string OwnerUserName { get; set; }

    /// <summary>
    /// Gets or sets the task status.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Gets user's tags.
    /// </summary>
    public IList<string> Tags { get; private set; } = new List<string>();

    /// <summary>
    /// Gets user's tags.
    /// </summary>
    public IList<string> Comments { get; private set; } = new List<string>();
}

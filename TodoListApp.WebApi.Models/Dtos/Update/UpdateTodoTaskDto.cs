namespace TodoListApp.WebApi.Models.Dtos.Update;
public class UpdateTodoTaskDto
{
    public UpdateTodoTaskDto(int id, string title, string? description, DateTime dueDate, int statusId, int ownerUserId, int listId)
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this.DueDate = dueDate;
        this.StatusId = statusId;
        this.OwnerUserId = ownerUserId;
        this.ListId = listId;
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
    /// Gets or sets the task due date.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the task status.
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the task owner.
    /// </summary>
    public int OwnerUserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the list.
    /// </summary>
    public int ListId { get; set; }
}

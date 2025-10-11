namespace TodoListApp.WebApi.Models.Dtos.Update;

/// <summary>
/// Data Transfer Object for updating the status of a task.
/// </summary>
public class UpdateStatusDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStatusDto"/> class.
    /// </summary>
    public UpdateStatusDto()
    {
    }

    /// <summary>
    /// Gets or sets the unique identifier of the status.
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    public int TaskId { get; set; }
}

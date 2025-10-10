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
    /// Initializes a new instance of the <see cref="UpdateStatusDto"/> class.
    /// </summary>
    /// <param name="statusId">The unique identifier of the staus.</param>
    /// <param name="taskId">The unique identifier of the task.</param>
    public UpdateStatusDto(int statusId, int taskId)
    {
        this.StatusId = statusId;
        this.TaskId = taskId;
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

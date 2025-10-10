namespace TodoListApp.Services.WebApi.Enums;

/// <summary>
/// Enumeration representing the status of a to-do task.
/// </summary>
public enum TodoTaskStatus
{
    /// <summary>
    /// Indicates that the task has no status.
    /// </summary>
    None = 0,

    /// <summary>
    /// Indicates that the task has not been started.
    /// </summary>
    NotStarted = 1,

    /// <summary>
    /// Indicates that the task is currently in progress.
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// Indicates that the task has been completed.
    /// </summary>
    Completed = 3,
}

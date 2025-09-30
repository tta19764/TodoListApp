namespace TodoListApp.Services.Enums;

/// <summary>
/// Defines the filter options for tasks.
/// </summary>
public enum TaskFilter
{
    /// <summary>
    /// The tasks that are not completed.
    /// </summary>
    Active,

    /// <summary>
    /// The tasks that are in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// The tasks that are not started.
    /// </summary>
    NotStarted,

    /// <summary>
    /// The tasks that are completed.
    /// </summary>
    Completed,

    /// <summary>
    /// All tasks regardless of their status.
    /// </summary>
    All,
}

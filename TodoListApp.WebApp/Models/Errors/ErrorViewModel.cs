namespace TodoListApp.WebApp.Models.Errors;

/// <summary>
/// View model for error information.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Gets or sets the request ID.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether to show the RequestId.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);

    /// <summary>
    /// Gets or sets the error code for identification.
    /// </summary>
    public string ErrorCode { get; set; } = "ERR_UNKNOWN";

    /// <summary>
    /// Gets or sets user-friendly error message.
    /// </summary>
    public string ErrorMessage { get; set; } = "An unexpected error occurred.";

    /// <summary>
    /// Gets or sets the detailed error information (only in development).
    /// </summary>
    public string? ErrorDetails { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code.
    /// </summary>
    public int StatusCode { get; set; } = 500;

    /// <summary>
    /// Gets or sets the timestamp when error occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets a value indicating whether to show detailed error information.
    /// </summary>
    public bool ShowDetails => !string.IsNullOrEmpty(this.ErrorDetails);
}

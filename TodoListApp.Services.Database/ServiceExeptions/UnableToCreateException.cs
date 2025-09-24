namespace TodoListApp.Services.Database.ServiceExeptions;

/// <summary>
/// Represents an error that occurs when an entity cannot be created in the database.
/// </summary>
public class UnableToCreateException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToCreateException"/> class.
    /// </summary>
    public UnableToCreateException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToCreateException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public UnableToCreateException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToCreateException"/> class
    /// with a specified error message and a reference to the inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that caused this exception.</param>
    public UnableToCreateException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToCreateException"/> class
    /// with the entity name and key that caused the failure.
    /// </summary>
    /// <param name="entityName">The name of the entity.</param>
    /// <param name="key">The key of the entity (if known).</param>
    public UnableToCreateException(string entityName, Exception innerException, object? key = null)
        : base(
            key is null
            ? $"{entityName} could not be created."
            : $"{entityName} with key {key} could not be created.", innerException)
    {
        this.EntityName = entityName;
        this.Key = key;
    }

    /// <summary>
    /// Gets the name of the entity that caused the exception.
    /// </summary>
    public string? EntityName { get; }

    /// <summary>
    /// Gets the key of the entity that caused the exception, if available.
    /// </summary>
    public object? Key { get; }
}

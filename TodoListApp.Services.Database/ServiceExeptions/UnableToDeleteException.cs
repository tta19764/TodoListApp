using System.Runtime.Serialization;

namespace TodoListApp.Services.Database.ServiceExeptions;

/// <summary>
/// Represents an error that occurs when an entity cannot be deleted.
/// </summary>
[Serializable]
public class UnableToDeleteException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToDeleteException"/> class.
    /// </summary>
    public UnableToDeleteException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToDeleteException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public UnableToDeleteException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToDeleteException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public UnableToDeleteException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToDeleteException"/> class for a specific entity.
    /// </summary>
    /// <param name="entityName">The name of the entity that could not be deleted.</param>
    /// <param name="key">The key of the entity that could not be deleted.</param>
    public UnableToDeleteException(string entityName, object key)
        : base($"Unable to delete {entityName} with key {key}.")
    {
        this.EntityName = entityName;
        this.Key = key;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToDeleteException"/> class for a specific entity and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    /// <param name="entityName">The name of the entity that could not be deleted.</param>
    /// <param name="key">The key of the entity that could not be deleted.</param>
    public UnableToDeleteException(string entityName, object key, Exception innerException)
        : base($"Unable to delete {entityName} with key {key}.", innerException)
    {
        this.EntityName = entityName;
        this.Key = key;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToDeleteException"/> class with serialized data.
    /// </summary>
    protected UnableToDeleteException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        this.EntityName = info.GetString(nameof(this.EntityName));
        this.Key = info.GetValue(nameof(this.Key), typeof(object));
    }

    /// <summary>
    /// Gets the name of the entity that could not be deleted.
    /// </summary>
    public string? EntityName { get; }

    /// <summary>
    /// Gets the key of the entity that could not be deleted.
    /// </summary>
    public object? Key { get; }

    /// <inheritdoc/>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);

        info.AddValue(nameof(this.EntityName), this.EntityName);
        info.AddValue(nameof(this.Key), this.Key);
    }
}

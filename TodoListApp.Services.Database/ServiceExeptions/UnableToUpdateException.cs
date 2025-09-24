using System.Runtime.Serialization;

namespace TodoListApp.Services.Database.ServiceExeptions;

/// <summary>
/// Represents an error that occurs when an entity cannot be updated in the database.
/// </summary>
[Serializable]
public class UnableToUpdateException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToUpdateException"/> class.
    /// </summary>
    public UnableToUpdateException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToUpdateException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public UnableToUpdateException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToUpdateException"/> class
    /// with a specified error message and a reference to the inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that caused this exception.</param>
    public UnableToUpdateException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToUpdateException"/> class
    /// with the entity name and key that caused the failure.
    /// </summary>
    /// <param name="entityName">The name of the entity.</param>
    /// <param name="key">The key of the entity.</param>
    public UnableToUpdateException(string entityName, object key)
        : base($"{entityName} with key {key} could not be updated.")
    {
        this.EntityName = entityName;
        this.Key = key;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToUpdateException"/> class
    /// with the entity name and key that caused the failure and a reference to the inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that caused this exception.</param>
    public UnableToUpdateException(string entityName, object key, Exception innerException)
        : base($"{entityName} with key {key} could not be updated.", innerException)
    {
        this.EntityName = entityName;
        this.Key = key;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnableToUpdateException"/> class with serialized data.
    /// </summary>
    protected UnableToUpdateException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        this.EntityName = info.GetString(nameof(this.EntityName));
        this.Key = info.GetValue(nameof(this.Key), typeof(object));
    }

    /// <summary>
    /// Gets the name of the entity that caused the exception.
    /// </summary>
    public string? EntityName { get; }

    /// <summary>
    /// Gets the key of the entity that caused the exception.
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

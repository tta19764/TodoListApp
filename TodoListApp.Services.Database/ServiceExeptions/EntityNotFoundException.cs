using System.Runtime.Serialization;

namespace TodoListApp.Services.Database.ServiceExeptions;

/// <summary>
/// Represents errors that occur when an entity cannot be found in the data store.
/// </summary>
[Serializable]
public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
    /// </summary>
    public EntityNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class
    /// with a specified error message and a reference to the inner exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class
    /// with the name of the entity type and its key.
    /// </summary>
    /// <param name="entityName">The type name of the entity.</param>
    /// <param name="key">The key value of the entity that was not found.</param>
    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found.")
    {
        this.EntityName = entityName;
        this.Key = key;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with serialized data.
    /// </summary>
    protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
    {
        this.EntityName = info.GetString(nameof(this.EntityName));
        this.Key = info.GetValue(nameof(this.Key), typeof(object));
    }

    /// <summary>
    /// Gets the name of the entity type that was not found.
    /// </summary>
    public string? EntityName { get; }

    /// <summary>
    /// Gets the key value of the entity that was not found.
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

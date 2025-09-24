using System.Runtime.Serialization;

namespace TodoListApp.Services.Database.ServiceExeptions;

/// <summary>
/// Represents errors that occur when attempting to create an entity
/// with an identifier that already exists in the data store.
/// </summary>
[Serializable]
public class EntityWithIdExistsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityWithIdExistsException"/> class.
    /// </summary>
    public EntityWithIdExistsException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityWithIdExistsException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EntityWithIdExistsException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityWithIdExistsException"/> class
    /// with a specified error message and a reference to the inner exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public EntityWithIdExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityWithIdExistsException"/> class
    /// with the name of the entity type and the conflicting key.
    /// </summary>
    /// <param name="entityName">The type name of the entity.</param>
    /// <param name="key">The key value of the entity that already exists.</param>
    public EntityWithIdExistsException(string entityName, object key)
        : base($"{entityName} with key '{key}' already exists.")
    {
        this.EntityName = entityName;
        this.Key = key;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityWithIdExistsException"/> class with serialized data.
    /// </summary>
    protected EntityWithIdExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        this.EntityName = info.GetString(nameof(this.EntityName));
        this.Key = info.GetValue(nameof(this.Key), typeof(object));
    }

    /// <summary>
    /// Gets the name of the entity type that already exists.
    /// </summary>
    public string? EntityName { get; }

    /// <summary>
    /// Gets the key value of the entity that already exists.
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

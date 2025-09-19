namespace TodoListApp.Services.Database.Entities;

/// <summary>
/// Represents the base class for all entities in the data access layer.
/// This class provides a common structure for entities, including an identifier property.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEntity"/> class with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the entity.</param>
    protected BaseEntity(int id)
    {
        this.Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEntity"/> class with a default identifier of 0.
    /// </summary>
    protected BaseEntity()
    {
        this.Id = 0;
    }

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }
}

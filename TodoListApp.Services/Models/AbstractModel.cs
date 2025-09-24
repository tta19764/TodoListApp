namespace TodoListApp.Services.Models;

/// <summary>
/// AbstractModel class serves as a base class for models with an ID property.
/// </summary>
public abstract class AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractModel"/> class with an ID property.
    /// </summary>
    /// <param name="id">The unique identifier for the model.</param>
    protected AbstractModel(int id)
    {
        this.Id = id;
    }

    /// <summary>
    /// Gets or sets the unique identifier for the model.
    /// </summary>
    public int Id { get; set; }
}

namespace TodoListApp.Services.Models;

/// <summary>
/// Model representing a user.
/// </summary>
public class UserModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserModel"/> class.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="firstName">The user first name.</param>
    /// <param name="lastName">The user last name.</param>
    public UserModel(int id, string firstName, string lastName)
        : base(id)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    public string LastName { get; set; }
}

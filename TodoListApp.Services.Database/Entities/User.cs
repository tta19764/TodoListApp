using Microsoft.AspNetCore.Identity;

namespace TodoListApp.Services.Database.Entities;

/// <summary>
/// Represents a user entity in the database.
/// </summary>
public class User : IdentityUser<int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    public User()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class with parameters.
    /// </summary>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="userName">The user's username.</param>
    public User(string firstName, string lastName, string userName)
        : base(userName)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class with parameters.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="userName">The user's username.</param>
    /// <param name="passwordHash">The user's password.</param>
    public User(int id, string firstName, string lastName, string userName, string passwordHash)
        : base(userName)
    {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.PasswordHash = passwordHash;
    }

    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets the list with user's tags.
    /// </summary>
    public virtual IList<Tag> UserTags { get; private set; } = new List<Tag>();

    /// <summary>
    /// Gets the list with list user roles.
    /// </summary>
    public virtual IList<TodoListUserRole> TodoListUserRoles { get; private set; } = new List<TodoListUserRole>();

    /// <summary>
    /// Gets the list with user comments.
    /// </summary>
    public virtual IList<Comment> Comments { get; private set; } = new List<Comment>();

    /// <summary>
    /// Gets the list with the lists that the user owns.
    /// </summary>
    public virtual IList<TodoList> Lists { get; private set; } = new List<TodoList>();

    /// <summary>
    /// Gets the list with tasks that are assigned to the user.
    /// </summary>
    public virtual IList<TodoTask> Tasks { get; private set; } = new List<TodoTask>();
}

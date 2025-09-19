using Microsoft.AspNetCore.Identity;

namespace TodoListApp.WebApp.Data;

/// <summary>
/// Represents a user entity in the database.
/// </summary>
public class AppUser : IdentityUser<int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppUser"/> class.
    /// </summary>
    public AppUser()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppUser"/> class with parameters.
    /// </summary>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="userName">The user's username.</param>
    public AppUser(string firstName, string lastName, string userName)
        : base(userName)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppUser"/> class with parameters.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="userName">The user's username.</param>
    /// <param name="passwordHash">The user's password.</param>
    public AppUser(int id, string firstName, string lastName, string userName, string passwordHash)
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
}

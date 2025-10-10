using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models.Account;

/// <summary>
/// View model for the registration form.
/// </summary>
public class RegisterViewModel
{
    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    [Required(ErrorMessage = "Last Name is required.")]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user login (username).
    /// </summary>
    [Required(ErrorMessage = "Login is required.")]
    public string Login { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user password.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} characters long.")]
    [DataType(DataType.Password)]
    [Compare("ConfirmPassword", ErrorMessage = "Password does not match.")]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password confirmation.
    /// </summary>
    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = null!;

    /// <summary>
    /// Gets or sets the return URL after a successful registration.
    /// </summary>
    public Uri ReturnUrl { get; set; } = new Uri("/", UriKind.Relative);
}

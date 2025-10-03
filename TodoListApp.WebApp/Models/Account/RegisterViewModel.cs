using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last Name is required.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Login is required.")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} characters long.")]
    [DataType(DataType.Password)]
    [Compare("ConfirmPassword", ErrorMessage = "Password does not match.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = null!;

    public Uri ReturnUrl { get; set; } = new Uri("/", UriKind.Relative);
}

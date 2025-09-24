using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} characters long.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("ConfirmNewPassword", ErrorMessage = "Password does not match.")]
    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    public string ConfirmNewPassword { get; set; } = null!;
}

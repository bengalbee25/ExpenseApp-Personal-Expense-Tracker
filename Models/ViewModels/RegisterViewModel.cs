using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Web.Models.ViewModels;

public class RegisterViewModel
{
    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}

using System.ComponentModel.DataAnnotations;

namespace WinterSportAcademy.Models;
public class AuthModel//shift + alt + F
{
    [Required(ErrorMessage = "Email required")]
    [EmailAddress(ErrorMessage = "Not correct Email")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Minimum 6 simbols length")]
    public string Password { get; set; } = string.Empty;
}
using System.ComponentModel.DataAnnotations;

namespace WinterSportAcademy.Models;

public class InstructorDto
{
    public int InstructorId { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(Skiing|Snowboarding|Skating|Hockey)$")]
    public string Specialisation { get; set; } = string.Empty;
}
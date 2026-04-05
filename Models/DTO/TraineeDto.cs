using System.ComponentModel.DataAnnotations;

namespace WinterSportAcademy.Models;

public class TraineeDto
{
    public int TraineeId { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [RegularExpression("^(Beginner|Intermediate|Advanced)$")]
    public string SkillLevel { get; set; } = string.Empty;
}
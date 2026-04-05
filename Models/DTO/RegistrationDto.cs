using System.ComponentModel.DataAnnotations;

namespace WinterSportAcademy.Models;

public class RegistrationDto
{
    public int RegistrationNumber { get; set; }

    [Required]
    public int TraineeId { get; set; }

    [Required]
    public int TrainingSessionId { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime RegistrationTime { get; set; } = DateTime.UtcNow;

    public bool IsConfirmed { get; set; }
}

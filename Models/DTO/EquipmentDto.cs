using System.ComponentModel.DataAnnotations;

namespace WinterSportAcademy.Models;

public class EquipmentDto
{
    public int EquipmentId { get; set; }

    [Required(ErrorMessage = "This field can'e be null!")]
    [StringLength(50)]
    public string ItemName { get; set; } = string.Empty;

    [Required]
    public string ItemCategory { get; set; } = string.Empty;

    public string? Specification { get; set; }
    public string? Size { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? StartTime { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? EndTime { get; set; }

    public int? TraineeId { get; set; }
    public int? TrainingSessionId { get; set; }
}

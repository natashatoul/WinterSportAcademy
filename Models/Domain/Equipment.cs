using System.ComponentModel.DataAnnotations;

namespace WinterSportAcademy.Models;
public class Equipment //one-to-many
{
    public int  EquipmentId {get; set;}
    [Required(ErrorMessage = "This field can'e be null!")]
    [StringLength(50)]
    public String ItemName {get; set;} = string.Empty;// e.g., "Salomon S-Max"
    [Required]
    public String ItemCategory {get; set;} = string.Empty;// e.g., "Mountain Skis", "Ice Skates"
    
    public string? Specification {get; set;} // e.g., "Female", "Professional", "Hockey"
    public string? Size {get; set;}// e.g., "175cm", "42", "L"
    [DataType(DataType.DateTime)]
    public DateTime? StartTime {get; set;}
    
    [DataType(DataType.DateTime)]
    public DateTime? EndTime {get; set;}
    public int? TraineeId {get; set;}//if is not null this equipment is not avalible
    public Trainee? Trainee {get; set;}// when I will add new sky DB don't give me to do it if I don't have"?"
    public int? TrainingSessionId { get; set; } 
    public TrainingSession? TrainingSession { get; set; }
        
} 

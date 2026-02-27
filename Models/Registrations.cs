using System.ComponentModel.DataAnnotations;

namespace WinterSportAcademy.Models;
public class Registration // link table many-to-many trainee and session
{
    [Key]// I didn/t used RegistrationId and I need to show for my program that it is a PK
    public int RegistrationNumber {get; set;}
    [Required]
    public int  TraineeId {get; set;} // link to trainee
    public Trainee? Trainee {get; set;}
    [Required]
    public int  TrainingSessionId {get; set;}// lint to training session
    public TrainingSession? TrainingSession {get; set;}
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime RegistrationTime {get; set;} = DateTime.UtcNow;
    public bool IsConfirmed {get; set;}

}
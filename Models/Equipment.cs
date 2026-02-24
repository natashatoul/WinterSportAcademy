namespace WinterSportAcademy.Models;
public class Equipment 
{
    public int  EquipmentId {get; set;}
    public String ItemName {get; set;} = string.Empty;
    public String ItemCategory {get; set;} = string.Empty;
    public DateTime StartTime {get; set;}
    public DateTime EndTime {get; set;}
    public int TraineeId {get; set;} 
    public Trainee? Trainee {get; set;}// when I will add new sky DB don't give me to do it if I don't have"?"
        
} 
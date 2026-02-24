namespace WinterSportAcademy.Models;
public class Trainee // one-to-many
{
    public int  TraineeId {get; set;}
    public String FirstName {get; set;} = string.Empty;
    public String StillLevel {get; set;} = string.Empty;
    public List<Registration> Registrations {get; set;} = new(); // one trainee has many sessions
    public List<Equipment> RentEquipment {get; set;} = new(); // one trainee has many rents


    
} 
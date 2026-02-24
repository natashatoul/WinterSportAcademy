using System.Text.Json.Serialization;

namespace WinterSportAcademy.Models;
public class Trainee // one-to-many
{
    public int  TraineeId {get; set;}
    public String FirstName {get; set;} = string.Empty;
    public String LastName {get; set;} = string.Empty;
    public String SkillLevel {get; set;} = string.Empty;
    public List<Registration> Registrations {get; set;} = new(); // one trainee has many sessions
    [JsonIgnore]
    public List<Equipment> RentEquipment {get; set;} = new(); // one trainee has many rents

} 
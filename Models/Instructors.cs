using System.Text.Json.Serialization;

namespace WinterSportAcademy.Models;
public class Instructor // one-to-many
{
    public int  InstructorId {get; set;}
    public String FirstName {get; set;} = string.Empty;
    public String LastName {get; set;} = string.Empty;
    public String Specialisation {get; set;} = string.Empty;
    [JsonIgnore]
    public List<TrainingSession> Sessions {get; set;} = new(); // one instructor has many sessions
    
} 

namespace WinterSportAcademy.Models;
public class TrainingSession // details about session and instractors
{
    public int  TrainingSessionId {get; set;}
    public String Title {get; set;} = string.Empty;
    public DateTime StartTime {get; set;}
    public int InstructorId {get; set;} 
    public Instructor? Instructor {get; set;}
    public List<Registration> Registrations {get; set;} = new(); // one instructor has many sessions

} 
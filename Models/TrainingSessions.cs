using System.Text.Json.Serialization;

namespace WinterSportAcademy.Models;
public class TrainingSession // details about session and instractors
{
    public int  TrainingSessionId {get; set;}
    public String Title {get; set;} = string.Empty;
    public DateTime StartTime {get; set;}
    public int InstructorId {get; set;} 
    public Instructor? Instructor {get; set;}

    [JsonIgnore]// нужен, чтобы предотвратить зацикливание (circular reference).
// Без него API будет пытаться бесконечно загружать связанные данные, и Postman зависнет.
// Prevents circular references and stops the API from crashing
// Added to fix the "infinite loop" error mentioned in the lecture.
    public List<Registration> Registrations {get; set;} = new(); // one instructor has many sessions

} 
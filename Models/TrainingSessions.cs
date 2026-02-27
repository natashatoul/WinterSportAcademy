using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WinterSportAcademy.Models;
public class TrainingSession // details about session and instractors
{
    [Key]
    public int  TrainingSessionId {get; set;}
    [Required(ErrorMessage = "This field can'e be null!")]
    [StringLength(200)]
    public String Title {get; set;} = string.Empty;
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime StartTime {get; set;}
    [Required]
    public int InstructorId {get; set;} 
    public Instructor? Instructor {get; set;}

    [JsonIgnore]// нужен, чтобы предотвратить зацикливание (circular reference).
// Без него API будет пытаться бесконечно загружать связанные данные, и Postman зависнет.
// Prevents circular references and stops the API from crashing
// Added to fix the "infinite loop" error mentioned in the lecture.
    public List<Registration> Registrations {get; set;} = new(); // one instructor has many sessions

} 
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WinterSportAcademy.Models;
public class Instructor // one-to-many
{
    public int  InstructorId {get; set;}
    
    [Required(ErrorMessage = "This field can'e be null!")]
    [StringLength(50)]
    public String FirstName {get; set;} = string.Empty;
    
    [Required(ErrorMessage = "This field can'e be null!")]
    [StringLength(50)]
    public String LastName {get; set;} = string.Empty;
    [Required]
    public String Specialisation {get; set;} = string.Empty;
    [JsonIgnore]
    public List<TrainingSession> Sessions {get; set;} = new(); // one instructor has many sessions
    
} 

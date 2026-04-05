using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WinterSportAcademy.Models;
public class Trainee // one-to-many
{
    public int  TraineeId {get; set;}
    [Required(ErrorMessage = "This field can'e be null!")]
    [StringLength(50)]
    public String FirstName {get; set;} = string.Empty;
    [Required(ErrorMessage = "This field can'e be null!")]
    [StringLength(50)]
    public String LastName {get; set;} = string.Empty;
    [Required]
    [RegularExpression("^(Beginner|Intermediate|Advanced)$")]
    public String SkillLevel {get; set;} = string.Empty;
    [JsonIgnore]
    public List<Registration> Registrations {get; set;} = new(); // one trainee has many sessions
    [JsonIgnore]
    public List<Equipment> RentEquipment {get; set;} = new(); // one trainee has many rents

} 
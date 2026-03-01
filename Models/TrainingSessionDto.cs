using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WinterSportAcademy.Models;

public class TrainingSessionDto
{
    public int TrainingSessionId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;

    public string FormattedStartTime { get; set; } = string.Empty;

    [JsonIgnore]
    public DateTime StartTime { get; set; }

    [Required]
    public int InstructorId { get; set; }
}
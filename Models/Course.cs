namespace WinterSportAcademy.Models;

public enum SportType // fix value (Sport = SportType.AlpineSkiing)
{
    AlpineSkiing, Snowbord, FigureSkating, CrossCountrySkiing, IceHockey
}

public class Course
{
    public int CourseID {get; set;}
    public string Name {get; set; } = string.Empty; // same like ""
    public SportType Sport {get; set;}
    public string? Description {get; set;} // can be null (nullable)
    public int MaxStudents {get; set;} = 10;

    public decimal Price {get; set;}
    public string SkillLevelRequired {get; set;} = "Beginner";
    public int InstructorID {get; set;}
    public Instructor Instructor {get; set;} = null!;// I promist it will have a value
    

}
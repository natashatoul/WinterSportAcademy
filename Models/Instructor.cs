
namespace WinterSportAcademy.Models;

public class Instructor
{
    public int InstructorID {get; set;}
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    public string Specialisation {get; set;} = string.Empty;// Beginner Snowbording - Group A
    public int YearsOfExperiance {get; set;} 
    public decimal HourlyRate {get; set;}
    public string UserID {get; set;} = string.Empty;
    public List<Schedule>? Schedules {get; set;}
    public List<Enrollment>? Enrollments {get; set;} 





}
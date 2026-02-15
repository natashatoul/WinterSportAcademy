namespace WinterSportAcademy.Models;

public class Schedule
{
    public int ScheduleID {get; set;}
    public int CourseID {get; set;}
    public DayOfWeek DayOfWeek {get; set;}
    public TimeSpan StartTime {get; set;}
    public TimeSpan EndTime {get; set;}
    public string Location {get; set;} = string.Empty;
    public DateTime StartDate {get; set;}
    public DateTime EndDate {get; set;}
    public Course Course {get; set;} = null!;
}
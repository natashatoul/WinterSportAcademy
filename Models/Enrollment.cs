namespace WinterSportAcademy.Models;
public class Enrollment
{
    public int EnrollmentID {get; set;}
    public int StudentID {get; set;}
    public int CourseID {get; set;}
    public DateTime EnrollmentDate {get; set;} = DateTime.UtcNow;
    public string Status  {get; set;} = "Active"; // Confirmed, Cancelled, Completed
    public string PaymentStatus {get; set;} = "Pending"; //FK from Application User (Member)
    public Student Student {get; set;} = null!;
    public Course Course {get; set;}= null!;
   
}
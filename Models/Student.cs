namespace WinterSportAcademy.Models;

public class Student
{
    public int StudentID {get; set;}
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    public DateTime DateOfBirth {get; set;}
    public string SkillLevel {get; set;} = "Beginner"; 
    public string UserID {get; set;} = string.Empty;
    public List<Enrollment>? Enrollments {get; set;} 
    public List<EquipmentRental>? EquipmentRentals {get; set;}

}


using Microsoft.EntityFrameworkCore;

namespace WinterSportAcademy.Models;
public class AcademyContext : DbContext
{
    public AcademyContext(DbContextOptions<AcademyContext> options) : base(options){}
    public DbSet<Instructor> Instructors {get; set;} 
    public DbSet<Student> Students {get; set;} 
    public DbSet<Course> Courses {get; set;}
    public DbSet<Enrollment> Enrollments {get; set;}
    public DbSet<Schedule> Schedules {get; set;}
    public DbSet<EquipmentRental> EquipmentRentals {get; set;}
    


}


using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Models;

namespace WinterSportAcademy.Data;
public class WinterSportAcademyContext : DbContext
{
    public WinterSportAcademyContext(DbContextOptions<WinterSportAcademyContext> options) : base(options)
    {
        
    }
    public DbSet<Trainee> Trainees {get; set;}
    public DbSet<Instructor> Instructors {get; set;}
    
    public DbSet<Registration> Registrations {get; set;}
     public DbSet<Equipment> Equipments {get; set;}
    
}
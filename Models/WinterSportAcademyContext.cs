
using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WinterSportAcademy.Data;
public class WinterSportAcademyContext : IdentityDbContext<IdentityUser>
{
    public WinterSportAcademyContext(DbContextOptions<WinterSportAcademyContext> options) : base(options)
    {
        
    }
    public DbSet<Trainee> Trainees {get; set;}
    public DbSet<Instructor> Instructors {get; set;}
    
    public DbSet<Registration> Registrations {get; set;}
    public DbSet<Equipment> Equipments {get; set;}
    public DbSet<WinterSportAcademy.Models.TrainingSession> TrainingSession { get; set; } = default!;
    
}
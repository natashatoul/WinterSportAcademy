
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // One instructor -> many training sessions
        modelBuilder.Entity<TrainingSession>()
            .HasOne(ts => ts.Instructor)
            .WithMany(i => i.Sessions)
            .HasForeignKey(ts => ts.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Many-to-many via link table: Trainee <-> TrainingSession through Registration
        modelBuilder.Entity<Registration>()
            .HasOne(r => r.Trainee)
            .WithMany(t => t.Registrations)
            .HasForeignKey(r => r.TraineeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Registration>()
            .HasOne(r => r.TrainingSession)
            .WithMany(ts => ts.Registrations)
            .HasForeignKey(r => r.TrainingSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Prevent duplicate registration of the same trainee in the same session
        modelBuilder.Entity<Registration>()
            .HasIndex(r => new { r.TraineeId, r.TrainingSessionId })
            .IsUnique();

        // One trainee -> many equipment rentals
        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Trainee)
            .WithMany(t => t.RentEquipment)
            .HasForeignKey(e => e.TraineeId)
            .OnDelete(DeleteBehavior.SetNull);

        // One training session -> many equipment records
        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.TrainingSession)
            .WithMany(ts => ts.Equipments)
            .HasForeignKey(e => e.TrainingSessionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

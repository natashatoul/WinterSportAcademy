using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Data;
using WinterSportAcademy.Models;

namespace WinterSportAcademy.Repositories;

public class RegistrationRepository
{
    private readonly WinterSportAcademyContext _context;

    public RegistrationRepository(WinterSportAcademyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Registration>> GetAllAsync() =>
        await _context.Registrations
            .Include(r => r.Trainee)
            .Include(r => r.TrainingSession)
            .ToListAsync();

    public async Task<Registration?> GetByIdAsync(int id) =>
        await _context.Registrations
            .Include(r => r.Trainee)
            .Include(r => r.TrainingSession)
            .FirstOrDefaultAsync(r => r.RegistrationNumber == id);

    public async Task AddAsync(Registration registration)
    {
        _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Registration registration)
    {
        _context.Entry(registration).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Registration registration)
    {
        _context.Registrations.Remove(registration);
        await _context.SaveChangesAsync();
    }

    public async Task<TrainingSession?> GetSessionByIdAsync(int sessionId) =>
        await _context.TrainingSession.FindAsync(sessionId);

    public async Task<bool> ExistsAsync(int id) =>
        await _context.Registrations.AnyAsync(r => r.RegistrationNumber == id);

    public async Task<bool> HasTraineeSessionConflictAsync(int traineeId, DateTime sessionStartTime, int? excludeRegistrationNumber = null)
    {
        return await _context.Registrations
            .Include(r => r.TrainingSession)
            .AnyAsync(r =>
                r.TraineeId == traineeId &&
                r.TrainingSession != null &&
                r.TrainingSession.StartTime == sessionStartTime &&
                (excludeRegistrationNumber == null || r.RegistrationNumber != excludeRegistrationNumber.Value));
    }
}

using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Data;
using WinterSportAcademy.Models;

namespace WinterSportAcademy.Repositories;

public class EquipmentRepository
{
    private readonly WinterSportAcademyContext _context;

    public EquipmentRepository(WinterSportAcademyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Equipment>> GetAllAsync() =>
        await _context.Equipments
            .Include(e => e.TrainingSession)
            .Include(e => e.Trainee)
            .ToListAsync();

    public async Task<IEnumerable<Equipment>> GetAvailableAsync() =>
        await _context.Equipments
            .Where(e => e.TrainingSessionId == null)
            .ToListAsync();

    public async Task<Equipment?> GetByIdAsync(int id) =>
        await _context.Equipments
            .Include(e => e.TrainingSession)
            .Include(e => e.Trainee)
            .FirstOrDefaultAsync(e => e.EquipmentId == id);

    public async Task AddAsync(Equipment equipment)
    {
        _context.Equipments.Add(equipment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Equipment equipment)
    {
        _context.Entry(equipment).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Equipment equipment)
    {
        _context.Equipments.Remove(equipment);
        await _context.SaveChangesAsync();
    }

    public async Task<TrainingSession?> GetSessionByIdAsync(int sessionId) =>
        await _context.TrainingSession.FindAsync(sessionId);

    public async Task<bool> HasTraineeEquipmentConflictAsync(int traineeId, DateTime sessionStartTime, int? excludeEquipmentId = null)
    {
        return await _context.Equipments
            .Include(e => e.TrainingSession)
            .AnyAsync(e =>
                e.TraineeId == traineeId &&
                e.TrainingSession != null &&
                e.TrainingSession.StartTime == sessionStartTime &&
                (excludeEquipmentId == null || e.EquipmentId != excludeEquipmentId.Value));
    }
}

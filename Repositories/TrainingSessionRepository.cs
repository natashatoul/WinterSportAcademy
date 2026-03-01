using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Data;
using WinterSportAcademy.Models;

namespace WinterSportAcademy.Repositories;

public class TrainingSessionRepository
{
    private readonly WinterSportAcademyContext _context;

    public TrainingSessionRepository(WinterSportAcademyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TrainingSession>> GetAllAsync() => 
        await _context.TrainingSession.Include(s => s.Instructor).ToListAsync();

    public async Task<TrainingSession?> GetByIdAsync(int id) => 
        await _context.TrainingSession.Include(s => s.Instructor)
            .FirstOrDefaultAsync(s => s.TrainingSessionId == id);

    public async Task AddAsync(TrainingSession session)
    {
        _context.TrainingSession.Add(session);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TrainingSession session)
    {
        _context.Entry(session).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var session = await _context.TrainingSession.FindAsync(id);
        if (session != null)
        {
            _context.TrainingSession.Remove(session);
            await _context.SaveChangesAsync();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Data;
using WinterSportAcademy.Models;

namespace WinterSportAcademy.Repositories;

public class TraineeRepository
{
    private readonly WinterSportAcademyContext _context;

    public TraineeRepository(WinterSportAcademyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Trainee>> GetAllAsync() => 
        await _context.Trainees.Include(t => t.Registrations).ToListAsync();

    public async Task<Trainee?> GetByIdAsync(int id) => 
        await _context.Trainees.Include(t => t.Registrations).FirstOrDefaultAsync(t => t.TraineeId == id);

    public async Task AddAsync(Trainee trainee)
    {
        _context.Trainees.Add(trainee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Trainee trainee)
    {
        _context.Entry(trainee).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var trainee = await _context.Trainees.FindAsync(id);
        if (trainee != null)
        {
            _context.Trainees.Remove(trainee);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id) => 
        await _context.Trainees.AnyAsync(e => e.TraineeId == id);
}
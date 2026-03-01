using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Data;
using WinterSportAcademy.Models;

namespace WinterSportAcademy.Repositories;

public class InstructorRepository
{
    private readonly WinterSportAcademyContext _context;

    public InstructorRepository(WinterSportAcademyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Instructor>> GetAllAsync() => 
        await _context.Instructors.ToListAsync();

    public async Task<Instructor?> GetByIdAsync(int id) => 
        await _context.Instructors.FindAsync(id);

    public async Task AddAsync(Instructor instructor)
    {
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Instructor instructor)
    {
        _context.Entry(instructor).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor != null)
        {
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
        }
    }
}
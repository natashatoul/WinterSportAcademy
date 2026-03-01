using WinterSportAcademy.Models;
using WinterSportAcademy.Repositories;

namespace WinterSportAcademy.Services;

public interface IInstructorService
{
    Task<IEnumerable<Instructor>> GetAllAsync();
    Task<Instructor?> GetByIdAsync(int id);
    Task<Instructor> CreateAsync(InstructorDto dto);
    Task UpdateAsync(int id, InstructorDto dto); 
    Task DeleteAsync(int id);                     
    Task<bool> ExistsAsync(int id);
}

public class InstructorService : IInstructorService
{
    private readonly InstructorRepository _repo;
    private readonly ILogger<InstructorService> _logger;

    public InstructorService(InstructorRepository repo, ILogger<InstructorService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<IEnumerable<Instructor>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all instructors");
        return await _repo.GetAllAsync();
    }

    public async Task<Instructor?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

    public async Task<Instructor> CreateAsync(InstructorDto dto)
    {
        var instructor = new Instructor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Specialisation = dto.Specialisation
        };
        await _repo.AddAsync(instructor);
        _logger.LogInformation("Instructor {FirstName} {LastName} created with ID {Id}", 
        dto.FirstName, dto.LastName, instructor.InstructorId);
        return instructor;
    }

    public async Task UpdateAsync(int id, InstructorDto dto)
    {
        var instructor = await _repo.GetByIdAsync(id);
        if (instructor == null) return;

        // Обновляем поля из DTO
        instructor.FirstName = dto.FirstName;
        instructor.LastName = dto.LastName;
        instructor.Specialisation = dto.Specialisation;

        await _repo.UpdateAsync(instructor);
        _logger.LogInformation("Instructor {Id} updated", id);
    }

    public async Task DeleteAsync(int id)
    {
        await _repo.DeleteAsync(id);
        _logger.LogInformation("Instructor {Id} deleted", id);
    }

    public Task<bool> ExistsAsync(int id)
    {
        throw new NotImplementedException();
    }
}
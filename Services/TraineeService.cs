using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Data;
using WinterSportAcademy.Models;
using Microsoft.Extensions.Logging;
using WinterSportAcademy.Repositories;

namespace WinterSportAcademy.Services;

public class TraineeService : ITraineeService
{
    private readonly TraineeRepository _repo;
    private readonly ILogger<TraineeService> _logger;

    public TraineeService(TraineeRepository repo, ILogger<TraineeService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<IEnumerable<Trainee>> GetAllAsync()
    {
        _logger.LogInformation("Getting all trainees");
        return await _repo.GetAllAsync();
    }

    public async Task<Trainee?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Getting trainee {Id}", id);
        return await _repo.GetByIdAsync(id);
    }

    public async Task<Trainee> CreateAsync(TraineeDto dto)
    {
        var trainee = new Trainee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            SkillLevel = dto.SkillLevel
        };
        
        await _repo.AddAsync(trainee);
        _logger.LogInformation("Created trainee {Id}", trainee.TraineeId);
        return trainee;
    }

    public async Task UpdateAsync(int id, TraineeDto dto)
    {
        var trainee = await _repo.GetByIdAsync(id);
        if (trainee == null) 
        {
            _logger.LogWarning("Update failed: Trainee {Id} not found", id);
            throw new Exception("Trainee not found");
        }
        
        trainee.FirstName = dto.FirstName;
        trainee.LastName = dto.LastName;
        trainee.SkillLevel = dto.SkillLevel;
        
        await _repo.UpdateAsync(trainee);
        _logger.LogInformation("Updated trainee {Id}", id);
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting trainee {Id}", id);
        await _repo.DeleteAsync(id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repo.ExistsAsync(id);
    }
}
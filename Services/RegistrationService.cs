using WinterSportAcademy.Models;
using WinterSportAcademy.Repositories;

namespace WinterSportAcademy.Services;

public interface IRegistrationService
{
    Task<IEnumerable<Registration>> GetAllAsync();
    Task<Registration?> GetByIdAsync(int id);
    Task<(Registration? Registration, string? Error)> CreateAsync(RegistrationDto dto);
    Task<string?> UpdateAsync(int id, RegistrationDto dto);
    Task<bool> DeleteAsync(int id);
}

public class RegistrationService : IRegistrationService
{
    private readonly RegistrationRepository _repo;
    private readonly ILogger<RegistrationService> _logger;

    public RegistrationService(RegistrationRepository repo, ILogger<RegistrationService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<IEnumerable<Registration>> GetAllAsync() => await _repo.GetAllAsync();

    public async Task<Registration?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

    public async Task<(Registration? Registration, string? Error)> CreateAsync(RegistrationDto dto)
    {
        var session = await _repo.GetSessionByIdAsync(dto.TrainingSessionId);
        if (session == null)
        {
            return (null, "This session doesn't exist.");
        }

        var hasConflict = await _repo.HasTraineeSessionConflictAsync(dto.TraineeId, session.StartTime);
        if (hasConflict)
        {
            return (null, "You already has session at this time");
        }

        var registration = new Registration
        {
            TraineeId = dto.TraineeId,
            TrainingSessionId = dto.TrainingSessionId,
            RegistrationTime = dto.RegistrationTime,
            IsConfirmed = dto.IsConfirmed
        };

        await _repo.AddAsync(registration);
        _logger.LogInformation("Registration {Id} created", registration.RegistrationNumber);
        return (registration, null);
    }

    public async Task<string?> UpdateAsync(int id, RegistrationDto dto)
    {
        var registration = await _repo.GetByIdAsync(id);
        if (registration == null)
        {
            return "Registration not found";
        }

        var session = await _repo.GetSessionByIdAsync(dto.TrainingSessionId);
        if (session == null)
        {
            return "This session doesn't exist.";
        }

        var hasConflict = await _repo.HasTraineeSessionConflictAsync(dto.TraineeId, session.StartTime, id);
        if (hasConflict)
        {
            return "Trainee already has session at this time";
        }

        registration.TraineeId = dto.TraineeId;
        registration.TrainingSessionId = dto.TrainingSessionId;
        registration.RegistrationTime = dto.RegistrationTime;
        registration.IsConfirmed = dto.IsConfirmed;

        await _repo.UpdateAsync(registration);
        _logger.LogInformation("Registration {Id} updated", id);
        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var registration = await _repo.GetByIdAsync(id);
        if (registration == null) return false;

        await _repo.DeleteAsync(registration);
        _logger.LogInformation("Registration {Id} deleted", id);
        return true;
    }
}

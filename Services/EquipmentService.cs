using WinterSportAcademy.Models;
using WinterSportAcademy.Repositories;

namespace WinterSportAcademy.Services;

public interface IEquipmentService
{
    Task<IEnumerable<Equipment>> GetAllAsync();
    Task<IEnumerable<Equipment>> GetAvailableAsync();
    Task<Equipment?> GetByIdAsync(int id);
    Task<(Equipment? Equipment, string? Error)> CreateAsync(EquipmentDto dto);
    Task<string?> UpdateAsync(int id, EquipmentDto dto);
    Task<bool> DeleteAsync(int id);
}

public class EquipmentService : IEquipmentService
{
    private readonly EquipmentRepository _repo;
    private readonly ILogger<EquipmentService> _logger;

    public EquipmentService(EquipmentRepository repo, ILogger<EquipmentService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<IEnumerable<Equipment>> GetAllAsync() => await _repo.GetAllAsync();

    public async Task<IEnumerable<Equipment>> GetAvailableAsync() => await _repo.GetAvailableAsync();

    public async Task<Equipment?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

    public async Task<(Equipment? Equipment, string? Error)> CreateAsync(EquipmentDto dto)
    {
        if (dto.TraineeId != null && dto.TrainingSessionId != null)
        {
            var session = await _repo.GetSessionByIdAsync(dto.TrainingSessionId.Value);
            if (session == null)
            {
                return (null, "Session not found!");
            }

            var hasConflict = await _repo.HasTraineeEquipmentConflictAsync(dto.TraineeId.Value, session.StartTime);
            if (hasConflict)
            {
                return (null, "Trainee already has equipment for a session at this time.");
            }
        }

        var equipment = new Equipment
        {
            ItemName = dto.ItemName,
            ItemCategory = dto.ItemCategory,
            Specification = dto.Specification,
            Size = dto.Size,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            TraineeId = dto.TraineeId,
            TrainingSessionId = dto.TrainingSessionId
        };

        await _repo.AddAsync(equipment);
        _logger.LogInformation("Equipment {Id} created", equipment.EquipmentId);
        return (equipment, null);
    }

    public async Task<string?> UpdateAsync(int id, EquipmentDto dto)
    {
        var equipment = await _repo.GetByIdAsync(id);
        if (equipment == null)
        {
            return "Equipment not found";
        }

        if (dto.TraineeId != null && dto.TrainingSessionId != null)
        {
            var session = await _repo.GetSessionByIdAsync(dto.TrainingSessionId.Value);
            if (session == null)
            {
                return "Session not found!";
            }

            var hasConflict = await _repo.HasTraineeEquipmentConflictAsync(dto.TraineeId.Value, session.StartTime, id);
            if (hasConflict)
            {
                return "Trainee already has equipment for a session at this time.";
            }
        }

        equipment.ItemName = dto.ItemName;
        equipment.ItemCategory = dto.ItemCategory;
        equipment.Specification = dto.Specification;
        equipment.Size = dto.Size;
        equipment.StartTime = dto.StartTime;
        equipment.EndTime = dto.EndTime;
        equipment.TraineeId = dto.TraineeId;
        equipment.TrainingSessionId = dto.TrainingSessionId;

        await _repo.UpdateAsync(equipment);
        _logger.LogInformation("Equipment {Id} updated", id);
        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var equipment = await _repo.GetByIdAsync(id);
        if (equipment == null) return false;

        await _repo.DeleteAsync(equipment);
        _logger.LogInformation("Equipment {Id} deleted", id);
        return true;
    }
}

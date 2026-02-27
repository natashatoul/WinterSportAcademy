using WinterSportAcademy.Models;

namespace WinterSportAcademy.Services;

public interface ITraineeService
{
    Task<IEnumerable<Trainee>> GetAllAsync();
    Task<Trainee?> GetByIdAsync(int id);
    Task<Trainee> CreateAsync(TraineeDto dto);
    Task UpdateAsync(int id, TraineeDto dto);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
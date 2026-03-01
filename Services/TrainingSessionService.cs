using WinterSportAcademy.Models;
using WinterSportAcademy.Repositories;

namespace WinterSportAcademy.Services;

public interface ITrainingSessionService
{
    Task<IEnumerable<TrainingSessionDto>> GetAllSessionsAsync();
    Task<TrainingSessionDto?> GetSessionByIdAsync(int id);
    Task<TrainingSessionDto> CreateSessionAsync(TrainingSessionDto sessionDto);
    Task<bool> UpdateSessionAsync(int id, TrainingSessionDto sessionDto);
    Task<bool> DeleteSessionAsync(int id);
}

public class TrainingSessionService : ITrainingSessionService
{
    private readonly TrainingSessionRepository _repo;

    public TrainingSessionService(TrainingSessionRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<TrainingSessionDto>> GetAllSessionsAsync()
    {
        var sessions = await _repo.GetAllAsync();
        return sessions.Select(s => new TrainingSessionDto 
        {
            TrainingSessionId = s.TrainingSessionId,
            Title = s.Title,
            StartTime = s.StartTime,
            InstructorId = s.InstructorId,
            FormattedStartTime = s.StartTime.ToString("dd.MM.yyyy HH:mm")
        });
    }

    public async Task<TrainingSessionDto?> GetSessionByIdAsync(int id)
    {
        var s = await _repo.GetByIdAsync(id);
        if (s == null) return null;

        return new TrainingSessionDto
        {
            TrainingSessionId = s.TrainingSessionId,
            Title = s.Title,
            StartTime = s.StartTime,
            InstructorId = s.InstructorId,
            FormattedStartTime = s.StartTime.ToString("dd.MM.yyyy HH:mm")
        };
    }

    public async Task<TrainingSessionDto> CreateSessionAsync(TrainingSessionDto sessionDto)
    {
        var allSessions = await _repo.GetAllAsync();
        bool isBusy = allSessions.Any(s => 
            s.InstructorId == sessionDto.InstructorId && 
            s.StartTime == sessionDto.StartTime);

        if (isBusy)
        {
            // If Instructor is not avalible
            throw new Exception("Instructor is not avalible for this slot");
        }
        var session = new TrainingSession
        {
            Title = sessionDto.Title,
            StartTime = sessionDto.StartTime,
            InstructorId = sessionDto.InstructorId
        };
        
        await _repo.AddAsync(session);
        sessionDto.TrainingSessionId = session.TrainingSessionId;
        return sessionDto;
    }

    public async Task<bool> UpdateSessionAsync(int id, TrainingSessionDto sessionDto)
    {
        var session = await _repo.GetByIdAsync(id);
        if (session == null) return false;

        session.Title = sessionDto.Title;
        session.StartTime = sessionDto.StartTime;
        session.InstructorId = sessionDto.InstructorId;

        await _repo.UpdateAsync(session);
        return true;
    }

    public async Task<bool> DeleteSessionAsync(int id)
    {
        var session = await _repo.GetByIdAsync(id);
        if (session == null) return false;

        await _repo.DeleteAsync(id);
        return true;
    }
}
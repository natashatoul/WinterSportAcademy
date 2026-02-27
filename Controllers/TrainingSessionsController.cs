using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WinterSportAcademy.Data;
using WinterSportAcademy.Models;

namespace WinterSportAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingSessionsController : ControllerBase
    {
        private readonly WinterSportAcademyContext _context;
        private readonly ILogger<TrainingSessionsController> _logger;

        public TrainingSessionsController(WinterSportAcademyContext context, ILogger<TrainingSessionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/TrainingSessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingSession>>> GetTrainingSession()
        {
            _logger.LogInformation("List of all sessions.");
            return await _context.TrainingSession
                .Include(s => s.Instructor)
                .ToListAsync();

        }

        // GET: api/TrainingSessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingSession>> GetTrainingSession(int id)
        {
            _logger.LogInformation("Display session with ID: {Id}", id);
            var trainingSession = await _context.TrainingSession
                .Include(s => s.Instructor)
                .FirstOrDefaultAsync(s => s.TrainingSessionId == id);

            if (trainingSession == null)
            {
                _logger.LogWarning("Тренировка с ID: {Id} не найдена.", id);
                return NotFound();
            }
            return trainingSession;
        }

        // PUT: api/TrainingSessions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingSession(int id, TrainingSession trainingSession)
        {
            if (id != trainingSession.TrainingSessionId)
            {
                return BadRequest();
            }

            var hasConflict = await _context.TrainingSession
                .AnyAsync(s => s.InstructorId == trainingSession.InstructorId && 
                               s.StartTime == trainingSession.StartTime && 
                               s.TrainingSessionId != id);

            if (hasConflict)
            {
                _logger.LogWarning("Instructor {InsId} has session at {Time}", 
                    trainingSession.InstructorId, trainingSession.StartTime);
                return BadRequest("Instructor already has session at this time.");
            }

            _context.Entry(trainingSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Session {Id} updated.", id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TrainingSessionExists(id)) return NotFound();
                else 
                {
                    _logger.LogError(ex, "Error!", id);
                    throw;
                }
            }
            
            return NoContent();
        }

        // POST: api/TrainingSessions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrainingSession>> PostTrainingSession(TrainingSession trainingSession)
        {
            _logger.LogInformation("Создание новой тренировки: {Title}", trainingSession.Title);

            var hasConflict = await _context.TrainingSession
                .AnyAsync(s => s.InstructorId == trainingSession.InstructorId && 
                               s.StartTime == trainingSession.StartTime);

            if (hasConflict)
            {
                _logger.LogWarning("Can't create session, Instructor {InsId} has session at {Time}",
                    trainingSession.InstructorId, trainingSession.StartTime);
                return BadRequest("Instructor is not avalible.");
            }

            _context.TrainingSession.Add(trainingSession);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Тренировка создана с ID: {Id}", trainingSession.TrainingSessionId);
            return CreatedAtAction("GetTrainingSession", new { id = trainingSession.TrainingSessionId }, trainingSession);
        }

        // DELETE: api/TrainingSessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingSession(int id)
        {
        var trainingSession = await _context.TrainingSession.FindAsync(id);
            if (trainingSession == null)
            {
                _logger.LogWarning("Session ID: {Id} does not exist", id);
                return NotFound();
            }

            _context.TrainingSession.Remove(trainingSession);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Session ID: {Id} deleted.", id);

            return NoContent();
        }

        private bool TrainingSessionExists(int id)
        {
            return _context.TrainingSession.Any(e => e.TrainingSessionId == id);
        }
    }
}

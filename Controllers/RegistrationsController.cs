using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Data;
using WinterSportAcademy.Models;

namespace WinterSportAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly WinterSportAcademyContext _context;
        private readonly ILogger<RegistrationsController> _logger;

        public RegistrationsController(WinterSportAcademyContext context, ILogger<RegistrationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Registrations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Registration>>> GetRegistrations()
        {
            _logger.LogInformation("All registrations.");
            // Added Include, to see all details
            return await _context.Registrations
                .Include(r => r.Trainee)
                .Include(r => r.TrainingSession)
                .ToListAsync();
        }

        // GET: api/Registrations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Registration>> GetRegistration(int id)
        {
            _logger.LogInformation("Request ID registration: {Id}", id);
            var registration = await _context.Registrations
                .Include(r => r.Trainee)
                .Include(r => r.TrainingSession)
                .FirstOrDefaultAsync(r => r.RegistrationNumber == id);

            if (registration == null)
            {
                _logger.LogWarning("Registration ID: {Id} not found.", id);
                return NotFound();
            }

            return registration;
        }

        // PUT: api/Registrations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistration(int id, Registration registration)
        {
            if (id != registration.RegistrationNumber)
            {
                return BadRequest();
            }

            var currentSession = await _context.TrainingSession.FindAsync(registration.TrainingSessionId);
            if (currentSession != null)
            {
                var hasConflict = await _context.Registrations
                    .Include(r => r.TrainingSession)
                    .AnyAsync(r => r.TraineeId == registration.TraineeId && 
                                    r.TrainingSession != null &&
                                   r.TrainingSession.StartTime == currentSession.StartTime &&
                                   r.RegistrationNumber != id);

                if (hasConflict)
                {
                    _logger.LogWarning("Trainee {TraineeId} has session at {Time}", 
                        registration.TraineeId, currentSession.StartTime);
                    return BadRequest("Trainee already has session at this time");
                }
            }

            _context.Entry(registration).State = EntityState.Modified;

            try
    {
        await _context.SaveChangesAsync();
        _logger.LogInformation("Registration {Id} updated.", id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Вот это и есть "Proper handling": мы проверяем, существует ли запись
                if (!RegistrationExists(id)) return NotFound();

                _logger.LogError(ex, "Concurrency error for registration {Id}", id);
                throw; // Пробрасываем в Middleware для финальной обработки
            }
            catch (Exception ex)
            {
                // Ловим другие ошибки БД, логируем их локально
                _logger.LogError(ex, "Database error during update of registration {Id}", id);
                throw;
            }

            return NoContent();
        }

        // POST: api/Registrations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Registration>> PostRegistration(Registration registration)
       {
            _logger.LogInformation("New registration for trainee ID: {TraineeId}", registration.TraineeId);

            // 1. Find particular session for trainee 
            var session = await _context.TrainingSession.FindAsync(registration.TrainingSessionId);
            
            if (session == null)
            {
                return BadRequest("This session doesn't exist.");
            }

            // 2. Check if this trainee already has session at this time
            var hasConflict = await _context.Registrations
                .Include(r => r.TrainingSession)
                .AnyAsync(r => r.TraineeId == registration.TraineeId &&
                            r.TrainingSession != null &&
                               r.TrainingSession.StartTime == session.StartTime);

            if (hasConflict)
            {
                _logger.LogWarning("Trainee already has session at {Time}", session.StartTime);
                return BadRequest("You already has session at this time");
            }

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Registration created, number: {Id}", registration.RegistrationNumber);
            return CreatedAtAction("GetRegistration", new { id = registration.RegistrationNumber }, registration);
        }

        // DELETE: api/Registrations/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                _logger.LogWarning("This session not exist. You can't delete {Id}", id);
                return NotFound();
            }

            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Registration {Id} deleted.", id);

            return NoContent();
        }
        private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(e => e.RegistrationNumber == id);
        }

    }
}

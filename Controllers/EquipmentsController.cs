using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class EquipmentsController : ControllerBase
    {
        private readonly WinterSportAcademyContext _context;
        private readonly ILogger<EquipmentsController> _logger;

        public EquipmentsController(WinterSportAcademyContext context, ILogger<EquipmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Equipments// represent all equipments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipment>>> GetEquipments()
        {
            return await _context.Equipments//data about equipment which is avalible 
            .Include(e => e.TrainingSession)
            .Include(e => e.Trainee)
            .ToListAsync();
        }

        [HttpGet("Available")]// for Admin which equipment is avalible
        public async Task<ActionResult<IEnumerable<Equipment>>> GetAvailableEquipment()
        {
            return await _context.Equipments
                .Where(e => e.TrainingSessionId == null)
                .ToListAsync();
        }

        // GET: api/Equipments/5 - 5 it is an ID and return just one position
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> GetEquipment(int id)
        {
            var equipment = await _context.Equipments
            .Include(e => e.TrainingSession)// Include shows information who took this 
            .Include(e => e.Trainee)// equipment and for what session
            .FirstOrDefaultAsync(e => e.EquipmentId == id);

            if (equipment == null)
            {
                return NotFound();
            }

            return equipment;
        }

        // PUT: api/Equipments/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        
        public async Task<IActionResult> PutEquipment(int id, Equipment equipment)
        {
            if (id != equipment.EquipmentId)
            {
                return BadRequest("ID mismatch");
            }

            // We are checking for a situation where the same trainee attempts to use 
            // different equipment at the same time (during sessions that start simultaneously).
            if (equipment.TraineeId != null && equipment.TrainingSessionId != null)
            {
                // 1. First, we retrieve the data for the specific training session 
                // selected by the user to get its exact StartTime.
                var currentSession = await _context.TrainingSession
                    .FindAsync(equipment.TrainingSessionId);

                if (currentSession == null)
                {
                    return NotFound("Session not found!");
                }
                //here check the 'Equipments' table for any other record 
                // where the same trainee has already booked equipment
                // for a session that starts at the exact same time
                var hasConflict = await _context.Equipments
                    .Include(e => e.TrainingSession)
                    .AnyAsync(e =>
                        e.TraineeId == equipment.TraineeId &&
                        e.TrainingSession != null &&
                        e.TrainingSession.StartTime == currentSession.StartTime &&
                        e.EquipmentId != id);


                // A trainee cannot physically use two different sets of equipment 
                // for two different activities happening at the same time.
                if (hasConflict)
                {
                    _logger.LogWarning("Trainee {TraineeId} already has equipment for a session at {Time}",
                        equipment.TraineeId, currentSession.StartTime);
                    return BadRequest("Trainee already has equipment for a session at this time.");
                }
            }

            _context.Entry(equipment).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Equipment {Id} updated successfully", id);
            return NoContent();
        }

        // POST: api/Equipments
        [HttpPost]
        public async Task<ActionResult<Equipment>> PostEquipment(Equipment equipment)
        {
            _logger.LogInformation("Adding new equipment: {ItemName}", equipment.ItemName);
            if (equipment.TraineeId != null && equipment.TrainingSessionId != null)
            {
                var currentSession = await _context.TrainingSession.FindAsync(equipment.TrainingSessionId);

                if (currentSession != null)
                {
                    var hasConflict = await _context.Equipments
                        .Include(e => e.TrainingSession)
                        .AnyAsync(e =>
                            e.TraineeId == equipment.TraineeId &&
                            e.TrainingSession != null &&
                            e.TrainingSession.StartTime == currentSession.StartTime);

                    if (hasConflict)
                    {
                        return BadRequest("Trainee already has equipment for a session at this time.");
                    }
                }
            }
            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEquipment), new { id = equipment.EquipmentId }, equipment);
        }

        // DELETE: api/Equipments/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("Hello")]
        public ActionResult<string> SayHello()
        {
            return "Hello. world!";
        }

    }
}
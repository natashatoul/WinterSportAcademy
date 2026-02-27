using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Data;
using WinterSportAcademy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace WinterSportAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TraineesController : ControllerBase
    {
        private readonly WinterSportAcademyContext _context;
        private readonly ILogger<TraineesController> _logger;

        public TraineesController(WinterSportAcademyContext context, ILogger<TraineesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Trainees
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<IEnumerable<Trainee>>> GetTrainees()
        {
            _logger.LogInformation("Admin requested the list of all trainees.");
            return await _context.Trainees.ToListAsync();
        }

        // GET: api/Trainees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainee>> GetTrainee(int id)
        {
            var trainee = await _context.Trainees.FindAsync(id);

            if (trainee == null)
            {
                _logger.LogWarning("Trainee with ID {Id} not found.", id);
                return NotFound();
            }

            return trainee;
        }

        // PUT: api/Trainees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> PutTrainee(int id, Trainee trainee)
        {
            if (id != trainee.TraineeId)
            {
                return BadRequest();
            }

            _context.Entry(trainee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Trainee ID {Id} updated successfully.", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TraineeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Trainees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous] // for new users
        public async Task<ActionResult<Trainee>> PostTrainee(Trainee trainee)
        {
            _logger.LogInformation("Registering a new trainee: {FirstName}, {LastName}", trainee.FirstName, trainee.LastName);
            _context.Trainees.Add(trainee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrainee", new { id = trainee.TraineeId }, trainee);
        }

        // DELETE: api/Trainees/5
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteTrainee(int id)
        {
            var trainee = await _context.Trainees.FindAsync(id);
            if (trainee == null)
            {
                return NotFound();
            }

            _logger.LogWarning("Admin is deleting trainee ID {Id} {FirstName}, {LastName})", id, trainee.FirstName, trainee.LastName);
            _context.Trainees.Remove(trainee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TraineeExists(int id)
        {
            return _context.Trainees.Any(e => e.TraineeId == id);
        }
    }
}

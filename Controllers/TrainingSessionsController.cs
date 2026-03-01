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
using WinterSportAcademy.Services;

namespace WinterSportAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingSessionsController : ControllerBase
    {
        //private readonly WinterSportAcademyContext _context;
        private readonly ITrainingSessionService _service;
        private readonly ILogger<TrainingSessionsController> _logger;

      public TrainingSessionsController(ITrainingSessionService service, ILogger<TrainingSessionsController> logger)
    {
        _service = service;
        _logger = logger;
    }

        // GET: api/TrainingSessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingSession>>> GetTrainingSession()
        {
            _logger.LogInformation("List of all sessions.");
            var sessions = await _service.GetAllSessionsAsync();
            return Ok(sessions);

        }

        // GET: api/TrainingSessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingSession>> GetTrainingSession(int id)
        {
            _logger.LogInformation("Display session with ID: {Id}", id);
            var session = await _service.GetSessionByIdAsync(id);

            if (session == null)
            {
                _logger.LogWarning("Session {Id} not found.", id);
                return NotFound();
            }
            return Ok(session);
        }

        // PUT: api/TrainingSessions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingSession(int id, TrainingSessionDto dto)
        {
            if (id != dto.TrainingSessionId)
            {
                return BadRequest();
            }

            var result = await _service.UpdateSessionAsync(id, dto);
            if (!result) return NotFound();

            return NoContent();
        }

        // POST: api/TrainingSessions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrainingSessionDto>> PostTrainingSession(TrainingSessionDto dto)
        {
            _logger.LogInformation("Creating new session: {Title}", dto.Title);

            var created = await _service.CreateSessionAsync(dto);

            return CreatedAtAction(nameof(GetTrainingSession), new { id = created.TrainingSessionId }, created);
        }

        // DELETE: api/TrainingSessions/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        
        public async Task<IActionResult> DeleteTrainingSession(int id)
        {
            var result = await _service.DeleteSessionAsync(id);
            if (!result) return NotFound();

            _logger.LogInformation("Session {Id} deleted.", id);
            return NoContent();
        }
    }
}

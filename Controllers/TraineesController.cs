using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Models;
using WinterSportAcademy.Services; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace WinterSportAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // controller completly close with authorisation
    public class TraineesController : ControllerBase
    {
        // use servise interface, not database context
        private readonly ITraineeService _traineeService;
        private readonly ILogger<TraineesController> _logger;

        public TraineesController(ITraineeService traineeService, ILogger<TraineesController> logger)
        {
            _traineeService = traineeService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all registered trainees.
        /// </summary>
        /// <response code="200">Returns the list of trainees.</response>

        // GET: api/Trainees
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)] // only for admin
        public async Task<ActionResult<IEnumerable<Trainee>>> GetTrainees()
        {
            _logger.LogInformation("Admin requested the list of all trainees.");
            var trainees = await _traineeService.GetAllAsync();
            return Ok(trainees);
        }
        /// <summary>
        /// Gets a specific trainee by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the trainee.</param>
        /// <response code="200">Returns the requested trainee.</response>
        /// <response code="404">If the trainee was not found.</response>

        // GET: api/Trainees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainee>> GetTrainee(int id)
        {
            var trainee = await _traineeService.GetByIdAsync(id);
            if (trainee == null)
            {
                _logger.LogWarning("Trainee with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(trainee);
        }

        /// <summary>
        /// Updates an existing trainee's information.
        /// </summary>
        /// <param name="id">ID of the trainee to update.</param>
        /// <param name="dto">Updated trainee data details.</param>
        /// <response code="204">Update successful.</response>
        /// <response code="400">If IDs do not match or data is invalid.</response>

        // PUT: api/Trainees/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        
        public async Task<IActionResult> PutTrainee(int id, TraineeDto dto) // use DTO(Data Transfer Object)
        {
            if (id != dto.TraineeId)
            {
                _logger.LogWarning("ID mismatch during update for Trainee {Id}", id);
                return BadRequest("ID mismatch");
            }
            await _traineeService.UpdateAsync(id, dto);
            
            _logger.LogInformation("Trainee ID {Id} updated successfully.", id);
            return NoContent();
        }

        /// <summary>
        /// Registers a new trainee in the system.
        /// </summary>
        /// <param name="dto">The trainee details.</param>
        /// <response code="201">Trainee created successfully.</response>
        /// <response code="400">If the data is invalid.</response>

        // POST: api/Trainees
        [HttpPost]
        [AllowAnonymous] // for new users
        public async Task<ActionResult<Trainee>> PostTrainee(TraineeDto dto)  
        {
            _logger.LogInformation("Registering a new trainee: {FirstName} {LastName}", dto.FirstName, dto.LastName);
            var trainee = await _traineeService.CreateAsync(dto); 
            return CreatedAtAction(nameof(GetTrainee), new { id = trainee.TraineeId }, trainee);
        }

        /// <summary>
        /// Deletes a trainee from the system. Admin only.
        /// </summary>
        /// <param name="id">The ID of the trainee to remove.</param>
        /// <response code="204">Trainee deleted successfully.</response>
        /// <response code="404">If the trainee was not found.</response>

        // DELETE: api/Trainees/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        
        public async Task<IActionResult> DeleteTrainee(int id)
        {
            var trainee = await _traineeService.GetByIdAsync(id);
            if (trainee == null)
            {
                _logger.LogWarning("Delete failed: Trainee ID {Id} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Admin is deleting trainee ID {Id}", id);
            await _traineeService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("Hello")]
        public ActionResult<string> SayHello()
        {
            return "Hello. world!";
        }
    }
}
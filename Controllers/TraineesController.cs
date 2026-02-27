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
    [Authorize] // controller completly close with authorisation
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

        // GET: api/Trainees
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)] // only for admin
        public async Task<ActionResult<IEnumerable<Trainee>>> GetTrainees()
        {
            try
            {
                _logger.LogInformation("Admin requested the list of all trainees.");
                var trainees = await _traineeService.GetAllAsync();
                return Ok(trainees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching trainees");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Trainees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainee>> GetTrainee(int id)
        {
            try
            {
                var trainee = await _traineeService.GetByIdAsync(id);
                if (trainee == null)
                {
                    _logger.LogWarning("Trainee with ID {Id} not found.", id);
                    return NotFound();
                }
                return Ok(trainee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching trainee {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Trainees/5
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> PutTrainee(int id, TraineeDto dto) // use DTO(Data Transfer Object)
        {
            if (id != dto.TraineeId)
            {
                _logger.LogWarning("ID mismatch during update for Trainee {Id}", id);
                return BadRequest("ID mismatch");
            }

            try
            {
                await _traineeService.UpdateAsync(id, dto);
                _logger.LogInformation("Trainee ID {Id} updated successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (!await _traineeService.ExistsAsync(id))
                {
                    return NotFound();
                }
                _logger.LogError(ex, "Error updating trainee {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Trainees
        [HttpPost]
        [AllowAnonymous] // for new users
        public async Task<ActionResult<Trainee>> PostTrainee(TraineeDto dto)  
        {
            try
            {
                _logger.LogInformation("Registering a new trainee: {FirstName} {LastName}", dto.FirstName, dto.LastName);
                var trainee = await _traineeService.CreateAsync(dto); 
                return CreatedAtAction("GetTrainee", new { id = trainee.TraineeId }, trainee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating trainee");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/Trainees/5
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteTrainee(int id)
        {
            try
            {
                var trainee = await _traineeService.GetByIdAsync(id);
                if (trainee == null)
                {
                    _logger.LogWarning("Delete failed: Trainee ID {Id} not found.", id);
                    return NotFound();
                }

                _logger.LogWarning("Admin is deleting trainee ID {Id} ({FirstName} {LastName})", id, trainee.FirstName, trainee.LastName);
                await _traineeService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting trainee {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
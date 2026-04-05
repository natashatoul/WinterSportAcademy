using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WinterSportAcademy.Models;
using WinterSportAcademy.Services;

namespace WinterSportAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _service;
        private readonly ILogger<RegistrationsController> _logger;

        public RegistrationsController(IRegistrationService service, ILogger<RegistrationsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Registration>>> GetRegistrations()
        {
            _logger.LogInformation("All registrations.");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Registration>> GetRegistration(int id)
        {
            _logger.LogInformation("Request ID registration: {Id}", id);
            var registration = await _service.GetByIdAsync(id);

            if (registration == null)
            {
                _logger.LogWarning("Registration ID: {Id} not found.", id);
                return NotFound();
            }

            return Ok(registration);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistration(int id, RegistrationDto dto)
        {
            if (id != dto.RegistrationNumber)
            {
                return BadRequest();
            }

            var error = await _service.UpdateAsync(id, dto);
            if (error != null)
            {
                if (error == "Registration not found")
                {
                    return NotFound();
                }
                return BadRequest(error);
            }

            _logger.LogInformation("Registration {Id} updated.", id);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Registration>> PostRegistration(RegistrationDto dto)
        {
            _logger.LogInformation("New registration for trainee ID: {TraineeId}", dto.TraineeId);
            var (registration, error) = await _service.CreateAsync(dto);
            if (error != null || registration == null)
            {
                return BadRequest(error);
            }

            _logger.LogInformation("Registration created, number: {Id}", registration.RegistrationNumber);
            return CreatedAtAction(nameof(GetRegistration), new { id = registration.RegistrationNumber }, registration);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("This session not exist. You can't delete {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Registration {Id} deleted.", id);
            return NoContent();
        }
    }
}

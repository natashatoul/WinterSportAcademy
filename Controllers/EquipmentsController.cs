using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WinterSportAcademy.Models;
using WinterSportAcademy.Services;

namespace WinterSportAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        private readonly IEquipmentService _service;
        private readonly ILogger<EquipmentsController> _logger;

        public EquipmentsController(IEquipmentService service, ILogger<EquipmentsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/Equipments// represent all equipments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipment>>> GetEquipments()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("Available")]// for Admin which equipment is avalible
        public async Task<ActionResult<IEnumerable<Equipment>>> GetAvailableEquipment()
        {
            return Ok(await _service.GetAvailableAsync());
        }

        // GET: api/Equipments/5 - 5 it is an ID and return just one position
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> GetEquipment(int id)
        {
            var equipment = await _service.GetByIdAsync(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return Ok(equipment);
        }

        // PUT: api/Equipments/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        
        public async Task<IActionResult> PutEquipment(int id, EquipmentDto dto)
        {
            if (id != dto.EquipmentId)
            {
                return BadRequest("ID mismatch");
            }

            var error = await _service.UpdateAsync(id, dto);
            if (error != null)
            {
                if (error == "Equipment not found")
                {
                    return NotFound();
                }
                return BadRequest(error);
            }

            _logger.LogInformation("Equipment {Id} updated successfully", id);
            return NoContent();
        }

        // POST: api/Equipments
        [HttpPost]
        public async Task<ActionResult<Equipment>> PostEquipment(EquipmentDto dto)
        {
            _logger.LogInformation("Adding new equipment: {ItemName}", dto.ItemName);
            var (equipment, error) = await _service.CreateAsync(dto);
            if (error != null || equipment == null)
            {
                return BadRequest(error);
            }

            return CreatedAtAction(nameof(GetEquipment), new { id = equipment.EquipmentId }, equipment);
        }

        [Authorize]
        [HttpPost("{id}/rent")]
        public async Task<IActionResult> RentEquipment(int id)
        {
            var traineeIdClaim = User.FindFirst("TraineeId")?.Value;
            if (!int.TryParse(traineeIdClaim, out var traineeId))
            {
                return BadRequest("Trainee profile is not linked to this account.");
            }

            var error = await _service.RentToTraineeAsync(id, traineeId);
            if (error != null)
            {
                if (error == "Equipment not found")
                {
                    return NotFound(error);
                }
                return BadRequest(error);
            }

            return Ok("Equipment rented successfully.");
        }

        // DELETE: api/Equipments/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("Hello")]
        public ActionResult<string> SayHello()
        {
            return "Hello. world!";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Models;

namespace WinterSportAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentRentalController : ControllerBase
    {
        private readonly AcademyContext _context;

        public EquipmentRentalController(AcademyContext context)
        {
            _context = context;
        }

        // GET: api/EquipmentRental
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentRental>>> GetEquipmentRentals()
        {
            return await _context.EquipmentRentals.ToListAsync();
        }

        // GET: api/EquipmentRental/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentRental>> GetEquipmentRental(int id)
        {
            var equipmentRental = await _context.EquipmentRentals.FindAsync(id);

            if (equipmentRental == null)
            {
                return NotFound();
            }

            return equipmentRental;
        }

        // PUT: api/EquipmentRental/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipmentRental(int id, EquipmentRental equipmentRental)
        {
            if (id != equipmentRental.EquipmentRentalID)
            {
                return BadRequest();
            }

            _context.Entry(equipmentRental).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentRentalExists(id))
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

        // POST: api/EquipmentRental
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EquipmentRental>> PostEquipmentRental(EquipmentRental equipmentRental)
        {
            _context.EquipmentRentals.Add(equipmentRental);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEquipmentRental", new { id = equipmentRental.EquipmentRentalID }, equipmentRental);
        }

        // DELETE: api/EquipmentRental/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipmentRental(int id)
        {
            var equipmentRental = await _context.EquipmentRentals.FindAsync(id);
            if (equipmentRental == null)
            {
                return NotFound();
            }

            _context.EquipmentRentals.Remove(equipmentRental);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EquipmentRentalExists(int id)
        {
            return _context.EquipmentRentals.Any(e => e.EquipmentRentalID == id);
        }
    }
}

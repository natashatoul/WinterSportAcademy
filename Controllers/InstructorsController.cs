using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using WinterSportAcademy.Data;
using WinterSportAcademy.Models;
using WinterSportAcademy.Services;

namespace WinterSportAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        //private readonly WinterSportAcademyContext _context;
        private readonly IInstructorService _service;

        //public InstructorsController(WinterSportAcademyContext service)
        public InstructorsController(IInstructorService service)
        {
            //_context = context;
            _service = service;
        }

        // GET: api/Instructors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instructor>>> GetInstructors()
        {
            //return await _service.Instructors.ToListAsync();
            var instructors = await _service.GetAllAsync();
            return Ok(instructors);
        }

        // GET: api/Instructors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Instructor>> GetInstructor(int id)
        {
            var instructor = await _service.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return Ok(instructor);
        }

        // PUT: api/Instructors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstructor(int id, InstructorDto dto)
      {
            try
            {
                await _service.UpdateAsync(id, dto);
            }
            catch (Exception)
            {
                if (!await _service.ExistsAsync(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        // POST: api/Instructors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Instructor>> PostInstructor(InstructorDto dto)
        {
            // _context.Instructors.Add(instructor);
            // await _context.SaveChangesAsync();
            var createInstructor = await _service.CreateAsync(dto);
            return CreatedAtAction("GetInstructor", new { id = createInstructor.InstructorId }, createInstructor);
        }

        // DELETE: api/Instructors/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            // var instructor = await _context.Instructors.FindAsync(id);
            // if (instructor == null)
            // {
            //     return NotFound();
            // }

            // _context.Instructors.Remove(instructor);
            // await _context.SaveChangesAsync();
            var instructor = await _service.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return NoContent();
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DuesApi.Data;
using DuesApi.Models;
using DuesApi.Models.Dtos;

namespace DuesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResidentsController : ControllerBase
    {
        private readonly DuesContext _db;
        public ResidentsController(DuesContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resident>>> GetAll() =>
            await _db.Residents.Include(r => r.Apartment).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Resident>> GetById(int id)
        {
            var resident = await _db.Residents.Include(r => r.Apartment).FirstOrDefaultAsync(r => r.Id == id);
            return resident is null ? NotFound() : resident;
        }

        [HttpPost]
        public async Task<ActionResult<Resident>> Create(ResidentDto dto)
        {
            var apartment = await _db.Apartments.FindAsync(dto.ApartmentId);
            if (apartment is null) return BadRequest("Apartment does not exist");

            var resident = new Resident { Name = dto.Name, ApartmentId = dto.ApartmentId };
            _db.Residents.Add(resident);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = resident.Id }, resident);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ResidentDto dto)
        {
            var resident = await _db.Residents.FindAsync(id);
            if (resident is null) return NotFound();

            resident.Name = dto.Name;
            resident.ApartmentId = dto.ApartmentId;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resident = await _db.Residents.FindAsync(id);
            if (resident is null) return NotFound();

            _db.Residents.Remove(resident);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

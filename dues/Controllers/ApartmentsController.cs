using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DuesApi.Data;
using DuesApi.Models;
using DuesApi.Models.Dtos;

namespace DuesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentsController : ControllerBase
    {
        private readonly DuesContext _db;
        public ApartmentsController(DuesContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Apartment>>> GetAll() =>
            await _db.Apartments.Include(a => a.Residents).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Apartment>> GetById(int id)
        {
            var apartment = await _db.Apartments
                .Include(a => a.Residents)
                .Include(a => a.Dues)
                .FirstOrDefaultAsync(a => a.Id == id);
            return apartment is null ? NotFound() : apartment;
        }

        [HttpPost]
        public async Task<ActionResult<Apartment>> Create(ApartmentDto dto)
        {
            var apartment = new Apartment { Number = dto.Number, MonthlyFee = dto.MonthlyFee };
            _db.Apartments.Add(apartment);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = apartment.Id }, apartment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ApartmentDto dto)
        {
            var apartment = await _db.Apartments.FindAsync(id);
            if (apartment is null) return NotFound();

            apartment.Number = dto.Number;
            apartment.MonthlyFee = dto.MonthlyFee;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var apartment = await _db.Apartments.FindAsync(id);
            if (apartment is null) return NotFound();

            _db.Apartments.Remove(apartment);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

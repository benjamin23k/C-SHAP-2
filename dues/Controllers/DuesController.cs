using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DuesApi.Data;
using DuesApi.Models;
using DuesApi.Models.Dtos;

namespace DuesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DuesController : ControllerBase
    {
        private readonly DuesContext _db;
        public DuesController(DuesContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Due>>> GetAll() =>
            await _db.Dues.Include(d => d.Apartment).Include(d => d.Payments).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Due>> GetById(int id)
        {
            var due = await _db.Dues
                .Include(d => d.Apartment)
                .Include(d => d.Payments)
                .FirstOrDefaultAsync(d => d.Id == id);
            return due is null ? NotFound() : due;
        }

        [HttpGet("apartment/{apartmentId}")]
        public async Task<ActionResult<IEnumerable<Due>>> GetByApartment(int apartmentId) =>
            await _db.Dues.Where(d => d.ApartmentId == apartmentId).Include(d => d.Payments).ToListAsync();

        [HttpPost]
        public async Task<ActionResult<Due>> Create(DueDto dto)
        {
            var apartment = await _db.Apartments.FindAsync(dto.ApartmentId);
            if (apartment is null) return BadRequest("Apartment does not exist");

            var due = new Due
            {
                ApartmentId = dto.ApartmentId,
                Month = dto.Month,
                Year = dto.Year,
                Amount = dto.Amount,
                DueDate = dto.DueDate,
                Status = DueStatus.Pending
            };
            _db.Dues.Add(due);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = due.Id }, due);
        }

        [HttpPost("generate-monthly")]
        public async Task<ActionResult<IEnumerable<Due>>> GenerateMonthly(int month, int year)
        {
            var apartments = await _db.Apartments.ToListAsync();
            var newDues = new List<Due>();

            foreach (var apt in apartments)
            {
                bool exists = await _db.Dues.AnyAsync(d => d.ApartmentId == apt.Id && d.Month == month && d.Year == year);
                if (exists) continue;

                var due = new Due
                {
                    ApartmentId = apt.Id,
                    Month = month,
                    Year = year,
                    Amount = apt.MonthlyFee,
                    DueDate = new DateTime(year, month, 5),
                    Status = DueStatus.Pending
                };
                _db.Dues.Add(due);
                newDues.Add(due);
            }

            await _db.SaveChangesAsync();
            return Ok(newDues);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var due = await _db.Dues.FindAsync(id);
            if (due is null) return NotFound();

            _db.Dues.Remove(due);
            await _db.SaveChangesAsync();
            return NoContent();
        }

     
        [HttpGet("reports/debts")]
        public async Task<ActionResult<IEnumerable<DebtReportDto>>> DebtReport()
        {
            var apartments = await _db.Apartments.Include(a => a.Dues).ToListAsync();

            var report = apartments.Select(apt =>
            {
                var dues = apt.Dues.Where(d => d.Status != DueStatus.Paid).ToList();
                return new DebtReportDto(
                    apt.Id,
                    apt.Number,
                    dues.Sum(d => d.Balance),
                    dues.Count
                );
            }).Where(r => r.TotalDebt > 0);

            return Ok(report);
        }

       
        [HttpPost("update-overdue")]
        public async Task<IActionResult> UpdateOverdue()
        {
            var now = DateTime.Now;
            var overdue = await _db.Dues
                .Where(d => d.Status == DueStatus.Pending && d.DueDate < now)
                .ToListAsync();

            foreach (var d in overdue)
                d.Status = DueStatus.Overdue;

            await _db.SaveChangesAsync();
            return Ok(new { updated = overdue.Count });
        }
    }
}

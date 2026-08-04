using Microsoft.AspNetCore.Mvc;
using Dues.Domain.Entities;
using Dues.Infrastructure.Interfaces;
using DuesApi.Models.Dtos;

namespace DuesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DuesController : ControllerBase
    {
        private readonly IDueRepository _repository;
        private readonly IApartmentRepository _apartmentRepository;

        public DuesController(IDueRepository repository, IApartmentRepository apartmentRepository)
        {
            _repository = repository;
            _apartmentRepository = apartmentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Due>>> GetAll() =>
            await _repository.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Due>> GetById(int id)
        {
            var due = await _repository.GetByIdAsync(id);
            return due is null ? NotFound() : due;
        }

        [HttpGet("apartment/{apartmentId}")]
        public async Task<ActionResult<IEnumerable<Due>>> GetByApartment(int apartmentId) =>
            await _repository.GetByApartmentAsync(apartmentId);

        [HttpPost]
        public async Task<ActionResult<Due>> Create(DueDto dto)
        {
            var apartment = await _apartmentRepository.GetByIdAsync(dto.ApartmentId);
            if (apartment is null) return BadRequest("Apartment does not exist");

            var due = new Due
            {
                ApartmentId = dto.ApartmentId,
                Month = dto.Month,
                Year = dto.Year,
                Amount = dto.Amount,
    
                Status = DueStatus.Pending
            };

            var created = await _repository.AddAsync(due);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPost("generate-monthly")]
        public async Task<ActionResult<IEnumerable<Due>>> GenerateMonthly(int month, int year) =>
            Ok(await _repository.GenerateMonthlyAsync(month, year));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var due = await _repository.GetByIdAsync(id);
            if (due is null) return NotFound();

            await _repository.DeleteAsync(due);
            return NoContent();
        }

        [HttpGet("reports/debts")]
        public async Task<ActionResult<IEnumerable<DebtReportDto>>> DebtReport()
        {
            var report = await _repository.GetDebtReportAsync();
            var result = report.Select(r => new DebtReportDto(
                r.Apartment.Id,
                r.Apartment.Number,
                r.TotalDebt,
                r.OverdueCount
            ));

            return Ok(result);
        }

        [HttpPost("update-overdue")]
        public async Task<IActionResult> UpdateOverdue()
        {
            var updated = await _repository.UpdateOverdueAsync();
            return Ok(new { updated });
        }
    }
}

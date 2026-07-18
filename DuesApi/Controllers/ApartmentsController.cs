using Microsoft.AspNetCore.Mvc;
using Dues.Domain.Entities;
using Dues.Infrastructure.Interfaces;
using DuesApi.Models.Dtos;

namespace DuesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentsController : ControllerBase
    {
        private readonly IApartmentRepository _repository;

        public ApartmentsController(IApartmentRepository repository) => _repository = repository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Apartament>>> GetAll() =>
            await _repository.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Apartament>> GetById(int id)
        {
            var apartment = await _repository.GetByIdAsync(id);
            return apartment is null ? NotFound() : apartment;
        }

        [HttpPost]
        public async Task<ActionResult<Apartament>> Create(ApartmentDto dto)
        {
            var apartment = new Apartament { Number = dto.Number, MonthlyFee = dto.MonthlyFee };
            var created = await _repository.AddAsync(apartment);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ApartmentDto dto)
        {
            var apartment = await _repository.GetByIdAsync(id);
            if (apartment is null) return NotFound();

            apartment.Number = dto.Number;
            apartment.MonthlyFee = dto.MonthlyFee;

            await _repository.UpdateAsync(apartment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var apartment = await _repository.GetByIdAsync(id);
            if (apartment is null) return NotFound();

            await _repository.DeleteAsync(apartment);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Dues.Domain.Entities;
using Dues.Infrastructure.Interfaces;
using DuesApi.Models.Dtos;

namespace DuesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResidentsController : ControllerBase
    {
        private readonly IResidentRepository _repository;
        private readonly IApartmentRepository _apartmentRepository;

        public ResidentsController(IResidentRepository repository, IApartmentRepository apartmentRepository)
        {
            _repository = repository;
            _apartmentRepository = apartmentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resident>>> GetAll() =>
            await _repository.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Resident>> GetById(int id)
        {
            var resident = await _repository.GetByIdAsync(id);
            return resident is null ? NotFound() : resident;
        }

        [HttpPost]
        public async Task<ActionResult<Resident>> Create(ResidentDto dto)
        {
            var apartment = await _apartmentRepository.GetByIdAsync(dto.ApartmentId);
            if (apartment is null) return BadRequest("Apartment does not exist");

            var resident = new Resident { Name = dto.Name, ApartmentId = dto.ApartmentId };
            var created = await _repository.AddAsync(resident);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ResidentDto dto)
        {
            var resident = await _repository.GetByIdAsync(id);
            if (resident is null) return NotFound();

            resident.Name = dto.Name;
            resident.ApartmentId = dto.ApartmentId;

            await _repository.UpdateAsync(resident);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resident = await _repository.GetByIdAsync(id);
            if (resident is null) return NotFound();

            await _repository.DeleteAsync(resident);
            return NoContent();
        }
    }
}

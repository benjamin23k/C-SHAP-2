using Microsoft.AspNetCore.Mvc;
using Dues.Domain.Entities;
using Dues.Infrastructure.Interfaces;
using DuesApi.Models.Dtos;

namespace DuesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _repository;
        private readonly IDueRepository _dueRepository;

        public PaymentsController(IPaymentRepository repository, IDueRepository dueRepository)
        {
            _repository = repository;
            _dueRepository = dueRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAll() =>
            await _repository.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetById(int id)
        {
            var payment = await _repository.GetByIdAsync(id);
            return payment is null ? NotFound() : payment;
        }

        [HttpGet("due/{dueId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetByDue(int dueId) =>
            await _repository.GetByDueAsync(dueId);

        [HttpPost]
        public async Task<ActionResult<Payment>> Create(PaymentDto dto)
        {
            var due = await _dueRepository.GetByIdAsync(dto.DueId);
            if (due is null) return BadRequest("Due does not exist");
            if (dto.Amount <= 0) return BadRequest("Amount must be greater than 0");
            if (due.Balance <= 0) return BadRequest("Due is already paid");

            var payment = new Payment
            {
                DueId = dto.DueId,
                Amount = dto.Amount,
                Method = dto.Method,
                ReceiptNumber = $"REC-{DateTime.Now:yyyyMMddHHmmss}-{dto.DueId}"
            };

            var created = await _repository.CreateAsync(payment);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id}/receipt")]
        public async Task<IActionResult> GetReceipt(int id)
        {
            var payment = await _repository.GetWithDetailsAsync(id);
            if (payment is null) return NotFound();

            var receipt = new
            {
                payment.ReceiptNumber,
                Apartment = payment.Due!.Apartment!.Number,
                Period = $"{payment.Due.Month:00}/{payment.Due.Year}",
                AmountPaid = payment.Amount,
                Method = payment.Method.ToString(),
                RemainingBalance = payment.Due.Balance
            };

            return Ok(receipt);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _repository.GetByIdAsync(id);
            if (payment is null) return NotFound();

            await _repository.DeleteAsync(payment);
            return NoContent();
        }
    }
}

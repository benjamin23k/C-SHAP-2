using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DuesApi.Data;
using DuesApi.Models;
using DuesApi.Models.Dtos;

namespace DuesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly DuesContext _db;
        public PaymentsController(DuesContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAll() =>
            await _db.Payments.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetById(int id)
        {
            var payment = await _db.Payments.FindAsync(id);
            return payment is null ? NotFound() : payment;
        }

        [HttpGet("due/{dueId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetByDue(int dueId) =>
            await _db.Payments.Where(p => p.DueId == dueId).ToListAsync();

        // Register a payment, updates the due's balance/status
        [HttpPost]
        public async Task<ActionResult<Payment>> Create(PaymentDto dto)
        {
            var due = await _db.Dues.FindAsync(dto.DueId);
            if (due is null) return BadRequest("Due does not exist");
            if (dto.Amount <= 0) return BadRequest("Amount must be greater than 0");
            if (due.Balance <= 0) return BadRequest("Due is already paid");

            var payment = new Payment
            {
                DueId = dto.DueId,
                Amount = dto.Amount,
                Method = dto.Method,
                Date = DateTime.Now,
                ReceiptNumber = $"REC-{DateTime.Now:yyyyMMddHHmmss}-{dto.DueId}"
            };

            _db.Payments.Add(payment);
            due.AmountPaid += dto.Amount;
            due.Status = due.Balance <= 0 ? DueStatus.Paid : DueStatus.Partial;

            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
        }

        // Receipt for a payment
        [HttpGet("{id}/receipt")]
        public async Task<IActionResult> GetReceipt(int id)
        {
            var payment = await _db.Payments
                .Include(p => p.Due)
                .ThenInclude(d => d!.Apartment)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment is null) return NotFound();

            var receipt = new
            {
                payment.ReceiptNumber,
                payment.Date,
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
            var payment = await _db.Payments.Include(p => p.Due).FirstOrDefaultAsync(p => p.Id == id);
            if (payment is null) return NotFound();

            var due = payment.Due!;
            due.AmountPaid -= payment.Amount;
            due.Status = due.AmountPaid <= 0 ? DueStatus.Pending : DueStatus.Partial;

            _db.Payments.Remove(payment);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

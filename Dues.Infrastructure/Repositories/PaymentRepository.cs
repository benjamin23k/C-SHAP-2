using Dues.Domain.Entities;
using Dues.Infrastructure.Context;
using Dues.Infrastructure.Core;
using Dues.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dues.Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(DuesContext db) : base(db)
        {
        }

        public async Task<List<Payment>> GetByDueAsync(int dueId) =>
            await Db.Payments.Where(p => p.DueId == dueId).ToListAsync();

        public async Task<Payment> CreateAsync(Payment payment)
        {
            var due = await Db.Dues.FindAsync(payment.DueId)
                ?? throw new InvalidOperationException("Due does not exist");

            Db.Payments.Add(payment);

            due.AmountPaid += payment.Amount;
            due.Status = due.Balance <= 0 ? DueStatus.Paid : DueStatus.Partial;

            await Db.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment?> GetWithDetailsAsync(int id) =>
            await Db.Payments
                .Include(p => p.Due)
                .ThenInclude(d => d!.Apartment)
                .FirstOrDefaultAsync(p => p.Id == id);

        public override async Task DeleteAsync(Payment payment)
        {
            var due = await Db.Dues.FindAsync(payment.DueId);
            if (due is not null)
            {
                due.AmountPaid -= payment.Amount;
                due.Status = due.AmountPaid <= 0 ? DueStatus.Pending : DueStatus.Partial;
            }

            Db.Payments.Remove(payment);
            await Db.SaveChangesAsync();
        }
    }
}

using Dues.Domain.Entities;

namespace Dues.Infrastructure.Interfaces
{
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
        Task<List<Payment>> GetByDueAsync(int dueId);
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment?> GetWithDetailsAsync(int id);
        Task DeleteAsync(Payment payment);
    }
}

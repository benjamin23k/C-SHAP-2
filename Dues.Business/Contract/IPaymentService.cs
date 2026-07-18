using Dues.Business.Dtos;

namespace Dues.Business.Contract
{
    public interface IPaymentService
    {
        Task<List<PaymentDto>> GetByDueAsync(int dueId);
        Task<PaymentDto?> GetByIdAsync(int id);
        Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
        Task DeleteAsync(int id);
    }
}

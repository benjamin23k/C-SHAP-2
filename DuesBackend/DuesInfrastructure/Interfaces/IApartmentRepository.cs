using Dues.Domain.Entities;

namespace Dues.Infrastructure.Interfaces
{
    public interface IApartmentRepository
    {
        Task<List<Apartment>> GetAllAsync();
        Task<Apartment?> GetByIdAsync(int id);
        Task<Apartment> AddAsync(Apartment apartment);
        Task UpdateAsync(Apartment apartment);
        Task DeleteAsync(Apartment apartment);
    }
}

using Dues.Domain.Entities;

namespace Dues.Infrastructure.Interfaces
{
    public interface IApartmentRepository
    {
        Task<List<Apartament>> GetAllAsync();
        Task<Apartament?> GetByIdAsync(int id);
        Task<Apartament> AddAsync(Apartament apartment);
        Task UpdateAsync(Apartament apartment);
        Task DeleteAsync(Apartament apartment);
    }
}

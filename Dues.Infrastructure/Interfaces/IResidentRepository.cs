using Dues.Domain.Entities;

namespace Dues.Infrastructure.Interfaces
{
    public interface IResidentRepository
    {
        Task<List<Resident>> GetAllAsync();
        Task<Resident?> GetByIdAsync(int id);
        Task<Resident> AddAsync(Resident resident);
        Task UpdateAsync(Resident resident);
        Task DeleteAsync(Resident resident);
    }
}

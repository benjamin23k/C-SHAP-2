using Dues.Domain.Entities;

namespace Dues.Infrastructure.Interfaces
{
    public interface IDueRepository
    {
        Task<List<Due>> GetAllAsync();
        Task<Due?> GetByIdAsync(int id);
        Task<List<Due>> GetByApartmentAsync(int apartmentId);
        Task<Due> AddAsync(Due due);
        Task UpdateAsync(Due due);
        Task DeleteAsync(Due due);
        Task<List<Due>> GenerateMonthlyAsync(int month, int year);
        Task<int> UpdateOverdueAsync();
        Task<List<(Apartment Apartment, decimal TotalDebt, int OverdueCount)>> GetDebtReportAsync();
    }
}

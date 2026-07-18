using Dues.Business.Dtos;

namespace Dues.Business.Contract
{
    public interface IDueService
    {
        Task<List<DueDto>> GetAllAsync();
        Task<DueDto?> GetByIdAsync(int id);
        Task<List<DueDto>> GetByApartmentAsync(int apartmentId);
        Task<DueDto> CreateAsync(CreateDueDto dto);
        Task UpdateAsync(int id, UpdateDueDto dto);
        Task DeleteAsync(int id);
        Task<List<DueDto>> GenerateMonthlyAsync(int month, int year);
        Task<int> UpdateOverdueAsync();
        Task<List<DebtReportDto>> GetDebtReportAsync();
    }
}

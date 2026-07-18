using Dues.Business.Dtos;

namespace Dues.Business.Contract
{
    public interface IResidentService
    {
        Task<List<ResidentDto>> GetAllAsync();
        Task<ResidentDto?> GetByIdAsync(int id);
        Task<ResidentDto> CreateAsync(CreateResidentDto dto);
        Task UpdateAsync(int id, UpdateResidentDto dto);
        Task DeleteAsync(int id);
    }
}

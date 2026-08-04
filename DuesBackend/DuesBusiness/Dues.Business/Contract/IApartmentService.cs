using Dues.Business.Dtos;

namespace Dues.Business.Contract
{
    public interface IApartmentService
    {
        Task<List<ApartmentDto>> GetAllAsync();
        Task<ApartmentDto?> GetByIdAsync(int id);
        Task<ApartmentDto> CreateAsync(CreateApartmentDto dto);
        Task UpdateAsync(int id, UpdateApartmentDto dto);
        Task DeleteAsync(int id);
    }
}

using System.ComponentModel.DataAnnotations;
using Dues.Business.Contract;
using Dues.Business.Dtos;
using Dues.Domain.Entities;
using Dues.Infrastructure.Interfaces;

namespace Dues.Business.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IApartmentRepository _repository;

        public ApartmentService(IApartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ApartmentDto>> GetAllAsync()
        {
            var apartments = await _repository.GetAllAsync();
            return apartments.Select(ToDto).ToList();
        }

        public async Task<ApartmentDto?> GetByIdAsync(int id)
        {
            var apartment = await _repository.GetByIdAsync(id);
            return apartment is null ? null : ToDto(apartment);
        }

        public async Task<ApartmentDto> CreateAsync(CreateApartmentDto dto)
        {
            Validate(dto.Number, dto.MonthlyFee);

            var apartment = new Apartament
            {
                Number = dto.Number.Trim(),
                MonthlyFee = dto.MonthlyFee
            };

            var created = await _repository.AddAsync(apartment);
            return ToDto(created);
        }

        public async Task UpdateAsync(int id, UpdateApartmentDto dto)
        {
            Validate(dto.Number, dto.MonthlyFee);

            var apartment = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"There is no apartment with Id {id}.");

            apartment.Number = dto.Number.Trim();
            apartment.MonthlyFee = dto.MonthlyFee;

            await _repository.UpdateAsync(apartment);
        }

        public async Task DeleteAsync(int id)
        {
            var apartment = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"There is no apartment with Id {id}.");

            await _repository.DeleteAsync(apartment);
        }

        
        private static void Validate(string number, decimal monthlyFee)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(number))
                errors.Add("The apartment number is required.");
            else if (number.Length > 20)
                errors.Add("The apartment number cannot exceed 20 characters.");

            if (monthlyFee <= 0)
                errors.Add("The monthly fee must be greater than 0.";

            if (errors.Count > 0)
                throw new ValidationException(string.Join(" ", errors));
        }

        private static ApartmentDto ToDto(Apartament apartment) => new()
        {
            Id = apartment.Id,
            Number = apartment.Number,
            MonthlyFee = apartment.MonthlyFee
        };
    }
}

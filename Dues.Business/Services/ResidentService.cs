using System.ComponentModel.DataAnnotations;
using Dues.Business.Contract;
using Dues.Business.Dtos;
using Dues.Domain.Entities;
using Dues.Infrastructure.Interfaces;

namespace Dues.Business.Services
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentRepository _residentRepository;
        private readonly IApartmentRepository _apartmentRepository;

        public ResidentService(IResidentRepository residentRepository, IApartmentRepository apartmentRepository)
        {
            _residentRepository = residentRepository;
            _apartmentRepository = apartmentRepository;
        }

        public async Task<List<ResidentDto>> GetAllAsync()
        {
            var residents = await _residentRepository.GetAllAsync();
            return residents.Select(ToDto).ToList();
        }

        public async Task<ResidentDto?> GetByIdAsync(int id)
        {
            var resident = await _residentRepository.GetByIdAsync(id);
            return resident is null ? null : ToDto(resident);
        }

        public async Task<ResidentDto> CreateAsync(CreateResidentDto dto)
        {
            await ValidateAsync(dto.Name, dto.Phone, dto.Email, dto.ApartmentId);

            var resident = new Resident
            {
                Name = dto.Name.Trim(),
                Phone = dto.Phone?.Trim() ?? string.Empty,
                Email = dto.Email?.Trim() ?? string.Empty,
                ApartmentId = dto.ApartmentId
            };

            var created = await _residentRepository.AddAsync(resident);
            return ToDto(created);
        }

        public async Task UpdateAsync(int id, UpdateResidentDto dto)
        {
            await ValidateAsync(dto.Name, dto.Phone, dto.Email, dto.ApartmentId);

            var resident = await _residentRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"There is no resident with Id {id}.");

            resident.Name = dto.Name.Trim();
            resident.Phone = dto.Phone?.Trim() ?? string.Empty;
            resident.Email = dto.Email?.Trim() ?? string.Empty;
            resident.ApartmentId = dto.ApartmentId;

            await _residentRepository.UpdateAsync(resident);
        }

        public async Task DeleteAsync(int id)
        {
            var resident = await _residentRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"There is no resident with Id {id}.");

            await _residentRepository.DeleteAsync(resident);
        }

        private async Task ValidateAsync(string name, string phone, string email, int apartmentId)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(name))
                errors.Add("The name is required.");
            else if (name.Length > 30)
                errors.Add("The name cannot exceed 30 characters.");

            if (!string.IsNullOrEmpty(phone) && phone.Length > 20)
                errors.Add("The phone number cannot exceed 20 characters.");

            if (!string.IsNullOrEmpty(email))
            {
                if (email.Length > 50)
                    errors.Add("The email cannot exceed 50 characters.");
                else if (!new EmailAddressAttribute().IsValid(email))
                    errors.Add("The email format is not valid.");
            }

            if (apartmentId <= 0)
                errors.Add("You must specify a valid apartment.");
            else if (await _apartmentRepository.GetByIdAsync(apartmentId) is null)
                errors.Add($"There is no apartment with Id {apartmentId}.");

            if (errors.Count > 0)
                throw new ValidationException(string.Join(" ", errors));
        }


        private static ResidentDto ToDto(Resident resident) => new()
        {
            Id = resident.Id,
            Name = resident.Name,
            Phone = resident.Phone,
            Email = resident.Email,
            ApartmentId = resident.ApartmentId,
            ApartmentNumber = resident.Apartment?.Number
        };
    }
}

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
                ?? throw new KeyNotFoundException($"No existe un residente con Id {id}.");

            resident.Name = dto.Name.Trim();
            resident.Phone = dto.Phone?.Trim() ?? string.Empty;
            resident.Email = dto.Email?.Trim() ?? string.Empty;
            resident.ApartmentId = dto.ApartmentId;

            await _residentRepository.UpdateAsync(resident);
        }

        public async Task DeleteAsync(int id)
        {
            var resident = await _residentRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"No existe un residente con Id {id}.");

            await _residentRepository.DeleteAsync(resident);
        }

        // Validaciones explicitas de cada campo de la entidad Resident (via BasePerson),
        // incluida la regla de negocio de que el apartamento referenciado debe existir.
        private async Task ValidateAsync(string name, string phone, string email, int apartmentId)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(name))
                errors.Add("El nombre es obligatorio.");
            else if (name.Length > 30)
                errors.Add("El nombre no puede superar los 30 caracteres.");

            if (!string.IsNullOrEmpty(phone) && phone.Length > 20)
                errors.Add("El telefono no puede superar los 20 caracteres.");

            if (!string.IsNullOrEmpty(email))
            {
                if (email.Length > 50)
                    errors.Add("El correo no puede superar los 50 caracteres.");
                else if (!new EmailAddressAttribute().IsValid(email))
                    errors.Add("El correo no tiene un formato valido.");
            }

            if (apartmentId <= 0)
                errors.Add("Debe indicar un apartamento valido.");
            else if (await _apartmentRepository.GetByIdAsync(apartmentId) is null)
                errors.Add($"No existe un apartamento con Id {apartmentId}.");

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

using System.ComponentModel.DataAnnotations;
using Dues.Business.Contract;
using Dues.Business.Dtos;
using Dues.Domain.Entities;
using Dues.Infrastructure.Interfaces;

namespace Dues.Business.Services
{
    public class DueService : IDueService
    {
        private readonly IDueRepository _dueRepository;
        private readonly IApartmentRepository _apartmentRepository;

        public DueService(IDueRepository dueRepository, IApartmentRepository apartmentRepository)
        {
            _dueRepository = dueRepository;
            _apartmentRepository = apartmentRepository;
        }

        public async Task<List<DueDto>> GetAllAsync()
        {
            var dues = await _dueRepository.GetAllAsync();
            return dues.Select(ToDto).ToList();
        }

        public async Task<DueDto?> GetByIdAsync(int id)
        {
            var due = await _dueRepository.GetByIdAsync(id);
            return due is null ? null : ToDto(due);
        }

        public async Task<List<DueDto>> GetByApartmentAsync(int apartmentId)
        {
            var dues = await _dueRepository.GetByApartmentAsync(apartmentId);
            return dues.Select(ToDto).ToList();
        }

        public async Task<DueDto> CreateAsync(CreateDueDto dto)
        {
            await ValidateAsync(dto.ApartmentId, dto.Month, dto.Year, dto.Amount, dto.DueDate);

            var due = new Due
            {
                ApartmentId = dto.ApartmentId,
                Month = dto.Month,
                Year = dto.Year,
                Amount = dto.Amount,
                DueDate = dto.DueDate,
                Status = DueStatus.Pending
            };

            var created = await _dueRepository.AddAsync(due);
            return ToDto(created);
        }

        public async Task UpdateAsync(int id, UpdateDueDto dto)
        {
            ValidateAmountAndDate(dto.Amount, dto.DueDate);

            var due = await _dueRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"No existe una cuota con Id {id}.");

            due.Amount = dto.Amount;
            due.DueDate = dto.DueDate;
            due.Status = dto.Status;

            await _dueRepository.UpdateAsync(due);
        }

        public async Task DeleteAsync(int id)
        {
            var due = await _dueRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"No existe una cuota con Id {id}.");

            await _dueRepository.DeleteAsync(due);
        }

        public async Task<List<DueDto>> GenerateMonthlyAsync(int month, int year)
        {
            var errors = new List<string>();

            if (month < 1 || month > 12)
                errors.Add("El mes debe estar entre 1 y 12.");

            if (year < 2000 || year > 2100)
                errors.Add("El anio no es valido.");

            if (errors.Count > 0)
                throw new ValidationException(string.Join(" ", errors));

            var created = await _dueRepository.GenerateMonthlyAsync(month, year);
            return created.Select(ToDto).ToList();
        }

        public Task<int> UpdateOverdueAsync() => _dueRepository.UpdateOverdueAsync();

        public async Task<List<DebtReportDto>> GetDebtReportAsync()
        {
            var report = await _dueRepository.GetDebtReportAsync();

            return report.Select(r => new DebtReportDto
            {
                ApartmentId = r.Apartment.Id,
                ApartmentNumber = r.Apartment.Number,
                TotalDebt = r.TotalDebt,
                OverdueCount = r.OverdueCount
            }).ToList();
        }

        // Validaciones explicitas de cada campo de la entidad Due, incluida
        // la regla de negocio de que el apartamento referenciado debe existir.
        private async Task ValidateAsync(int apartmentId, int month, int year, decimal amount, DateTime dueDate)
        {
            var errors = new List<string>();

            if (apartmentId <= 0)
                errors.Add("Debe indicar un apartamento valido.");
            else if (await _apartmentRepository.GetByIdAsync(apartmentId) is null)
                errors.Add($"No existe un apartamento con Id {apartmentId}.");

            if (month < 1 || month > 12)
                errors.Add("El mes debe estar entre 1 y 12.");

            if (year < 2000 || year > 2100)
                errors.Add("El anio no es valido.");

            if (amount <= 0)
                errors.Add("El monto debe ser mayor a 0.");

            if (dueDate == default)
                errors.Add("Debe indicar una fecha de vencimiento.");

            if (errors.Count > 0)
                throw new ValidationException(string.Join(" ", errors));
        }

        private static void ValidateAmountAndDate(decimal amount, DateTime dueDate)
        {
            var errors = new List<string>();

            if (amount <= 0)
                errors.Add("El monto debe ser mayor a 0.");

            if (dueDate == default)
                errors.Add("Debe indicar una fecha de vencimiento.");

            if (errors.Count > 0)
                throw new ValidationException(string.Join(" ", errors));
        }

        private static DueDto ToDto(Due due) => new()
        {
            Id = due.Id,
            ApartmentId = due.ApartmentId,
            ApartmentNumber = due.Apartment?.Number,
            Month = due.Month,
            Year = due.Year,
            Amount = due.Amount,
            AmountPaid = due.AmountPaid,
            Balance = due.Balance,
            DueDate = due.DueDate,
            Status = due.Status
        };
    }
}

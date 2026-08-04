using System.ComponentModel.DataAnnotations;
using Dues.Business.Contract;
using Dues.Business.Dtos;
using Dues.Domain.Entities;
using Dues.Infrastructure.Interfaces;

namespace Dues.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IDueRepository _dueRepository;

        public PaymentService(IPaymentRepository paymentRepository, IDueRepository dueRepository)
        {
            _paymentRepository = paymentRepository;
            _dueRepository = dueRepository;
        }

        public async Task<List<PaymentDto>> GetByDueAsync(int dueId)
        {
            var payments = await _paymentRepository.GetByDueAsync(dueId);
            return payments.Select(ToDto).ToList();
        }

        public async Task<PaymentDto?> GetByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetWithDetailsAsync(id);
            return payment is null ? null : ToDto(payment);
        }

        public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
        {
            await ValidateAsync(dto.DueId, dto.Amount, dto.Method, dto.ReceiptNumber);

            var payment = new Payment
            {
                DueId = dto.DueId,
                Amount = dto.Amount,
                Method = dto.Method,
                ReceiptNumber = dto.ReceiptNumber?.Trim() ?? string.Empty,
                Date = DateTime.Now
            };

            var created = await _paymentRepository.CreateAsync(payment);
            return ToDto(created);
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"No existe un pago con Id {id}.");

            await _paymentRepository.DeleteAsync(payment);
        }

        // Validaciones explicitas de cada campo de la entidad Payment, incluida
        // la regla de negocio de no poder pagar mas del saldo pendiente de la cuota.
        private async Task ValidateAsync(int dueId, decimal amount, PaymentMethod method, string receiptNumber)
        {
            var errors = new List<string>();

            if (dueId <= 0)
            {
                errors.Add("Debe indicar una cuota valida.");
            }
            else
            {
                var due = await _dueRepository.GetByIdAsync(dueId);
                if (due is null)
                    errors.Add($"No existe una cuota con Id {dueId}.");
                else if (amount > due.Balance)
                    errors.Add($"El monto del pago ({amount:C}) no puede superar el saldo pendiente ({due.Balance:C}).");
            }

            if (amount <= 0)
                errors.Add("El monto del pago debe ser mayor a 0.");

            if (!Enum.IsDefined(typeof(PaymentMethod), method))
                errors.Add("El metodo de pago no es valido.");

            if (!string.IsNullOrEmpty(receiptNumber) && receiptNumber.Length > 50)
                errors.Add("El numero de recibo no puede superar los 50 caracteres.");

            if (errors.Count > 0)
                throw new ValidationException(string.Join(" ", errors));
        }

        private static PaymentDto ToDto(Payment payment) => new()
        {
            Id = payment.Id,
            DueId = payment.DueId,
            Amount = payment.Amount,
            Date = payment.Date,
            Method = payment.Method,
            ReceiptNumber = payment.ReceiptNumber
        };
    }
}

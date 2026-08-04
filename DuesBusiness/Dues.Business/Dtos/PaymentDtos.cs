using System.ComponentModel.DataAnnotations;
using Dues.Domain.Entities;

namespace Dues.Business.Dtos
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int DueId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public PaymentMethod Method { get; set; }
        public string ReceiptNumber { get; set; } = string.Empty;
    }

    public class CreatePaymentDto
    {
        [Required]
        public int DueId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }

        [MaxLength(50)]
        public string? ReceiptNumber { get; set; }
    }
}

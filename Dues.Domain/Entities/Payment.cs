using System.ComponentModel.DataAnnotations;
using Dues.Domain.Core;

namespace Dues.Domain.Entities
{
    public enum PaymentMethod { Cash, Transfer, Card, Check }

    public class Payment : BaseEntity
    {
        public int DueId { get; set; }

        public Due? Due { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public PaymentMethod Method { get; set; }

        [MaxLength(50)]
        public string ReceiptNumber { get; set; } = string.Empty;
    }
}

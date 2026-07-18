using System.ComponentModel.DataAnnotations;
using Dues.Domain.Core;

namespace Dues.Domain.Entities
{
    public enum DueStatus { Pending, Paid, Overdue, Partial }

    public class Due : BaseEntity
    {
        public int ApartmentId { get; set; }

        public Apartament? Apartment { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime DueDate { get; set; }

        public DueStatus Status { get; set; } = DueStatus.Pending;

        public List<Payment> Payments { get; set; } = new();

        public decimal Balance => Amount - AmountPaid;
    }
}

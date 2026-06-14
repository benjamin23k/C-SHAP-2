using System.ComponentModel.DataAnnotations;
using DuesApi.Core;

namespace DuesApi.Models
{
    public class Resident : BasePerson
    {
        public int ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
    }

    public class Apartment : BaseEntity
    {
        [Required, MaxLength(20)]
        public string Number { get; set; } = string.Empty;

        public decimal MonthlyFee { get; set; }

        public List<Resident> Residents { get; set; } = new();
        public List<Due> Dues { get; set; } = new();
    }

    public enum DueStatus { Pending, Paid, Overdue, Partial }

    public class Due : BaseEntity
    {
        public int ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }

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

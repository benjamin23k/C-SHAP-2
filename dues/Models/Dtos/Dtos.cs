using DuesApi.Models;

namespace DuesApi.Models.Dtos
{
    public class ResidentDto
    {
        public string Name { get; set; } = string.Empty;
        public int ApartmentId { get; set; }
    }

    public class ApartmentDto
    {
        public string Number { get; set; } = string.Empty;
        public decimal MonthlyFee { get; set; }
    }

    public class DueDto
    {
        public int ApartmentId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class PaymentDto
    {
        public int DueId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
    }

    public class DebtReportDto
    {
        public int ApartmentId { get; set; }
        public string ApartmentNumber { get; set; } = string.Empty;
        public decimal TotalDebt { get; set; }
        public int PendingDues { get; set; }
    }
}

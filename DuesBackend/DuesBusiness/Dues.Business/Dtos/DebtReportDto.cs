namespace Dues.Business.Dtos
{
    public class DebtReportDto
    {
        public int ApartmentId { get; set; }
        public string ApartmentNumber { get; set; } = string.Empty;
        public decimal TotalDebt { get; set; }
        public int OverdueCount { get; set; }
    }
}

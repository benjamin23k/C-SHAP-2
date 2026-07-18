namespace DuesApi.Models.Dtos
{
    public class DebtReportDto
    {
        public int ApartmentId { get; }
        public string ApartmentNumber { get; }
        public decimal TotalDebt { get; }
        public int OverdueCount { get; }

        public DebtReportDto(int apartmentId, string apartmentNumber, decimal totalDebt, int overdueCount)
        {
            ApartmentId = apartmentId;
            ApartmentNumber = apartmentNumber;
            TotalDebt = totalDebt;
            OverdueCount = overdueCount;
        }
    }
}


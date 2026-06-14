using DuesApi.Models;

namespace DuesApi.Models.Dtos
{
    public record ResidentDto(string Name, int ApartmentId);

    public record ApartmentDto(string Number, decimal MonthlyFee);

    public record DueDto(int ApartmentId, int Month, int Year, decimal Amount, DateTime DueDate);

    public record PaymentDto(int DueId, decimal Amount, PaymentMethod Method);

    public record DebtReportDto(int ApartmentId, string ApartmentNumber, decimal TotalDebt, int PendingDues);
}

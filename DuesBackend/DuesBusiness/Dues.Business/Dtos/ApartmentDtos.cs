using System.ComponentModel.DataAnnotations;

namespace Dues.Business.Dtos
{
    public class ApartmentDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public decimal MonthlyFee { get; set; }
    }

    public class CreateApartmentDto
    {
        [Required]
        public string Number { get; set; } = string.Empty;

        [Required]
        public decimal MonthlyFee { get; set; }
    }

    public class UpdateApartmentDto
    {
        [Required]
        public string Number { get; set; } = string.Empty;

        [Required]
        public decimal MonthlyFee { get; set; }
    }
}

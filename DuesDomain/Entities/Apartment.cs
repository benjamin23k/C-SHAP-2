using System.ComponentModel.DataAnnotations;

namespace Dues.Domain.Entities
{
    public class Apartment
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Number { get; set; } = string.Empty;

        public decimal MonthlyFee { get; set; }
    }
}

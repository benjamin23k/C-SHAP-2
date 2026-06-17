using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DuesApi.Models.Dtos
{
    public class ApartmentDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public decimal MonthlyFee { get; set; }
    }
}

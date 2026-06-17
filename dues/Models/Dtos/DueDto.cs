using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DuesApi.Models.Dtos
{
    public class DueDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public int ApartmentId { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Dues.Domain.Entities;

namespace DuesApi.Models.Dtos
{
    public class PaymentDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public int DueId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }
    }
}

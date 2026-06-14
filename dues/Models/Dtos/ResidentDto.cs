using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DuesApi.Models.Dtos
{
    public class ResidentDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ApartmentId { get; set; }
    }
}

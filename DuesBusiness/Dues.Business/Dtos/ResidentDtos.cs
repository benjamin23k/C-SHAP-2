using System.ComponentModel.DataAnnotations;

namespace Dues.Business.Dtos
{
    public class ResidentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int ApartmentId { get; set; }
        public string? ApartmentNumber { get; set; }
    }

    public class CreateResidentDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        [Required]
        public int ApartmentId { get; set; }
    }

    public class UpdateResidentDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        [Required]
        public int ApartmentId { get; set; }
    }
}

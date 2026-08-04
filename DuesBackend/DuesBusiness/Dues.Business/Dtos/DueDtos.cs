using System.ComponentModel.DataAnnotations;
using Dues.Domain.Entities;

namespace Dues.Business.Dtos
{
    public class DueDto
    {
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public string? ApartmentNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public DateTime DueDate { get; set; }
        public DueStatus Status { get; set; }
    }

    public class CreateDueDto
    {
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

    public class UpdateDueDto
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public DueStatus Status { get; set; }
    }
}

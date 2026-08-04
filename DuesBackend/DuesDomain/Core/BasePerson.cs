using System.ComponentModel.DataAnnotations;

namespace Dues.Domain.Core
{
    public abstract class BasePerson : BaseEntity
    {
        [Required, MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;
    }
}

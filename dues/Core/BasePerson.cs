using System.ComponentModel.DataAnnotations;

namespace DuesApi.Core
{
  
    public abstract class BasePerson : BaseEntity
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;
    }
}

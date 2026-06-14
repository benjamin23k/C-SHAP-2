using System.ComponentModel.DataAnnotations;

namespace DuesApi.Core
{
    /// <summary>
    /// Base class for any person-like entity in the system (e.g. Resident,
    /// and future entities like Administrator/Guard).
    /// </summary>
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

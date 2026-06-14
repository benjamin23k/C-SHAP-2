namespace DuesApi.Core
{
    /// <summary>
    /// Base class for entities, providing a primary key and audit fields
    /// shared across the domain (Apartment, Due, Payment, etc.).
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}

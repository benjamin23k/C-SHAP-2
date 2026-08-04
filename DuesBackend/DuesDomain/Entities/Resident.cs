using Dues.Domain.Core;

namespace Dues.Domain.Entities
{
    public class Resident : BasePerson
    {
        public int ApartmentId { get; set; }

        public Apartment? Apartment { get; set; }
    }
}

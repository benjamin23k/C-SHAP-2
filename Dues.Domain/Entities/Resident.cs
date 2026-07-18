using Dues.Domain.Core;

namespace Dues.Domain.Entities
{
    public class Resident : BasePerson
    {
        public int ApartmentId { get; set; }

        public Apartament? Apartment { get; set; }
    }
}

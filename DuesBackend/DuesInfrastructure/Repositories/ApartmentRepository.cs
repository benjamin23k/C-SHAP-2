using Dues.Domain.Entities;
using Dues.Infrastructure.Context;
using Dues.Infrastructure.Core;
using Dues.Infrastructure.Interfaces;

namespace Dues.Infrastructure.Repositories
{
    public class ApartmentRepository : BaseRepository<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(DuesContext db) : base(db)
        {
        }
    }
}

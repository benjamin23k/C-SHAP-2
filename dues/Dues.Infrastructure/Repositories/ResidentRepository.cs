using Dues.Domain.Entities;
using Dues.Infrastructure.Context;
using Dues.Infrastructure.Core;
using Dues.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dues.Infrastructure.Repositories
{
    public class ResidentRepository : BaseRepository<Resident>, IResidentRepository
    {
        public ResidentRepository(DuesContext db) : base(db)
        {
        }

        public override async Task<List<Resident>> GetAllAsync() =>
            await Db.Residents.Include(r => r.Apartment).ToListAsync();

        public override async Task<Resident?> GetByIdAsync(int id) =>
            await Db.Residents.Include(r => r.Apartment).FirstOrDefaultAsync(r => r.Id == id);

        public override async Task<Resident> AddAsync(Resident resident) =>
            await base.AddAsync(resident);
    }
}

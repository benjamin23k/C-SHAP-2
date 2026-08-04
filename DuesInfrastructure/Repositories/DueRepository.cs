using Dues.Domain.Entities;
using Dues.Infrastructure.Context;
using Dues.Infrastructure.Core;
using Dues.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dues.Infrastructure.Repositories
{
    public class DueRepository : BaseRepository<Due>, IDueRepository
    {
        public DueRepository(DuesContext db) : base(db)
        {
        }

        public override async Task<List<Due>> GetAllAsync() =>
            await Db.Dues.Include(d => d.Apartment).Include(d => d.Payments).ToListAsync();

        public override async Task<Due?> GetByIdAsync(int id) =>
            await Db.Dues
                .Include(d => d.Apartment)
                .Include(d => d.Payments)
                .FirstOrDefaultAsync(d => d.Id == id);

        public async Task<List<Due>> GetByApartmentAsync(int apartmentId) =>
            await Db.Dues
                .Where(d => d.ApartmentId == apartmentId)
                .Include(d => d.Payments)
                .ToListAsync();

        public override async Task<Due> AddAsync(Due due) =>
            await base.AddAsync(due);

        public async Task<List<Due>> GenerateMonthlyAsync(int month, int year)
        {
            var apartments = await Db.Apartments.ToListAsync();
            var newDues = new List<Due>();

            foreach (var apt in apartments)
            {
                bool exists = await Db.Dues.AnyAsync(d =>
                    d.ApartmentId == apt.Id && d.Month == month && d.Year == year);

                if (exists) continue;

                var due = new Due
                {
                    ApartmentId = apt.Id,
                    Month = month,
                    Year = year,
                    Amount = apt.MonthlyFee,
                    DueDate = new DateTime(year, month, 5),
                    Status = DueStatus.Pending
                };

                Db.Dues.Add(due);
                newDues.Add(due);
            }

            await Db.SaveChangesAsync();
            return newDues;
        }

        public async Task<int> UpdateOverdueAsync()
        {
            var now = DateTime.Now;
            var overdue = await Db.Dues
                .Where(d => d.Status == DueStatus.Pending && d.DueDate < now)
                .ToListAsync();

            foreach (var due in overdue)
                due.Status = DueStatus.Overdue;

            await Db.SaveChangesAsync();
            return overdue.Count;
        }

        public async Task<List<(Apartment Apartment, decimal TotalDebt, int OverdueCount)>> GetDebtReportAsync()
        {
            var apartments = await Db.Apartments.ToListAsync();
            var dues = await Db.Dues.Where(d => d.Status != DueStatus.Paid).ToListAsync();

            return apartments
                .Select(apt =>
                {
                    var aptDues = dues.Where(d => d.ApartmentId == apt.Id).ToList();
                    return (apt, aptDues.Sum(d => d.Balance), aptDues.Count);
                })
                .Where(r => r.Item2 > 0)
                .ToList();
        }
    }
}

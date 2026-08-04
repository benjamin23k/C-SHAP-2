using Microsoft.EntityFrameworkCore;
using Dues.Domain.Entities;

namespace Dues.Infrastructure.Context
{
    public class DuesContext : DbContext
    {
        public DuesContext(DbContextOptions<DuesContext> options) : base(options)
        {
        }

        public DbSet<Resident> Residents => Set<Resident>();
        public DbSet<Apartment> Apartments => Set<Apartment>();
        public DbSet<Due> Dues => Set<Due>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Apartment>()
                .Property(x => x.MonthlyFee)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Due>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Due>()
                .Property(x => x.AmountPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);
        }
    }
}

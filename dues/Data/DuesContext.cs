using Microsoft.EntityFrameworkCore;
using DuesApi.Models;

namespace DuesApi.Data
{
    public class DuesContext : DbContext
    {
        public DuesContext(DbContextOptions<DuesContext> options) : base(options) { }

        public DbSet<Resident> Residents => Set<Resident>();
        public DbSet<Apartment> Apartments => Set<Apartment>();
        public DbSet<Due> Dues => Set<Due>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apartment>()
                .HasMany(a => a.Residents)
                .WithOne(r => r.Apartment)
                .HasForeignKey(r => r.ApartmentId);

            modelBuilder.Entity<Apartment>()
                .HasMany(a => a.Dues)
                .WithOne(d => d.Apartment)
                .HasForeignKey(d => d.ApartmentId);

            modelBuilder.Entity<Due>()
                .HasMany(d => d.Payments)
                .WithOne(p => p.Due)
                .HasForeignKey(p => p.DueId);

            // decimal precision for SQL Server
            modelBuilder.Entity<Apartment>().Property(a => a.MonthlyFee).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Due>().Property(d => d.Amount).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Due>().Property(d => d.AmountPaid).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(10,2)");

            // Seed data
            modelBuilder.Entity<Apartment>().HasData(
                new Apartment { Id = 1, Number = "101", MonthlyFee = 2500m },
                new Apartment { Id = 2, Number = "102", MonthlyFee = 2500m },
                new Apartment { Id = 3, Number = "201", MonthlyFee = 3000m }
            );

            modelBuilder.Entity<Resident>().HasData(
                new Resident { Id = 1, Name = "Wilson Rosario", ApartmentId = 1 },
                new Resident { Id = 2, Name = "Maria Perez", ApartmentId = 2 }
            );
        }
    }
}

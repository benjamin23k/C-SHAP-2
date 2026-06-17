using Microsoft.EntityFrameworkCore;
using DuesApi.Models;

namespace DuesApi.Data
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

        
    }
}

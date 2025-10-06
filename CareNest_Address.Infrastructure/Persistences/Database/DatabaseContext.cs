using CareNest_Address.Domain.Entitites;
using Microsoft.EntityFrameworkCore;


namespace CareNest_Address.Infrastructure.Persistences.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Address> addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}

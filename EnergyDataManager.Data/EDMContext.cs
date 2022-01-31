using EnergyDataManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EnergyDataManager.Data
{
    public class EDMContext : DbContext
    {
        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<Reading> Readings { get; set; }
        private readonly IConfiguration _configuration;

        public EDMContext(IConfiguration config,
            DbContextOptions<EDMContext> options) : base(options)
        {
            _configuration = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("EDM_DB");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new ReadingConfiguration());
        }
    }
}

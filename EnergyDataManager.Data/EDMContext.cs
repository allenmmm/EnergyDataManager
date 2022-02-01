using EnergyDataManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace EnergyDataManager.Data
{
    public class EDMContext : DbContext
    {
        internal DbSet<Domain.Account> Accounts { get; set; }
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
                var connectionString =
                    _configuration.GetConnectionString("EDM_DB");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new ReadingConfiguration());
        }

        public int Seed(
            IEnumerable<EnergyDataReader.Account.File.Account> accounts)
        {
            int rowsSaved = 0;
            if(! Accounts.Any())
            {
                foreach (var account in accounts)
                {
                    Add(new Domain.Account(account));
                }
                rowsSaved = SaveChanges();
            }
            return rowsSaved;
        }
    }
}

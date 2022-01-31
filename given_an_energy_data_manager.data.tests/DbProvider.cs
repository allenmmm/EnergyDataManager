using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace given_an_energy_data_manager.data.tests
{
    public abstract class DbProvider<TContext> : IDisposable where TContext : DbContext
    {
        protected TContext _context;
        private readonly string _SeedingScriptPath;

        public DbProvider(string seedingScriptPath = "")
        {
            _SeedingScriptPath = seedingScriptPath;
        }

        public DbProvider(
            DbContextOptions contextOptions,
            string seedingScriptPath = "")
        {
            _SeedingScriptPath = seedingScriptPath;
            ConfigureContext(contextOptions);
        }

        protected void PostConifgureContextOptions(
            DbContextOptions contextOptions)
        {
            ConfigureContext(contextOptions);
        }

        private void ConfigureContext(
            DbContextOptions contextOptions)
        {
            try
            {
                _context = (TContext)Activator.CreateInstance(
                    typeof(TContext), null,
                    contextOptions);
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected int SeedFromFile(string seedingScriptFileName)
        {
            return Seed(File.ReadAllText($"{_SeedingScriptPath}{seedingScriptFileName}"));
        }

        protected int Seed(string command)
        {
            int effectedRows = 0;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    effectedRows = _context.Database.ExecuteSqlRaw(command);
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return (effectedRows);
        }

        public virtual void Dispose()
        {
            if (_context != null)
            {
                _context.Database.EnsureDeleted();
                _context.Dispose();
                _context = null;
            }
        }
    }
}

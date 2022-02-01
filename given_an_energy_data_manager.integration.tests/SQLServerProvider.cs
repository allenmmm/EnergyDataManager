using Microsoft.EntityFrameworkCore;

namespace given_an_energy_data_manager.integration.tests
{
    public class SQLServerProvider<TContext> :
        DbProvider<TContext> where TContext : DbContext
    {
        public SQLServerProvider(
            string connectionString,
            string seedingScriptPath = null)
                : base(CreateDbContextOptions(
                    connectionString),
                    seedingScriptPath)
        {
        }

        public SQLServerProvider(
            string seedingScriptPath = null) : base(seedingScriptPath)
        {
        }

        protected void PostConifgureContextOptions(
                string connectionString)
        {
            base.PostConifgureContextOptions(
                CreateDbContextOptions(
                    connectionString));
        }


        private static DbContextOptions<TContext> CreateDbContextOptions(
            string connection)
        {
            return new DbContextOptionsBuilder<TContext>()
                  .UseSqlServer(connection).Options;
        }
    }
}

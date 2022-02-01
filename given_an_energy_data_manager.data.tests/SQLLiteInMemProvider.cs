using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace given_an_energy_data_manager.data.tests
{
    public class SQLLiteInMemProvider<TContext> :
        DbProvider<TContext>, IDisposable where TContext : DbContext
    {
        private readonly DbConnection _connection;

        private static DbConnection CreateInMemoryDatabase(string connection)
        {
            var connections = new SqliteConnection(connection);
            connections.Open();
            return connections;
        }

        public SQLLiteInMemProvider(string connection, string seedingScriptPath = null)
            : base(
                  new DbContextOptionsBuilder<TContext>()
                        .UseSqlite(CreateInMemoryDatabase(connection)).Options,
                  seedingScriptPath)
        {
            _connection = _context.Database.GetDbConnection();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}

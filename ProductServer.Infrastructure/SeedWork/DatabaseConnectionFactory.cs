using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace ProductServer.Infrastructure.SeedWork
{
    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly SqlServerOptions sqlServerOptions;

        public DatabaseConnectionFactory(IOptions<SqlServerOptions> sqlServerOptions)
        {
            this.sqlServerOptions = sqlServerOptions.Value;
        }

        public IDbConnection GetNewDbConnection()
        {
            return new SqlConnection(sqlServerOptions.SqlServerConnection);
        }
    }
}
using System.Data;

namespace ProductServer.Infrastructure.SeedWork
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection GetNewDbConnection();
    }
}
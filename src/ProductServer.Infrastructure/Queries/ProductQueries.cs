using Microsoft.EntityFrameworkCore;
using ProductServer.Application.Queries.Product;
using ProductServer.Infrastructure.SeedWork;

namespace ProductServer.Infrastructure.Queries
{
    internal class ProductQueries : IProductQueries
    {
        private readonly IDbContextFactory<DatabaseContext> dbContextFactory;

        public ProductQueries(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<bool> AnyByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
            return await dbContext.Products.AsNoTracking().AnyAsync(c => c.Id == id, cancellationToken).ConfigureAwait(false);
        }
    }
}

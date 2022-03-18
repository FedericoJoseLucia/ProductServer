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
            return await dbContext.Products
                .AsNoTracking()
                .AnyAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
            return await dbContext.Products
                .AsNoTracking()
                .Select(x => new ProductDto(x.Id, x.Denomination, x.Price, (ProductDto.ProductState)x.State.Id))
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}

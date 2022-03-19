using Microsoft.EntityFrameworkCore;
using ProductServer.Domain.Aggregates.Product;
using ProductServer.Domain.SeedWork;
using ProductServer.Infrastructure.SeedWork;

namespace ProductServer.Infrastructure.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext dbContext;

        public ProductRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork => dbContext;

        public void Add(Product product)
        {
            dbContext.Products.Add(product);
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
        }
    }
}

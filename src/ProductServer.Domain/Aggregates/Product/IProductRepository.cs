using ProductServer.Domain.SeedWork;

namespace ProductServer.Domain.Aggregates.Product
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Product product, CancellationToken cancellationToken = default);
    }
}

namespace ProductServer.Application.Queries.Product
{
    public interface IProductQueries
    {
        Task<bool> AnyByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}

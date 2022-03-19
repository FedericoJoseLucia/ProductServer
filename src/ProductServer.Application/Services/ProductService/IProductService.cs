namespace ProductServer.Application.Services.ProductService
{
    public interface IProductService
    {
        Task<bool> AnyByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}

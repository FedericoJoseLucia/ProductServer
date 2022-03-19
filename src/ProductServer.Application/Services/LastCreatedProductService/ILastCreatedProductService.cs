namespace ProductServer.Application.Services.LastCreatedProductService
{
    public interface ILastCreatedProductService
    {
        Task<LastCreatedProductDto?> GetAsync(CancellationToken cancellationToken = default);
        Task WriteToCacheAtomicallyAsync(LastCreatedProductDto product, CancellationToken cancellationToken = default);
    }
}

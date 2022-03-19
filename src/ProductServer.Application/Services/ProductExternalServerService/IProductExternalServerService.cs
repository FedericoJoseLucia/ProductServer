namespace ProductServer.Application.Services.ProductExternalServerService
{
    public interface IProductExternalServerService
    {
        Task<ProductExternalDataDto?> GetProductExternalDataByCode(int code, CancellationToken cancellationToken = default);
    }
}

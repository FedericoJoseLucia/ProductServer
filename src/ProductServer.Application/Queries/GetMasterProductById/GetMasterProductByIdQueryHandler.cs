using ProductServer.Application.SeedWork;
using ProductServer.Application.Services.LastCreatedProductService;
using ProductServer.Application.Services.ProductExternalServerService;
using ProductServer.Application.Services.ProductService;

namespace ProductServer.Application.Queries.GetMasterProductById
{
    internal class GetMasterProductByIdQueryHandler : IQueryHandler<GetMasterProductByIdQuery, MasterProductDto?>
    {
        private readonly IProductService productService;
        private readonly ILastCreatedProductService lastCreatedProductService;
        private readonly IProductExternalServerService productExternalServerService;

        public GetMasterProductByIdQueryHandler(IProductService productService, ILastCreatedProductService lastCreatedProductService, IProductExternalServerService productExternalServerService)
        {
            this.productService = productService;
            this.lastCreatedProductService = lastCreatedProductService;
            this.productExternalServerService = productExternalServerService;
        }

        public async Task<MasterProductDto?> Handle(GetMasterProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productService.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

            if(product is null)
                return null;

            var getLastCreatedProductTask = lastCreatedProductService.GetAsync(cancellationToken);
            var getProductExternalDataTask = productExternalServerService.GetProductExternalDataByCode(product.ExternalCode, cancellationToken);

            await Task.WhenAll(getLastCreatedProductTask, getProductExternalDataTask).ConfigureAwait(false);

            var lastCreatedProduct = getLastCreatedProductTask.Result;
            var productExternalData = getProductExternalDataTask.Result;

            return new MasterProductDto(
                id: product.Id, 
                externalCode: product.ExternalCode, 
                denomination: product.Denomination, 
                price: product.Price, 
                state: (MasterProductDto.ProductState)product.State,
                lastCreatedProductId: lastCreatedProduct?.Id,
                lastCreatedProductDenomination: lastCreatedProduct?.Denomination,
                department: productExternalData?.Department,
                stock: productExternalData?.Stock);
        }
    }
}

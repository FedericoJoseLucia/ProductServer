using ProductServer.Application.SeedWork;
using ProductServer.Application.Services.ProductService;

namespace ProductServer.Application.Queries.GetProductById
{
    internal class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductService productService;

        public GetProductByIdQueryHandler(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await productService.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        }
    }
}

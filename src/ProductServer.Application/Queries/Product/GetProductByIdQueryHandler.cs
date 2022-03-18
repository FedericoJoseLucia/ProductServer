using ProductServer.Application.SeedWork;

namespace ProductServer.Application.Queries.Product
{
    internal class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductQueries productQueries;

        public GetProductByIdQueryHandler(IProductQueries productQueries)
        {
            this.productQueries = productQueries;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await productQueries.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        }
    }
}

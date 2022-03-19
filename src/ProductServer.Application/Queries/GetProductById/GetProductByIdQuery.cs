using ProductServer.Application.SeedWork;
using ProductServer.Application.Services.ProductService;

namespace ProductServer.Application.Queries.GetProductById
{
    public class GetProductByIdQuery : IQuery<ProductDto?>
    {
        public GetProductByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }

    }
}

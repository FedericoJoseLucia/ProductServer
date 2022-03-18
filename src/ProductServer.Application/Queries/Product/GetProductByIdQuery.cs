using ProductServer.Application.SeedWork;

namespace ProductServer.Application.Queries.Product
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

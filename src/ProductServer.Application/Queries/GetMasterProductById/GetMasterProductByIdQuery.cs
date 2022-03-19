using ProductServer.Application.SeedWork;

namespace ProductServer.Application.Queries.GetMasterProductById
{
    public class GetMasterProductByIdQuery : IQuery<MasterProductDto?>
    {
        public GetMasterProductByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }

    }
}

using ProductServer.Application.Queries.Product;
using ProductServer.Application.SeedWork;
using ProductServer.Domain.Aggregates.Product;

namespace ProductServer.Application.Commands.CreateProduct
{
    internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductQueries productQueries;

        public CreateProductCommandHandler(IProductRepository productRepository, IProductQueries productQueries)
        {
            this.productRepository = productRepository;
            this.productQueries = productQueries;
        }

        public async Task<CommandResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            bool exists = await productQueries.AnyByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

            if (exists)
                return CommandResult.Error(Resources.Application.ProductExists);

            Product aggregate = new(request.Id, request.Denomination, request.Price);

            productRepository.Add(aggregate);

            await productRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return CommandResult.Success();
        }
    }
}

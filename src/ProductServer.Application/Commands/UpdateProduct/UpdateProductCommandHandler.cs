using ProductServer.Application.SeedWork;
using ProductServer.Domain.Aggregates.Product;

namespace ProductServer.Application.Commands.UpdateProduct
{
    internal class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly IProductRepository productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<CommandResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product? aggregate = await productRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

            if (aggregate is null)
                return CommandResult.Error(Resources.Application.ProductNotFound);

            bool hasChanges = false;

            if (request.Denomination is not null && aggregate.Denomination != request.Denomination)
            {
                aggregate.UpdateDenomination(request.Denomination);
                hasChanges = true;
            }

            if (request.Price is not null && aggregate.Price != request.Price)
            {
                aggregate.UpdatePrice(request.Price.Value);
                hasChanges = true;
            }

            if (request.State is not null && aggregate.State.Id != (int)request.State)
            {
                aggregate.UpdateState(ProductState.GetById((int)request.State));
                hasChanges = true;
            }

            if(hasChanges)
                await productRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return CommandResult.Success();
        }
    }
}

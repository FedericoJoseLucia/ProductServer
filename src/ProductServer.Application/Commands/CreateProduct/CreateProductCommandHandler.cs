using ProductServer.Application.SeedWork;
using ProductServer.Application.Services.ProductService;
using ProductServer.Domain.Aggregates.Product;

namespace ProductServer.Application.Commands.CreateProduct
{
    internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductService productService;

        public CreateProductCommandHandler(IProductRepository productRepository, IProductService productService)
        {
            this.productRepository = productRepository;
            this.productService = productService;
        }

        public async Task<CommandResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            bool exists = await productService.AnyByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

            if (exists)
                return CommandResult.Error(Resources.Application.ProductExists);

            Product aggregate = new(request.Id, request.ExternalCode, request.Denomination, request.Price);

            productRepository.Add(aggregate);

            await productRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return CommandResult.Success();
        }
    }
}

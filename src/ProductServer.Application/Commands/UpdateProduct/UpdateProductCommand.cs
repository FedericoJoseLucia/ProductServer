using ProductServer.Application.SeedWork;

namespace ProductServer.Application.Commands.UpdateProduct
{
    public class UpdateProductCommand : ICommand
    {
        public UpdateProductCommand(Guid id, string? denomination, decimal? price, ProductState? state)
        {
            Id = id;
            Denomination = denomination;
            Price = price;
            State = state;
        }

        public Guid Id { get; private set; }
        public string? Denomination { get; private set; }
        public decimal? Price { get; private set; }
        public ProductState? State { get; private set; }

        public enum ProductState
        {
            Enabled = 1,
            Suspended = 2,
            Disabled = 3,
        }
    }
}

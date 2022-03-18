using ProductServer.Application.SeedWork;

namespace ProductServer.Application.Commands.CreateProduct
{
    public class CreateProductCommand : ICommand
    {
        public CreateProductCommand(Guid id, string denomination, decimal price)
        {
            Id = id;
            Denomination = denomination;
            Price = price;
        }

        public Guid Id { get; private set; }
        public string Denomination { get; private set; }
        public decimal Price { get; private set; }
    }
}

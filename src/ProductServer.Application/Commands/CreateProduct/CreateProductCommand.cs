using ProductServer.Application.SeedWork;

namespace ProductServer.Application.Commands.CreateProduct
{
    public class CreateProductCommand : ICommand
    {
        public CreateProductCommand(Guid id, int externalCode, string denomination, decimal price)
        {
            Id = id;
            ExternalCode = externalCode;
            Denomination = denomination;
            Price = price;
        }

        public Guid Id { get; private set; }
        public int ExternalCode { get; private set; }
        public string Denomination { get; private set; }
        public decimal Price { get; private set; }
    }
}

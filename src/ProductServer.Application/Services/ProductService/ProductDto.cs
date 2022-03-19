namespace ProductServer.Application.Services.ProductService
{
    public class ProductDto
    {
        public ProductDto(Guid id, int externalCode, string denomination, decimal price, ProductState state)
        {
            Id = id;
            ExternalCode = externalCode;
            Denomination = denomination;
            Price = price;
            State = state;
        }

        public Guid Id { get; private set; }
        public int ExternalCode { get; private set; }
        public string Denomination { get; private set; }
        public decimal Price { get; private set; }
        public ProductState State { get; private set; }

        public enum ProductState
        {
            Enabled = 1,
            Suspended = 2,
            Disabled = 3,
        }
    }
}

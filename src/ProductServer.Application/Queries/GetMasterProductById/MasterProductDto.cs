namespace ProductServer.Application.Queries.GetMasterProductById
{
    public class MasterProductDto
    {
        public MasterProductDto(Guid id, int externalCode, string denomination, decimal price, ProductState state, Guid? lastCreatedProductId, string? lastCreatedProductDenomination, string? department, int? stock)
        {
            Id = id;
            ExternalCode = externalCode;
            Denomination = denomination;
            Price = price;
            State = state;
            LastCreatedProductId = lastCreatedProductId;
            LastCreatedProductDenomination = lastCreatedProductDenomination;
            Department = department;
            Stock = stock;
        }

        public Guid Id { get; private set; }
        public int ExternalCode { get; private set; }
        public string Denomination { get; private set; }
        public decimal Price { get; private set; }
        public ProductState State { get; private set; }

        public Guid? LastCreatedProductId { get; private set; }
        public string? LastCreatedProductDenomination { get; private set; }

        public string? Department { get; private set; }
        public int? Stock { get; private set; }

        public enum ProductState
        {
            Enabled = 1,
            Suspended = 2,
            Disabled = 3,
        }
    }
}

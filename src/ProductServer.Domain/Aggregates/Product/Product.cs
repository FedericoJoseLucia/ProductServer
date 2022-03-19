using ProductServer.Domain.Aggregates.Product.DomainEvents;
using ProductServer.Domain.SeedWork;

namespace ProductServer.Domain.Aggregates.Product
{
    public class Product : Entity<Guid>, IAggregateRoot
    {
        public Product(Guid id, int externalCode, string denomination, decimal price)
        {
            ValidateExternalCode(externalCode);
            ValidateDenomination(denomination);
            ValidatePrice(price);

            Id = id;
            ExternalCode = externalCode;
            Denomination = denomination;
            Price = price;
            CreatedDateUtc = DateTime.UtcNow;
            State = ProductState.Enabled;

            AddDomainEvent(new ProductCreatedEvent(Id, Denomination));
        }

        public int ExternalCode { get; private set; }
        public string Denomination { get; private set; }
        public decimal Price { get; private set; }
        public DateTime CreatedDateUtc { get; private set; }
        public ProductState State { get; private set; }


        public void UpdateDenomination(string newDenomination)
        {
            ValidateDenomination(newDenomination);
            Denomination = newDenomination;
        }

        public void UpdatePrice(decimal newPrice) 
        {
            ValidatePrice(newPrice);
            Price = newPrice;
        }

        public void UpdateState(ProductState newState)
        {
            State = newState;
        }

        private static void ValidateExternalCode(int externalCode)
        {
            if (externalCode <= 0 || externalCode >= 100)
                throw new ArgumentException(Resources.Domain.InvalidExternalCode, nameof(externalCode));
        }

        private static void ValidateDenomination(string denomination)
        {
            if (string.IsNullOrWhiteSpace(denomination))
                throw new ArgumentException(Resources.Domain.DenominationIsEmpty, nameof(denomination));
        }

        private static void ValidatePrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException(Resources.Domain.PriceIsLessThanZero, nameof(price));
        }
    }
}

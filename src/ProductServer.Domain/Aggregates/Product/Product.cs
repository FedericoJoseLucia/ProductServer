using ProductServer.Domain.SeedWork;

namespace ProductServer.Domain.Aggregates.Product
{
    public class Product : Entity<Guid>, IAggregateRoot
    {
        public Product(Guid id, string denomination, decimal price)
        {
            ValidateDenomination(denomination);
            ValidatePrice(price);

            Id = id;
            Denomination = denomination;
            Price = price;
            CreatedDateUtc = DateTime.UtcNow;
            State = ProductState.Enabled;
        }

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

        private static void ValidateDenomination(string newDenomination)
        {
            if (string.IsNullOrWhiteSpace(newDenomination))
                throw new ArgumentException(Resources.Domain.DenominationIsEmpty, nameof(newDenomination));
        }

        private static void ValidatePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentException(Resources.Domain.PriceIsLessThanZero, nameof(newPrice));
        }
    }
}

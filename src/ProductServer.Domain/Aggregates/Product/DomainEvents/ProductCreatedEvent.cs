using MediatR;

namespace ProductServer.Domain.Aggregates.Product.DomainEvents
{
    public class ProductCreatedEvent : INotification
    {
        public ProductCreatedEvent(Guid id, string denomination)
        {
            Id = id;
            Denomination = denomination;
        }

        public Guid Id { get; private set; }
        public string Denomination { get; private set; }
    }
}

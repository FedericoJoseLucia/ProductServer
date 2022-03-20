using MediatR;

namespace ProductServer.Domain.Aggregates.Product.DomainEvents
{
    public class ProductCreatedEvent : INotification
    {
        public ProductCreatedEvent(Guid id, int externalCode)
        {
            Id = id;
            ExternalCode = externalCode;
        }

        public Guid Id { get; private set; }
        public int ExternalCode { get; private set; }
    }
}

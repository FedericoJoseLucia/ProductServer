using MediatR;

// Microsoft SeedWork
// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/seedwork-domain-model-base-classes-interfaces

namespace ProductServer.Domain.SeedWork
{
    public abstract class Entity<TId>
    {
        int? _requestedHashCode;
        TId? _Id;
        private readonly List<INotification> domainEvents;

        public virtual TId? Id
        {
            get
            {
                return _Id;
            }
            protected set
            {

                _Id = value;
            }
        }

        public Entity()
        {
            domainEvents = new List<INotification>();
        }

        public IReadOnlyList<INotification> DomainEvents => domainEvents.AsReadOnly();

        protected void AddDomainEvent(INotification eventItem)
        {
            domainEvents.Add(eventItem);

        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            if (domainEvents is null) return;
            domainEvents.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            domainEvents?.Clear();
        }

        public bool IsTransient()
        {
            return Object.Equals(Id, default(TId));
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not Entity<TId>)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;
            Entity<TId> item = (Entity<TId>)obj;
            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return Object.Equals(item.Id, Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id!.GetHashCode() ^ 31;
                // XOR for random distribution. See:
                // https://blogs.msdn.microsoft.com/ericlippert/2011/02/28/guidelines-and-rules-for-gethashcode/
                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }
        public static bool operator ==(Entity<TId> left, Entity<TId> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null));
            else
                return left.Equals(right);
        }
        public static bool operator !=(Entity<TId> left, Entity<TId> right)
        {
            return !(left == right);
        }
    }
}

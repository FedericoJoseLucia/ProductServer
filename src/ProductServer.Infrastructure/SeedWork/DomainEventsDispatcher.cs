using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using ProductServer.Domain.SeedWork;

namespace ProductServer.Infrastructure.SeedWork
{
    internal class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IPublisher publisher;
        private readonly ILogger<DomainEventsDispatcher> logger;

        public DomainEventsDispatcher(IPublisher publisher, ILogger<DomainEventsDispatcher> logger)
        {
            this.publisher = publisher;
            this.logger = logger;
        }

        public async Task DispatchDomainEventsAsync(DbContext context, CancellationToken cancellationToken = default)
        {
            var domainEntities = GetDomainEntities(context);

            var domainEvents = GetDomainEvents(domainEntities).ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                var domainEventName = domainEvent.GetType().Name;

                try
                {
                    logger.LogDebug("DISPATCHING DOMAIN EVENT {domainEventName}; PARAMETER: {@domainEventData}", domainEventName, domainEvent);

                    await publisher.Publish(domainEvent, cancellationToken).ConfigureAwait(false);

                    logger.LogDebug("DOMAIN EVENT {domainEventName} DISPATCHED", domainEventName);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "DOMAIN EVENT {domainEventName} DISPATCH FAILED; PARAMETER: {@domainEventData}", domainEventName, domainEvent);
                }
            }
        }

        private static IEnumerable<EntityEntry<IAggregateRoot>> GetDomainEntities(DbContext context) =>
            context.ChangeTracker.Entries<IAggregateRoot>()
                .Where(HasDomainEvents);

        private static IEnumerable<INotification> GetDomainEvents(IEnumerable<EntityEntry<IAggregateRoot>> domainEntities) =>
            domainEntities.SelectMany(x => x.Entity.DomainEvents);

        private static bool HasDomainEvents(EntityEntry<IAggregateRoot> e) =>
            e.Entity.DomainEvents is { } && e.Entity.DomainEvents.Any();
    }
}
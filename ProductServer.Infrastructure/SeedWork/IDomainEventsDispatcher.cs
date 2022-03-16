using Microsoft.EntityFrameworkCore;

namespace ProductServer.Infrastructure.SeedWork
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchDomainEventsAsync(DbContext context, CancellationToken cancellationToken = default);
    }
}
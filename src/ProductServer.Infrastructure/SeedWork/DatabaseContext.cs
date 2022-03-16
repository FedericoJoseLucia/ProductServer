using ProductServer.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ProductServer.Infrastructure.SeedWork
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        private readonly IDomainEventsDispatcher domainEventsDispatcher;

#pragma warning disable CS8618
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }
#pragma warning restore CS8618

        [ActivatorUtilitiesConstructor]
        public DatabaseContext(DbContextOptions<DatabaseContext> options, IDomainEventsDispatcher domainEventsDispatcher)
            : base(options)
        {
            this.domainEventsDispatcher = domainEventsDispatcher;
        }

        public new async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //Eventual Consistency
            await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await domainEventsDispatcher.DispatchDomainEventsAsync(this, cancellationToken).ConfigureAwait(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }
    }
}
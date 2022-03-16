using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductServer.Infrastructure.Repositories;

namespace ProductServer.Infrastructure.Bootstrap
{
    public class DatabaseBootstrap : IHostedService
    {
        private readonly ILogger<DatabaseBootstrap> logger;
        private readonly IServiceProvider serviceProvider;

        public DatabaseBootstrap(ILogger<DatabaseBootstrap> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogInformation("APPLYING DATABASE MIGRATIONS");

                    using (IServiceScope scope = serviceProvider.CreateScope())
                    {
                        using (DatabaseContext context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
                        {
                            await context.Database.MigrateAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
                            logger.LogInformation("DATABASE MIGRATIONS APPLIED SUCCESSFULLY");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "APPLYING DATABASE MIGRATIONS FAILED" + "{newLine}", Environment.NewLine);
                }

                await Task.Delay(3000, cancellationToken).ConfigureAwait(false);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

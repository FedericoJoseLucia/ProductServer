using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductServer.Domain.SeedWork;
using ProductServer.Infrastructure.SeedWork;

namespace ProductServer.Infrastructure
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.ConfigureOptions(configuration);

            services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();

            services.AddDataBaseContext(configuration, environment);

            services.AddScoped<IDatabaseConnectionFactory, DatabaseConnectionFactory>();

            services.AddScoped<IUnitOfWork>(provider => provider.GetService<DatabaseContext>()!);

            return services;
        }

        private static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SqlServerOptions>(
                configuration.GetSection(SqlServerOptions.ConfigurationKey).Bind);

            return services;
        }

        private static IServiceCollection AddDataBaseContext(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            string? assemblyName = typeof(DatabaseContext).Assembly.GetName().Name;

            SqlServerOptions connectionStringOptions = configuration
                .GetSection(SqlServerOptions.ConfigurationKey)
                .Get<SqlServerOptions>();

            services.AddDbContextFactory<DatabaseContext>(options =>
            {
                options.UseSqlServer(connectionStringOptions.SqlServerConnection,
                    builder =>
                    {
                        builder.MigrationsAssembly(assemblyName);
                        builder.EnableRetryOnFailure(2);
                    });

                if (environment.IsDevelopment() || environment.IsStaging())
                {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                }
            });

            return services;
        }
    }
}
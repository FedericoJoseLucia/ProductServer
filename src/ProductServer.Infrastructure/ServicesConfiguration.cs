using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductServer.Application.Services.LastCreatedProductService;
using ProductServer.Application.Services.ProductExternalServerService;
using ProductServer.Application.Services.ProductService;
using ProductServer.Domain.Aggregates.Product;
using ProductServer.Domain.SeedWork;
using ProductServer.Infrastructure.Repositories;
using ProductServer.Infrastructure.SeedWork;
using ProductServer.Infrastructure.Services;

namespace ProductServer.Infrastructure
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.ConfigureOptions(configuration);

            services.ConfigureDatabase(configuration, environment);

            services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();

            services.AddMemoryCache();

            services.AddRepositories();
            services.AddServices();
            services.AddHttpClients(configuration);

            return services;
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ILastCreatedProductService, LastCreatedProductService>();
        }
        private static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            ProductExternalServerAPIOptions options = configuration
                .GetSection(ProductExternalServerAPIOptions.ConfigurationKey)
                .Get<ProductExternalServerAPIOptions>();

            services.AddHttpClient<IProductExternalServerService, ProductExternalServerService>(x => x.BaseAddress = options.BaseAddressUri);
        }

        private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SqlServerOptions>(
                configuration.GetSection(SqlServerOptions.ConfigurationKey).Bind);

            services.Configure<ProductExternalServerAPIOptions>(
                configuration.GetSection(ProductExternalServerAPIOptions.ConfigurationKey).Bind);
        }

        private static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
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

            services.AddScoped<IDatabaseConnectionFactory, DatabaseConnectionFactory>();

            services.AddScoped<IUnitOfWork>(provider => provider.GetService<DatabaseContext>()!);
        }
    }
}
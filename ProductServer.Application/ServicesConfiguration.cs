using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductServer.Application.Behaviors;

namespace ProductServer.Application
{
    public static class ServicesConfiguration
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration _)
        {
            services.AddMediatR(typeof(ServicesConfiguration).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(StopwatchLoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandLoggingBehavior<,>));
        }
    }
}

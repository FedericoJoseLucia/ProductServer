using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProductServer.Application.Behaviors;

namespace ProductServer.Application
{
    public static class ServicesConfiguration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServicesConfiguration).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(StopwatchLoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandLoggingBehavior<,>));
        }
    }
}

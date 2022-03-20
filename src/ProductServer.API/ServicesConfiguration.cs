using Microsoft.OpenApi.Models;
using ProductServer.API.Middleware;

namespace ProductServer
{
    public static class ServicesConfiguration
    {
        public static void AddPresentationLayer(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Server", Version = "v1" });
                options.SupportNonNullableReferenceTypes();
            });

            services.AddTransient<ExceptionMiddleware>();
        }
    }
}

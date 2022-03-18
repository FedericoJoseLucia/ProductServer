using Microsoft.OpenApi.Models;

namespace ProductServer
{
    public static class ServicesConfiguration
    {
        public static void AddPresentationLayer(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Server", Version = "v1" });
            });
        }
    }
}

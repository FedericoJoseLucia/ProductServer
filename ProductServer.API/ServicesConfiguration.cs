namespace ProductServer
{
    public static class ServicesConfiguration
    {
        public static void AddPresentationLayer(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();
        }
    }
}

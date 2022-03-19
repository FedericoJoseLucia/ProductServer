namespace ProductServer.Infrastructure
{
    public class SqlServerOptions
    {
        public const string ConfigurationKey = "ConnectionStrings";
        public string SqlServerConnection { get; set; } = string.Empty;
    }

    public class ProductExternalServerAPIOptions
    {
        public const string ConfigurationKey = "ProductExternalServerAPI";
        public string BaseAddress { get; set; } = string.Empty;
        public Uri BaseAddressUri => new(BaseAddress);
        public string ProductExternalDataEndpoint { get; set; } = "ProductExternalData";
    }
}

namespace ProductServer.Infrastructure
{
    public class SqlServerOptions
    {
        public const string ConfigurationKey = "ConnectionStrings";
        public string SqlServerConnection { get; set; } = string.Empty;
    }
}

using Microsoft.Extensions.Options;
using ProductServer.Application.Services.ProductExternalServerService;
using System.Text.Json;

namespace ProductServer.Infrastructure.Services
{
    internal class ProductExternalServerService : IProductExternalServerService
    {
        private readonly HttpClient httpClient;
        private readonly ProductExternalServerAPIOptions externalAPIOptions;

        public ProductExternalServerService(HttpClient httpClient, IOptions<ProductExternalServerAPIOptions> externalAPIOptions)
        {
            this.httpClient = httpClient;
            this.externalAPIOptions = externalAPIOptions.Value;
        }

        public async Task<ProductExternalDataDto?> GetProductExternalDataByCode(int code, CancellationToken cancellationToken = default)
        {
            string requestUri = $"{externalAPIOptions.ProductExternalDataEndpoint}/{code}";

            using HttpResponseMessage response = await httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<ProductExternalDataDto>(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}

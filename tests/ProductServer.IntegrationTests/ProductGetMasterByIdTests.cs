using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ProductServer.API.Models;
using ProductServer.IntegrationTests.SeedWork;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ProductServer.IntegrationTests
{
    [TestCaseOrderer(TestNumericOrderer.TypeName, TestNumericOrderer.AssemblyName)]
    public class ProductGetMasterByIdTests : IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory factory;
        private readonly HttpClient client;

        private const string createUri = "/api/Product/Create";
        private const string getMasterByIdUri = "/api/Product/GetById/Master";

        private static readonly Guid productId_1 = new("21b53198-3a9c-4169-aba3-4cb07fba2e40");
        private const int productExtCode_1 = 1;
        private const string productDenom_1 = "product_1";
        private const decimal productPrice_1 = 123.4M;

        private static readonly Guid productId_2 = new("5356fdf4-dbfd-462c-8a11-460a4023da23");
        private const int productExtCode_2 = 2;
        private const string productDenom_2 = "product_2";
        private const decimal productPrice_2 = 456.7M;

        public ProductGetMasterByIdTests(WebApplicationFactory factory)
        {
            this.factory = factory;
            this.client = factory.CreateClient();
        }

        [Fact, TestOrder(1)]
        public async Task GivenNotCreatedProduct_GetMasterById_ShouldFail()
        {
            // Act
            var response = await client.GetAsync($"{getMasterByIdUri}/{productId_1}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact, TestOrder(2)]
        public async Task GivenMultipleCreatedProducts_GetMasterById_ShouldSucceed_WithCorrectLastProductData()
        {
            // Arrange
            await CreateProductAsync(productId_1, productExtCode_1, productDenom_1, productPrice_1).ConfigureAwait(false);
            await CreateProductAsync(productId_2, productExtCode_2, productDenom_2, productPrice_2).ConfigureAwait(false);

            // Act
            var response = await client.GetAsync($"{getMasterByIdUri}/{productId_1}");

            await using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            MasterProduct? result = await JsonSerializer.DeserializeAsync<MasterProduct>(stream).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().NotBeNull();
            result!.Id.Should().Be(productId_1);
            result!.ExternalCode.Should().Be(productExtCode_1);
            result!.Denomination.Should().Be(productDenom_1);
            result!.Price.Should().Be(productPrice_1);
            result!.State.Should().Be(ProductState.Enabled);

            // External service properties
            result!.Stock.Should().NotBeNull();
            result!.Department.Should().NotBeNullOrEmpty();

            // Cached properties
            result!.LastCreatedProductId.Should().NotBeNull().And.Be(productId_2);
            result!.LastCreatedProductExternalCode.Should().NotBeNull().And.Be(productExtCode_2);
        }

        [Fact, TestOrder(3)]
        public async Task GivenCacheInvalidated_GetMasterById_ShouldSucceed_WithCorrectLastProductData()
        {
            // Arrange
            var cache = factory.Services.GetService<IMemoryCache>();
            cache!.Remove("last_product");

            // Act
            var response = await client.GetAsync($"{getMasterByIdUri}/{productId_2}");

            await using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            MasterProduct? result = await JsonSerializer.DeserializeAsync<MasterProduct>(stream).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().NotBeNull();
            result!.Id.Should().Be(productId_2);
            result!.ExternalCode.Should().Be(productExtCode_2);
            result!.Denomination.Should().Be(productDenom_2);
            result!.Price.Should().Be(productPrice_2);
            result!.State.Should().Be(ProductState.Enabled);

            // External service properties
            result!.Stock.Should().NotBeNull();
            result!.Department.Should().NotBeNullOrEmpty();

            // Cached properties
            result!.LastCreatedProductId.Should().NotBeNull().And.Be(productId_2);
            result!.LastCreatedProductExternalCode.Should().NotBeNull().And.Be(productExtCode_2);
        }

        private async Task CreateProductAsync(Guid id, int externalCode, string denomination, decimal price)
        {
            CreateProductRequest request = new()
            {
                Id = id,
                ExternalCode = externalCode,
                Denomination = denomination,
                Price = price
            };
            HttpContent content = JsonContent.Create(request);
            await client.PostAsync(createUri, content);
        }
    }
}

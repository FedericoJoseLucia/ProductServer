using FluentAssertions;
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
    public class ProductUpdateTests : IClassFixture<WebApplicationFactory>
    {
        private readonly HttpClient client;

        private const string createUri = "/api/Product/Create";
        private const string updateUri = "/api/Product/Update";
        private const string getByIdUri = "/api/Product/GetById";

        private static readonly Guid productId_1 = new("21b53198-3a9c-4169-aba3-4cb07fba2e40");
        private const int productExtCode_1 = 1;
        private const string productDenom_1 = "product_created";
        private const decimal productPrice_1 = 123.4M;

        private const string productDenom_2 = "product_updated";
        private const decimal productPrice_2 = 456.7M;
        private static readonly ProductState productState_2 = ProductState.Suspended;

        public ProductUpdateTests(WebApplicationFactory factory)
        {
            this.client = factory.CreateClient();
        }

        [Fact, TestOrder(0)]
        public async Task Setup()
        {
            // Arrange
            CreateProductRequest request = new()
            {
                Id = productId_1,
                ExternalCode = productExtCode_1,
                Denomination = productDenom_1,
                Price = productPrice_1
            };
            HttpContent content = JsonContent.Create(request);
            await client.PostAsync(createUri, content);
        }

        [Fact, TestOrder(1)]
        public async Task GivenInvalidId_Update_ShouldFail()
        {
            // Arrange
            UpdateProductRequest request = new()
            {
                Id = Guid.NewGuid(),
                Denomination = productDenom_2,
                Price = productPrice_2,
                State = productState_2
            };
            HttpContent content = JsonContent.Create(request);

            // Act
            var response = await client.PutAsync(updateUri, content);

            await using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            RequestResult? result = await JsonSerializer.DeserializeAsync<RequestResult>(stream).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result!.IsSuccess.Should().BeFalse();
            result!.Errors.Should().NotBeNullOrEmpty();
        }

        [Fact, TestOrder(2)]
        public async Task GivenInvalidParameters_Update_ShouldFail()
        {
            // Arrange
            UpdateProductRequest request = new()
            {
                Id = productId_1,
                Denomination = "",
                Price = -1,
                State = productState_2
            };
            HttpContent content = JsonContent.Create(request);

            // Act
            var response = await client.PutAsync(updateUri, content);

            await using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            RequestResult? result = await JsonSerializer.DeserializeAsync<RequestResult>(stream).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result!.IsSuccess.Should().BeFalse();
            result!.Errors.Should().NotBeNullOrEmpty();
        }

        [Fact, TestOrder(3)]
        public async Task GivenValidParameters_Update_ShouldSucceed()
        {
            // Arrange
            UpdateProductRequest request = new()
            {
                Id = productId_1,
                Denomination = productDenom_2,
                Price = productPrice_2,
                State = productState_2
            };
            HttpContent content = JsonContent.Create(request);

            // Act
            var response = await client.PutAsync(updateUri, content);

            await using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            RequestResult? result = await JsonSerializer.DeserializeAsync<RequestResult>(stream).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().NotBeNull();
            result!.IsSuccess.Should().BeTrue();
            result!.Errors.Should().NotBeNull();
            result!.Errors.Should().BeEmpty();
        }

        [Fact, TestOrder(4)]
        public async Task GivenProductUpdated_GetById_ShouldSucceed()
        {
            // Act
            var response = await client.GetAsync($"{getByIdUri}/{productId_1}");

            await using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            Product? result = await JsonSerializer.DeserializeAsync<Product>(stream).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().NotBeNull();
            result!.Id.Should().Be(productId_1);
            result!.ExternalCode.Should().Be(productExtCode_1);
            result!.Denomination.Should().Be(productDenom_2);
            result!.Price.Should().Be(productPrice_2);
            result!.State.Should().Be(productState_2);
        }
    }
}

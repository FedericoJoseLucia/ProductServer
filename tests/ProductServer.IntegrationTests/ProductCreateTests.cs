using FluentAssertions;
using ProductServer.API.Models;
using ProductServer.IntegrationTests.SeedWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ProductServer.IntegrationTests
{
    [TestCaseOrderer(TestNumericOrderer.TypeName, TestNumericOrderer.AssemblyName)]
    public class ProductCreateTests : IClassFixture<WebApplicationFactory>
    {
        private readonly HttpClient client;

        private const string createUri = "/api/Product/Create";
        private const string getByIdUri = "/api/Product/GetById";

        private static readonly Guid productId_1 = new("21b53198-3a9c-4169-aba3-4cb07fba2e40");
        private const int productExtCode_1 = 1;
        private const string productDenom_1 = "product_created";
        private const decimal productPrice_1 = 123.4M;

        public ProductCreateTests(WebApplicationFactory factory)
        {
            this.client = factory.CreateClient();
        }

        [Fact, TestOrder(1)]
        public async Task GivenInvalidParameters_Create_ShouldFail()
        {
            // Arrange
            CreateProductRequest request = new()
            {
                Id = productId_1,
                ExternalCode = 500,
                Denomination = "",
                Price = -1
            };
            HttpContent content = JsonContent.Create(request);

            // Act
            var response = await client.PostAsync(createUri, content);

            await using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            RequestResult? result = await JsonSerializer.DeserializeAsync<RequestResult>(stream).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result!.IsSuccess.Should().BeFalse();
            result!.Errors.Should().NotBeNullOrEmpty();
        }

        [Fact, TestOrder(2)]
        public async Task GivenValidParameters_Create_ShouldSucceed()
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

            // Act
            var response = await client.PostAsync(createUri, content);

            await using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            RequestResult? result = await JsonSerializer.DeserializeAsync<RequestResult>(stream).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().NotBeNull();
            result!.IsSuccess.Should().BeTrue();
            result!.Errors.Should().NotBeNull();
            result!.Errors.Should().BeEmpty();
        }

        [Fact, TestOrder(3)]
        public async Task GivenProductCreated_GetById_ShouldSucceed()
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
            result!.Denomination.Should().Be(productDenom_1);
            result!.Price.Should().Be(productPrice_1);
            result!.State.Should().Be(ProductState.Enabled);
        }
    }
}

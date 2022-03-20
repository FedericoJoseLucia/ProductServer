using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductServer.API.Controllers;
using ProductServer.API.Models;
using ProductServer.Application.Queries.GetMasterProductById;
using ProductServer.Application.SeedWork;
using ProductServer.Application.Services.ProductService;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProductServer.API.UnitTests
{
    public class ProductControllerTests
    {
        private static readonly Guid id = new("D84B82A5-7037-467C-A939-D39D5AE5CAE8");
        private const int externalCode = 5;
        private const string denomination = "test_product";
        private static readonly decimal price = 123;
        private const int stateId = 1;

        [Fact]
        public async Task Create_ShouldSucceed()
        {
            // Arrange
            ISender mediator = GetMockedMediatrInstanceWithCustomResult(CommandResult.Success());
            ProductController controller = new(mediator);
            CreateProductRequest request = new() { Id = id, Denomination = denomination, Price = price };

            // Act
            var result = await controller.Create(request, CancellationToken.None);

            // Assert
            var actionResult = Assert.IsType<ActionResult<RequestResult>>(result);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var requestResult = Assert.IsType<RequestResult>(objectResult.Value);

            requestResult.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Create_ShouldFail()
        {
            // Arrange
            string error = "error";
            ISender mediator = GetMockedMediatrInstanceWithCustomResult(CommandResult.Error(error));
            ProductController controller = new(mediator);
            CreateProductRequest request = new() { Id = id, Denomination = denomination, Price = price };

            // Act
            var result = await controller.Create(request, CancellationToken.None);

            // Assert
            var actionResult = Assert.IsType<ActionResult<RequestResult>>(result);
            var objectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var requestResult = Assert.IsType<RequestResult>(objectResult.Value);
            
            requestResult.IsSuccess.Should().BeFalse();
            requestResult.Errors.Should().OnlyContain(x => x == error);
        }

        [Fact]
        public async Task Update_ShouldSucceed()
        {
            // Arrange
            ISender mediator = GetMockedMediatrInstanceWithCustomResult(CommandResult.Success());
            ProductController controller = new(mediator);
            UpdateProductRequest request = new() { Id = id, Denomination = denomination, Price = price };

            // Act
            var result = await controller.Update(request, CancellationToken.None);

            // Assert
            var actionResult = Assert.IsType<ActionResult<RequestResult>>(result);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var requestResult = Assert.IsType<RequestResult>(objectResult.Value);

            requestResult.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Update_ShouldFail()
        {
            // Arrange
            string error = "error";
            ISender mediator = GetMockedMediatrInstanceWithCustomResult(CommandResult.Error(error));
            ProductController controller = new(mediator);
            UpdateProductRequest request = new() { Id = id, Denomination = denomination, Price = price };

            // Act
            var result = await controller.Update(request, CancellationToken.None);

            // Assert
            var actionResult = Assert.IsType<ActionResult<RequestResult>>(result);
            var objectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var requestResult = Assert.IsType<RequestResult>(objectResult.Value);
            
            requestResult.IsSuccess.Should().BeFalse();
            requestResult.Errors.Should().OnlyContain(x => x == error);
        }

        [Fact]
        public async Task GetById_ShouldSucceed()
        {
            // Arrange
            ProductDto dto = new(id, externalCode, denomination, price, (ProductDto.ProductState)stateId);
            ISender mediator = GetMockedMediatrInstanceWithCustomResult(dto);
            ProductController controller = new(mediator);

            // Act
            var result = await controller.GetById(id, CancellationToken.None);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var productFound = Assert.IsType<Product>(objectResult.Value);

            productFound.Should().NotBeNull();
            productFound.Id.Should().Be(id);
            productFound.Denomination.Should().Be(denomination);
            productFound.Price.Should().Be(price);
            productFound.State.Should().Be((ProductState)stateId);
        }

        [Fact]
        public async Task GetById_ShouldFail()
        {
            // Arrange
            ProductDto? dto = null;
            ISender mediator = GetMockedMediatrInstanceWithCustomResult(dto);
            ProductController controller = new(mediator);

            // Act
            var result = await controller.GetById(Guid.NewGuid(), CancellationToken.None);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetByIdMaster_ShouldSucceed()
        {
            // Arrange
            MasterProductDto dto = new(id, externalCode, denomination, price, (MasterProductDto.ProductState)stateId, null, null, null, null);
            ISender mediator = GetMockedMediatrInstanceWithCustomResult(dto);
            ProductController controller = new(mediator);

            // Act
            var result = await controller.GetMasterById(id, CancellationToken.None);

            // Assert
            var actionResult = Assert.IsType<ActionResult<MasterProduct>>(result);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var productFound = Assert.IsType<MasterProduct>(objectResult.Value);

            productFound.Should().NotBeNull();
            productFound.Id.Should().Be(id);
            productFound.Denomination.Should().Be(denomination);
            productFound.Price.Should().Be(price);
            productFound.State.Should().Be((ProductState)stateId);
        }

        [Fact]
        public async Task GetByIdMaster_ShouldFail()
        {
            // Arrange
            MasterProductDto? dto = null;
            ISender mediator = GetMockedMediatrInstanceWithCustomResult(dto);
            ProductController controller = new(mediator);

            // Act
            var result = await controller.GetMasterById(Guid.NewGuid(), CancellationToken.None);

            // Assert
            var actionResult = Assert.IsType<ActionResult<MasterProduct>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        private static ISender GetMockedMediatrInstanceWithCustomResult<TResult>(TResult result) where TResult : class?
        {
            Mock<ISender> mediator = new();
            mediator.Setup(x => x.Send(It.IsAny<IRequest<TResult>>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);
            return mediator.Object;
        }
    }
}

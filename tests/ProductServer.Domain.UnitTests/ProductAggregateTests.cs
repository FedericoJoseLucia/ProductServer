using FluentAssertions;
using ProductServer.Domain.Aggregates.Product;
using System;
using Xunit;

namespace ProductServer.Domain.UnitTests
{
    public class ProductAggregateTests
    {
        private static readonly Guid id = new("D84B82A5-7037-467C-A939-D39D5AE5CAE8");
        private const string denomination = "test_product";
        private static readonly decimal price = 123;

        [Fact]
        public void Product_ShouldBuild()
        {
            // Act
            Product product = new(id, denomination, price);

            // Assert
            product.Should().NotBeNull();
            product.Id.Should().Be(id);
            product.Denomination.Should().Be(denomination);
            product.Price.Should().Be(price);
            product.State.Should().Be(ProductState.Enabled);
        }

        [Theory]
        [InlineData("updated_test_product")]
        public void Product_ShouldUpdateDenomination_WithValidDenomination(string newDenomination)
        {
            // Arrange
            Product product = new(id, denomination, price);

            // Act
            product.UpdateDenomination(newDenomination);

            // Assert
            product.Denomination.Should().Be(newDenomination);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(300.5)]
        public void Product_ShouldUpdatePrice_WithValidPrice(decimal newPrice)
        {
            // Arrange
            Product product = new(id, denomination, price);

            // Act
            product.UpdatePrice(newPrice);

            // Assert
            product.Price.Should().Be(newPrice);
        }

        [Fact]
        public void Product_ShouldUpdateState_WithValidState()
        {
            // Arrange
            Product product = new(id, denomination, price);
            ProductState newState = ProductState.Disabled;

            // Act
            product.UpdateState(newState);

            // Assert
            product.State.Should().Be(newState);
        }

        [Theory]
        [InlineData("")]
        public void Product_UpdateDenomination_ShouldThrow_WithInvalidDenomination(string newDenomination)
        {
            // Arrange
            Product product = new(id, denomination, price);
            var action = () => product.UpdateDenomination(newDenomination);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(-1)]
        public void Product_UpdatePrice_ShouldThrow_WithInvalidPrice(decimal newPrice)
        {
            // Arrange
            Product product = new(id, denomination, price);
            var action = () => product.UpdatePrice(newPrice);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ProductState_ShouldGetById_WithValidId(int productStateId)
        {
            // Act
            ProductState productState = ProductState.GetById(productStateId);

            // Assert
            productState.Id.Should().Be(productStateId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(4)]
        public void ProductState_GetById_ShouldThrow_WithInvalidId(int productStateId)
        {
            // Arrange
            var action = () => ProductState.GetById(productStateId);

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}

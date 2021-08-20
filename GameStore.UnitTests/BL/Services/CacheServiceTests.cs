using AutoFixture;
using FakeItEasy;
using GameStore.BL.Enums;
using GameStore.BL.Services;
using GameStore.DAL.Entities;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace GameStore.UnitTests.BL.Services
{
    public class CacheServiceTests
    {
        [Fact]
        public void ShouldReturnNotFoundOnGetEntity()
        {
            //Arrange
            var memory = A.Fake<IMemoryCache>();

            var service = new CacheService<Product>(memory);
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };
            var product = fixture.Create<Product>();

            //Act
            var result = service.GetEntity(product.Id);

            //Assert
            Assert.Equal(ServiceResultType.NotFound, result.Result);
        }

        [Fact]
        public void ShouldReturnSuccessOnRemoveEntity()
        {
            //Arrange
            var memory = A.Fake<IMemoryCache>();

            var service = new CacheService<Product>(memory);
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };
            var product = fixture.Create<Product>();

            //Act
            var result = service.RemoveEntity(product.Id);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);
        }
    }
}
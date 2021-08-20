using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AutoFixture;
using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using GameStore.BL.Services;
using GameStore.DAL.Entities;
using GameStore.BL.ResultWrappers;
using GameStore.BL.Enums;

namespace GameStore.UnitTests.BL.Utilities
{
    public class CacheServiceTests
    {
        [Fact]
        public void ShouldReturnNotFoundOnGetEntity()
        {
            //Arrange
            var memory = A.Fake<IMemoryCache>();

            var service = new CacheService<Product>(memory);
            var fixture = new Fixture()
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
            var fixture = new Fixture()
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

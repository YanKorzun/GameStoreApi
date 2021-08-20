using AutoFixture;
using GameStore.BL.Utilities;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Parameters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameStore.UnitTests.BL.Utilities
{
    public class ProductPropUtilityTests
    {
        [Fact]
        public void ShouldReturnDefaultExpression()
        {
            //Arrange
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var propModel = fixture.Create<ProductParametersDto>();

            //Act
            var result = ProductPropUtility.GetOrderExpression(propModel);
            Expression<Func<Product, object>> expectedExpression = o => o.Name;

            //Assert
            Assert.Equal(expectedExpression.ToString(), result.ToString());

        }

        [Fact]
        public void ShouldReturnOrderByNameExpression()
        {
            //Arrange
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var propModel = fixture.Build<ProductParametersDto>().With(o => o.OrderBy, "Name").Create();

            //Act
            var result = ProductPropUtility.GetOrderExpression(propModel);
            Expression<Func<Product, object>> expectedExpression = o => o.Name;

            //Assert
            Assert.Equal(expectedExpression.ToString(), result.ToString());

        }

        [Fact]
        public void ShouldReturnOrderByGenreExpression()
        {
            //Arrange
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var propModel = fixture.Build<ProductParametersDto>().With(o => o.OrderBy, "Genre").Create();

            //Act
            var result = ProductPropUtility.GetOrderExpression(propModel);
            Expression<Func<Product, object>> expectedExpression = o => o.Genre;

            //Assert
            Assert.Equal(expectedExpression.ToString(), result.ToString());

        }

        [Fact]
        public void ShouldReturnOrderByTotalRatingExpression()
        {
            //Arrange
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var propModel = fixture.Build<ProductParametersDto>().With(o => o.OrderBy, "TotalRating").Create();

            //Act
            var result = ProductPropUtility.GetOrderExpression(propModel);
            Expression<Func<Product, object>> expectedExpression = o => o.TotalRating;

            //Assert
            Assert.Equal(expectedExpression.ToString(), result.ToString());

        }

        [Fact]
        public void ShouldReturnOrderByPriceExpression()
        {
            //Arrange
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var propModel = fixture.Build<ProductParametersDto>().With(o => o.OrderBy, "Price").Create();

            //Act
            var result = ProductPropUtility.GetOrderExpression(propModel);
            Expression<Func<Product, object>> expectedExpression = o => o.Price;

            //Assert
            Assert.Equal(expectedExpression.ToString(), result.ToString());

        }

        [Fact]
        public void ShouldReturnOrderByCountExpression()
        {
            //Arrange
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var propModel = fixture.Build<ProductParametersDto>().With(o => o.OrderBy, "Count").Create();

            //Act
            var result = ProductPropUtility.GetOrderExpression(propModel);
            Expression<Func<Product, object>> expectedExpression = o => o.Count;

            //Assert
            Assert.Equal(expectedExpression.ToString(), result.ToString());

        }
        

        [Fact]
        public void ShouldReturnFilterbyAgeExpression()
        {
            //Arrange
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var propModel = fixture.Build<ProductParametersDto>().Without(o => o.Genre).Create();

            //Act
            var result = ProductPropUtility.GetFilterExpression(propModel);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ShouldReturnFilterbyGenreExpression()
        {
            //Arrange
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var propModel = fixture.Build<ProductParametersDto>().Without(o => o.AgeRating).Create();

            //Act
            var result = ProductPropUtility.GetFilterExpression(propModel);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ShouldReturnFilterbyGenreAndAgeExpression()
        {
            //Arrange
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var propModel = fixture.Build<ProductParametersDto>().Create();

            //Act
            var result = ProductPropUtility.GetFilterExpression(propModel);

            //Assert
            Assert.NotNull(result);
        }

    }
}

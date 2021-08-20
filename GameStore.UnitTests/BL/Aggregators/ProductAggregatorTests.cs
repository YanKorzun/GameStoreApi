using System.Collections.Generic;
using AutoFixture;
using AutoMapper;
using GameStore.BL.Aggregators;
using GameStore.BL.Mappers;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Products;
using Xunit;

namespace GameStore.UnitTests.BL.Aggregators
{
    public class ProductAggregatorTests
    {
        [Fact]
        public void ShouldReturnProduct()
        {
            //Arrange

            var mapper =
                new Mapper(new MapperConfiguration(cfg =>
                    cfg.AddProfiles(new List<Profile> { new ProductModelProfile() })));
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var aggregator = new ProductAggregator();

            var productDto = fixture.Build<InputProductDto>().Without(o => o.Logo).Without(o => o.Background).Create();
            var logoString = fixture.Create<string>();
            var bgString = fixture.Create<string>();

            var expectedProduct = mapper.Map<Product>(productDto);

            expectedProduct.Logo = logoString;
            expectedProduct.Background = bgString;

            //Act
            var result = aggregator.AggregateProduct(productDto, bgString, logoString);

            //Assert
            AssertProduct(expectedProduct, result);
        }

        [Fact]
        public void ShouldReturnProductWhenSendsExtendedModel()
        {
            //Arrange
            var mapper =
                new Mapper(new MapperConfiguration(cfg =>
                    cfg.AddProfiles(new List<Profile> { new ProductModelProfile() })));
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var aggregator = new ProductAggregator();

            var productDto = fixture.Build<ExtendedInputProductDto>().Without(o => o.Logo).Without(o => o.Background)
                .Create();
            var logoString = fixture.Create<string>();
            var bgString = fixture.Create<string>();

            var expectedProduct = mapper.Map<Product>(productDto);

            expectedProduct.Logo = logoString;
            expectedProduct.Background = bgString;

            //Act
            var result = aggregator.AggregateProduct(productDto, bgString, logoString);

            //Assert
            Assert.Equal(expectedProduct.Id, result.Id);
            AssertProduct(expectedProduct, result);
        }

        private static void AssertProduct(Product expectedProduct, Product result)
        {
            Assert.Equal(expectedProduct.AgeRating, result.AgeRating);
            Assert.Equal(expectedProduct.Background, result.Background);
            Assert.Equal(expectedProduct.Count, result.Count);
            Assert.Equal(expectedProduct.DateCreated, result.DateCreated);
            Assert.Equal(expectedProduct.Developers, result.Developers);
            Assert.Equal(expectedProduct.Logo, result.Logo);
            Assert.Equal(expectedProduct.Genre, result.Genre);
            Assert.Equal(expectedProduct.Name, result.Name);
            Assert.Equal(expectedProduct.Platform, result.Platform);
            Assert.Equal(expectedProduct.Price, result.Price);
            Assert.Equal(expectedProduct.PublicationDate, result.PublicationDate);
            Assert.Equal(expectedProduct.Publishers, result.Publishers);
            Assert.Equal(expectedProduct.TotalRating, result.TotalRating);
        }
    }
}
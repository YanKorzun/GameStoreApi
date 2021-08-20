using System.Collections.Generic;
using AutoFixture;
using AutoMapper;
using GameStore.BL.Aggregators;
using GameStore.BL.Mappers;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Products;
using Newtonsoft.Json;
using Xunit;

namespace GameStore.UnitTests.BL.Aggregators
{
    public class ProductAggregatorTests
    {
        [Fact]
        public void ShouldReturnProduct()
        {
            //Arrange

            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile> { new ProductModelProfile() })));
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var aggregator = new ProductAggregator();

            var productDto = fixture.Build<InputProductDto>().Without(o => o.Logo).Without(o => o.Background).Create();
            var logoString = fixture.Create<string>();
            var bgString = fixture.Create<string>();

            var expectedResult = mapper.Map<Product>(productDto);

            expectedResult.Logo = logoString;
            expectedResult.Background = bgString;

            //Act
            var result = aggregator.AggregateProduct(productDto, bgString, logoString);

            //Assert
            Assert.Equal(expectedResult.AgeRating, result.AgeRating);
            Assert.Equal(expectedResult.Background, result.Background);
            Assert.Equal(expectedResult.Count, result.Count);
            Assert.Equal(expectedResult.DateCreated, result.DateCreated);
            Assert.Equal(expectedResult.Developers, result.Developers);
            Assert.Equal(expectedResult.Logo, result.Logo);
            Assert.Equal(expectedResult.Genre, result.Genre);
            Assert.Equal(expectedResult.Name, result.Name);
            Assert.Equal(expectedResult.Platform, result.Platform);
            Assert.Equal(expectedResult.Price, result.Price);
            Assert.Equal(expectedResult.PublicationDate, result.PublicationDate);
            Assert.Equal(expectedResult.Publishers, result.Publishers);
            Assert.Equal(expectedResult.TotalRating, result.TotalRating);
        }

        [Fact]
        public void ShouldReturnProductWhenSendsExtendedModel()
        {
            //Arrange
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile> { new ProductModelProfile() })));
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var aggregator = new ProductAggregator();

            var productDto = fixture.Build<ExtendedInputProductDto>().Without(o => o.Logo).Without(o => o.Background)
                .Create();
            var logoString = fixture.Create<string>();
            var bgString = fixture.Create<string>();

            var expectedResult = mapper.Map<Product>(productDto);

            expectedResult.Logo = logoString;
            expectedResult.Background = bgString;

            //Act
            var result = aggregator.AggregateProduct(productDto, bgString, logoString);

            //Assert
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.AgeRating, result.AgeRating);
            Assert.Equal(expectedResult.Background, result.Background);
            Assert.Equal(expectedResult.Count, result.Count);
            Assert.Equal(expectedResult.DateCreated, result.DateCreated);
            Assert.Equal(expectedResult.Developers, result.Developers);
            Assert.Equal(expectedResult.Logo, result.Logo);
            Assert.Equal(expectedResult.Genre, result.Genre);
            Assert.Equal(expectedResult.Name, result.Name);
            Assert.Equal(expectedResult.Platform, result.Platform);
            Assert.Equal(expectedResult.Price, result.Price);
            Assert.Equal(expectedResult.PublicationDate, result.PublicationDate);
            Assert.Equal(expectedResult.Publishers, result.Publishers);
            Assert.Equal(expectedResult.TotalRating, result.TotalRating);
        }
    }
}
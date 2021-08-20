using AutoFixture;
using GameStore.BL.Aggregators;
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
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var aggregator = new ProductAggregator();

            var productDto = fixture.Build<InputProductDto>().Without(o => o.Logo).Without(o => o.Background).Create();
            var logostring = fixture.Create<string>();
            var bgstring = fixture.Create<string>();

            var expectedResult = JsonConvert.DeserializeObject<Product>(JsonConvert.SerializeObject(productDto));

            expectedResult.Logo = logostring;
            expectedResult.Background = bgstring;

            //Act
            var result = aggregator.AggregateProduct(productDto, bgstring, logostring);

            //Assert
            var serializedResult = JsonConvert.SerializeObject(result);
            var serializedExpectedResult = JsonConvert.SerializeObject(expectedResult);

            Assert.Equal(serializedExpectedResult, serializedResult);
        }

        [Fact]
        public void ShouldReturnProductWhenSendsExtendedModel()
        {
            //Arrange
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var aggregator = new ProductAggregator();

            var productDto = fixture.Build<ExtendedInputProductDto>().Without(o => o.Logo).Without(o => o.Background).Create();
            var logostring = fixture.Create<string>();
            var bgstring = fixture.Create<string>();

            var expectedResult = JsonConvert.DeserializeObject<Product>(JsonConvert.SerializeObject(productDto));

            expectedResult.Logo = logostring;
            expectedResult.Background = bgstring;

            //Act
            var result = aggregator.AggregateProduct(productDto, bgstring, logostring);

            //Assert
            var serializedResult = JsonConvert.SerializeObject(result);
            var serializedExpectedResult = JsonConvert.SerializeObject(expectedResult);

            Assert.Equal(serializedExpectedResult, serializedResult);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoFixture;
using FakeItEasy;
using GameStore.BL.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO.Ratings;
using Xunit;

namespace GameStore.UnitTests.BL.Services
{
    public class ProductRatingServiceTests
    {
        [Fact]
        public async Task ShouldCreateProductRatingAsync()
        {
            //Arrange
            var ratingRepository = A.Fake<IProductRatingRepository>();
            var productRepository = A.Fake<IProductRepository>();

            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var userId = fixture.Create<int>();
            var ratingDto = fixture.Create<RatingDto>();

            var fakeRatings = new List<ProductRating> { fixture.Create<ProductRating>() };
            var productRatingService = new ProductRatingService(ratingRepository, productRepository);

            A.CallTo(() => ratingRepository.GetRatingsAsync(A<Expression<Func<ProductRating, bool>>>._))
                .Returns(fakeRatings);

            //Act
            var result = await productRatingService.CreateProductRatingAsync(userId, ratingDto);

            //Assert
            Assert.NotNull(result);

            A.CallTo(() => ratingRepository.CreateRatingAsync(A<ProductRating>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => ratingRepository.GetRatingsAsync(A<Expression<Func<ProductRating, bool>>>._))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => productRepository.FindProductByIdAsync(A<int>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() =>
                productRepository.UpdateItemWithModifiedPropsAsync(A<Product>._,
                    A<Expression<Func<Product, object>>>._)).MustHaveHappenedOnceExactly();
        }
    }
}
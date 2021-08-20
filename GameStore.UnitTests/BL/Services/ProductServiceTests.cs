using AutoFixture;
using AutoMapper;
using CloudinaryDotNet.Actions;
using FakeItEasy;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.BL.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO.Parameters;
using GameStore.WEB.DTO.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace GameStore.UnitTests.BL.Services
{
    public class ProductServiceTests
    {

        [Fact]
        public async Task ShouldReturnSuccessFromCreateProductAsync()
        {
            //Arrange
            var mapper = A.Fake<IMapper>();
            var productRepository = A.Fake<IProductRepository>();
            var customProductAggregator = A.Fake<ICustomProductAggregator>();
            var cloudinaryService = A.Fake<ICloudinaryService>();

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var productService = new ProductService(mapper, productRepository, customProductAggregator, cloudinaryService);

            var inputDto = fixture.Build<InputProductDto>().Without(o => o.Logo).Without(o => o.Background).Create();

            var logo = fixture.Create<string>();
            var bground = fixture.Create<string>();


            var uploadResult = new ServiceResult<ImageUploadResult>(ServiceResultType.Success, new ImageUploadResult() { Url = fixture.Create<System.Uri>() });

            A.CallTo(() => cloudinaryService.UploadAsync(A<IFormFile>._)).Returns(Task.FromResult(uploadResult));


            //Act
            var result = await productService.CreateProductAsync(inputDto);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => productRepository.CreateItemAsync(A<Product>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => customProductAggregator.AggregateProduct(A<InputProductDto>._, bground, logo));
            A.CallTo(() => cloudinaryService.UploadAsync(A<IFormFile>._)).MustHaveHappenedTwiceExactly();
        }

        [Fact]
        public async Task ShouldReturnSuccessFromUpdateProductAsync()
        {
            //Arrange
            var mapper = A.Fake<IMapper>();
            var productRepository = A.Fake<IProductRepository>();
            var customProductAggregator = A.Fake<ICustomProductAggregator>();
            var cloudinaryService = A.Fake<ICloudinaryService>();

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var productService = new ProductService(mapper, productRepository, customProductAggregator, cloudinaryService);

            var inputDto = fixture.Build<ExtendedInputProductDto>().Without(o => o.Logo).Without(o => o.Background).Create();

            var logo = fixture.Create<string>();
            var bground = fixture.Create<string>();

            var uploadResult = new ServiceResult<ImageUploadResult>(ServiceResultType.Success, new ImageUploadResult() { Url = fixture.Create<System.Uri>() });

            A.CallTo(() => cloudinaryService.UploadAsync(A<IFormFile>._)).Returns(Task.FromResult(uploadResult));
            
            //Act
            var result = await productService.UpdateProductAsync(inputDto);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => productRepository.UpdateProductAsync(A<Product>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => cloudinaryService.UploadAsync(A<IFormFile>._)).MustHaveHappenedTwiceExactly();
        }

        [Fact]
        public async Task ShouldReturnBadUploadResultFromUpdateProductAsync()
        {
            //Arrange
            const string errorMessage = "problems with uploading your pictures";
            var mapper = A.Fake<IMapper>();
            var productRepository = A.Fake<IProductRepository>();
            var customProductAggregator = A.Fake<ICustomProductAggregator>();
            var cloudinaryService = A.Fake<ICloudinaryService>();

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var productService = new ProductService(mapper, productRepository
                , customProductAggregator, cloudinaryService);

            var inputDto = fixture.Build<ExtendedInputProductDto>().Without(o => o.Logo).Without(o => o.Background).Create();

            var logo = fixture.Create<string>();
            var bground = fixture.Create<string>();


            var uploadResult = new ServiceResult<ImageUploadResult>(ServiceResultType.InvalidData, errorMessage, new ImageUploadResult() { });

            A.CallTo(() => cloudinaryService.UploadAsync(A<IFormFile>._)).Returns(Task.FromResult(uploadResult));


            //Act
            var result = await productService.UpdateProductAsync(inputDto);

            //Assert
            Assert.Equal(errorMessage, result.ErrorMessage);

            A.CallTo(() => productRepository.UpdateProductAsync(A<Product>._)).MustNotHaveHappened();
            A.CallTo(() => cloudinaryService.UploadAsync(A<IFormFile>._)).MustHaveHappenedTwiceExactly();
        }

        [Fact]
        public async Task ShouldReturnSuccesResultFromDeleteProductAsync()
        {
            //Arrange
            var mapper = A.Fake<IMapper>();
            var productRepository = A.Fake<IProductRepository>();
            var customProductAggregator = A.Fake<ICustomProductAggregator>();
            var cloudinaryService = A.Fake<ICloudinaryService>();

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var productId = fixture.Create<int>();

            var productService = new ProductService(mapper, productRepository, customProductAggregator, cloudinaryService);

            var deleteResult = new ServiceResult(ServiceResultType.Success);

            A.CallTo(() => productRepository.DeleteProductAsync(A<int>._)).Returns(deleteResult);


            //Act
            var result = await productService.DeleteProductAsync(productId);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => productRepository.DeleteProductAsync(A<int>._)).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async Task ShouldBeCalledOnceFromFindProductByIdAsync()
        {
            //Arrange
            var mapper = A.Fake<IMapper>();
            var productRepository = A.Fake<IProductRepository>();
            var customProductAggregator = A.Fake<ICustomProductAggregator>();
            var cloudinaryService = A.Fake<ICloudinaryService>();

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var productId = fixture.Create<int>();

            var productService = new ProductService(mapper, productRepository, customProductAggregator, cloudinaryService);

            var deleteResult = new ServiceResult(ServiceResultType.Success);

            A.CallTo(() => productRepository.FindProductByIdAsync(A<int>._));


            //Act
            var result = await productService.FindProductByIdAsync(productId);

            //Assert
            Assert.NotNull(result);

            A.CallTo(() => productRepository.FindProductByIdAsync(A<int>._)).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async Task ShouldBeCalledOnceFromGetProductsBySearchTermAsync()
        {
            //Arrange
            var mapper = A.Fake<IMapper>();
            var productRepository = A.Fake<IProductRepository>();
            var customProductAggregator = A.Fake<ICustomProductAggregator>();
            var cloudinaryService = A.Fake<ICloudinaryService>();

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var productId = fixture.Create<int>();

            var productService = new ProductService(mapper, productRepository, customProductAggregator, cloudinaryService);

            var deleteResult = new ServiceResult(ServiceResultType.Success);
            var searchTerm = "Name";
            var limit = 4;
            var offset = 5;

            A.CallTo(() => productRepository.GetProductsBySearchTermAsync(A<string>._, A<int>._, A<int>._));

           

            //Act
            var result = await productService.GetProductsBySearchTermAsync(searchTerm, limit, offset);

            //Assert
            Assert.NotNull(result);

            A.CallTo(() => productRepository.GetProductsBySearchTermAsync(A<string>._, A<int>._, A<int>._)).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async Task ShouldBeCalledOnceFromGetPagedProductListAsync()
        {
            //Arrange
            var mapper = A.Fake<IMapper>();
            var productRepository = A.Fake<IProductRepository>();
            var customProductAggregator = A.Fake<ICustomProductAggregator>();
            var cloudinaryService = A.Fake<ICloudinaryService>();

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var parameters = fixture.Create<ProductParametersDto>();

            var productId = fixture.Create<int>();

            var productService = new ProductService(mapper, productRepository, customProductAggregator, cloudinaryService);

            var deleteResult = new ServiceResult(ServiceResultType.Success);

            A.CallTo(() => productRepository.SearchForMultipleItemsAsync(A<Expression<Func<Product, bool>>>._, A<int>._, A<int>._, A<Expression<Func<Product, object>>>._, A<OrderType>._));


            //Act
            var result = await productService.GetPagedProductListAsync(parameters);

            //Assert
            Assert.NotNull(result);

            A.CallTo(() => productRepository.SearchForMultipleItemsAsync(A<Expression<Func<Product, bool>>>._, A<int>._, A<int>._, A<Expression<Func<Product, object>>>._, A<OrderType>._)).MustHaveHappenedOnceExactly();

        }
    }
}

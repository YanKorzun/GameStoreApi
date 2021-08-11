using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO;
using GameStore.WEB.DTO.ProductModels;

namespace GameStore.BL.Services
{
    public class ProductService : IProductService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ICustomProductAggregator _customProductAggregator;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IProductPropUtility _propUtility;

        public ProductService(IMapper mapper, IProductRepository productRepository,
            ICustomProductAggregator customProductAggregator, ICloudinaryService cloudinaryService,
            IProductPropUtility propUtility)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _customProductAggregator = customProductAggregator;
            _cloudinaryService = cloudinaryService;
            _propUtility = propUtility;
        }

        public async Task<ServiceResult<ProductModel>> CreateProductAsync(InputProductModel model) =>
            await HandleProductAsync(_productRepository.CreateItemAsync, model);

        public async Task<ServiceResult<ProductModel>> UpdateProductAsync(ExtendedInputProductModel model) =>
            await HandleProductAsync(_productRepository.UpdateProductAsync, model);

        public async Task<ServiceResult> DeleteProductAsync(int id)
        {
            var result = await _productRepository.DeleteProductAsync(id);

            return result;
        }

        public async Task<ProductModel> FindProductById(int id)
        {
            var product = await _productRepository.FindProductByIdAsync(id);

            return _mapper.Map<ProductModel>(product);
        }

        public async Task<List<ProductModel>> GetProductsBySearchTermAsync(string term, int limit, int offset)
        {
            var products = await _productRepository.GetProductsBySearchTermAsync(term, limit, offset);

            return _mapper.Map<List<ProductModel>>(products);
        }

        public async Task<List<ProductModel>> GetPagedProductList(ProductParameters productParameters)
        {
            Expression<Func<Product, bool>> expression = null;
            var sortExpression = _propUtility.GetOrderExpression(productParameters);
            if (productParameters.Genre is not null)
            {
                expression = o => o.Genre == productParameters.Genre;
            }

            if (productParameters.AgeRating is not null)
            {
                expression = o => o.AgeRating == productParameters.AgeRating;
            }

            if (productParameters.Genre is not null && productParameters.AgeRating is not null)
            {
                expression = o => o.Genre == productParameters.Genre && o.AgeRating == productParameters.AgeRating;
            }

            var products = await _productRepository.SearchForMultipleItemsAsync(expression,
                (int)productParameters.Offset, (int)productParameters.Limit, sortExpression,
                productParameters.OrderType);

            var modelsList = _mapper.Map<List<ProductModel>>(products);

            return modelsList;
        }

        private async Task<ServiceResult<ProductModel>> HandleProductAsync(
            Func<Product, Task<Product>> createUpdateAsync, InputProductModel model)
        {
            var getUrlsResult = await UploadProductImages(model);
            if (getUrlsResult.Result is not ServiceResultType.Success)
            {
                return new(getUrlsResult.Result);
            }

            var product =
                _customProductAggregator.AggregateProduct(model, getUrlsResult.Data.bgUrl, getUrlsResult.Data.logoUrl);

            var updatedProduct = await createUpdateAsync(product);

            return new(getUrlsResult.Result,
                _mapper.Map<ExtendedProductModel>(updatedProduct));
        }

        private async Task<ServiceResult<(string bgUrl, string logoUrl)>> UploadProductImages(InputProductModel model)
        {
            var backgroundFileUploadResult = await _cloudinaryService.Upload(model.Background);
            var logoFileUploadResult = await _cloudinaryService.Upload(model.Logo);

            if (backgroundFileUploadResult.Result is not ServiceResultType.Success)
            {
                return new(backgroundFileUploadResult.Result,
                    backgroundFileUploadResult.ErrorMessage);
            }

            if (logoFileUploadResult.Result is not ServiceResultType.Success)
            {
                return new(logoFileUploadResult.Result,
                    logoFileUploadResult.ErrorMessage);
            }

            return new(ServiceResultType.Success,
                (backgroundFileUploadResult.Data.Url.ToString(), logoFileUploadResult.Data.Url.ToString()));
        }
    }
}
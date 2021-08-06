using AutoMapper;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Repositories;
using GameStore.WEB.DTO.ProductModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BL.Enums;

namespace GameStore.BL.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICustomProductAggregator _customProductAggregator;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductService(IMapper mapper, IProductRepository productRepository, ICustomProductAggregator customProductAggregator, ICloudinaryService cloudinaryService)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _customProductAggregator = customProductAggregator;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ServiceResult<ProductModel>> CreateProductAsync(InputProductModel model) => await HandleProductAsync(_productRepository.CreateItemAsync, model);

        public async Task<ServiceResult<ProductModel>> UpdateProductAsync(ExtendedInputProductModel model) => await HandleProductAsync(_productRepository.UpdateProductAsync, model);

        public async Task<ServiceResult> DeleteProductAsync(int id)
        {
            var result = await _productRepository.DeleteProductAsync(id);

            return result;
        }

        public async Task<ProductModel> FindProductById(int id)
        {
            var product = await _productRepository.FindProductById(id);

            return _mapper.Map<ProductModel>(product);
        }

        public async Task<List<ProductModel>> GetProductsBySearchTermAsync(string term, int limit, int offset)
        {
            var products = await _productRepository.GetProductsBySearchTermAsync(term, limit, offset);
            return _mapper.Map<List<ProductModel>>(products);
        }

        private async Task<ServiceResult<ProductModel>> HandleProductAsync(Func<Product, Task<Product>> createUpdate, InputProductModel model)
        {
            var getUrlsResult = await GetUrlFromUploadResult(model);
            if (getUrlsResult.Result is not ServiceResultType.Success)
            {
                return new(getUrlsResult.Result);
            }

            var product = _customProductAggregator.InputModelToBasic(model, getUrlsResult.Data);

            var updatedProduct = await createUpdate(product);

            return new(getUrlsResult.Result, _mapper.Map<ExtendedProductModel>(updatedProduct));
        }

        private async Task<ServiceResult<(string bgUrl, string logoUrl)>> GetUrlFromUploadResult(InputProductModel model)
        {
            var first = await _cloudinaryService.Upload(model.Background);
            var second = await _cloudinaryService.Upload(model.Logo);

            if (first.Result is not ServiceResultType.Success)
            {
                return new(first.Result, first.ErrorMessage);
            }
            else if (second.Result is not ServiceResultType.Success)
            {
                return new(second.Result, second.ErrorMessage);
            }

            return new(ServiceResultType.Success, (first.Data.Url.ToString(), second.Data.Url.ToString()));
        }
    }
}
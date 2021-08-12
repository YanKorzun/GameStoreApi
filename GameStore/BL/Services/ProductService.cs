using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.BL.Utilities;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO.Parameters;
using GameStore.WEB.DTO.Products;

namespace GameStore.BL.Services
{
    public class ProductService : IProductService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ICustomProductAggregator _customProductAggregator;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IMapper mapper, IProductRepository productRepository,
            ICustomProductAggregator customProductAggregator, ICloudinaryService cloudinaryService)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _customProductAggregator = customProductAggregator;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ServiceResult<ProductDto>> CreateProductAsync(InputProductDto dto) =>
            await HandleProductAsync(_productRepository.CreateItemAsync, dto);

        public async Task<ServiceResult<ProductDto>> UpdateProductAsync(ExtendedInputProductDto dto) =>
            await HandleProductAsync(_productRepository.UpdateProductAsync, dto);

        public async Task<ServiceResult> DeleteProductAsync(int id)
        {
            var result = await _productRepository.DeleteProductAsync(id);

            return result;
        }

        public async Task<ProductDto> FindProductById(int id)
        {
            var product = await _productRepository.FindProductByIdAsync(id);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<List<ProductDto>> GetProductsBySearchTermAsync(string term, int limit, int offset)
        {
            var products = await _productRepository.GetProductsBySearchTermAsync(term, limit, offset);

            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<List<ProductDto>> GetPagedProductList(ProductParametersDto productParametersDto)
        {
            var sortExpression = ProductPropUtility.GetOrderExpression(productParametersDto);
            var filterExpression = ProductPropUtility.GetFilterExpression(productParametersDto);

            var products = await _productRepository.SearchForMultipleItemsAsync(filterExpression,
                productParametersDto.Offset, productParametersDto.Limit, sortExpression,
                productParametersDto.OrderType);

            var modelsList = _mapper.Map<List<ProductDto>>(products);

            return modelsList;
        }

        private async Task<ServiceResult<ProductDto>> HandleProductAsync(
            Func<Product, Task<Product>> createUpdateAsync, InputProductDto dto)
        {
            var getUrlsResult = await UploadProductImages(dto);
            if (getUrlsResult.Result is not ServiceResultType.Success)
            {
                return new(getUrlsResult.Result);
            }

            var product =
                _customProductAggregator.AggregateProduct(dto, getUrlsResult.Data.bgUrl, getUrlsResult.Data.logoUrl);

            var updatedProduct = await createUpdateAsync(product);

            return new(getUrlsResult.Result,
                _mapper.Map<ExtendedProductDto>(updatedProduct));
        }

        private async Task<ServiceResult<(string bgUrl, string logoUrl)>> UploadProductImages(InputProductDto dto)
        {
            var backgroundFileUploadResult = await _cloudinaryService.Upload(dto.Background);
            var logoFileUploadResult = await _cloudinaryService.Upload(dto.Logo);

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
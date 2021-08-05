using AutoMapper;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Repositories;
using GameStore.WEB.DTO.ProductModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BL.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICustomProductMapper _customProductMapper;

        public ProductService(IMapper mapper, IProductRepository productRepository, ICustomProductMapper customProductMapper)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _customProductMapper = customProductMapper;
        }

        public async Task<ProductModel> CreateProductAsync(InputProductModel model) => await HandleProductAsync(_productRepository.CreateItemAsync, model);

        public async Task<ProductModel> UpdateProductAsync(ExtendedInputProductModel model) => await HandleProductAsync(_productRepository.UpdateProductAsync, model);

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

        private async Task<ProductModel> HandleProductAsync(Func<Product, Task<Product>> createUpdate, InputProductModel model)
        {
            var product = await _customProductMapper.InputModelToBasic(model);

            var updatedProduct = await createUpdate(product);

            return _mapper.Map<ProductModel>(updatedProduct);
        }
    }
}
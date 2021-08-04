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
    public class ProductService<T> : IProductService<T>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ProductModel> CreateProductAsync(T model) => await CUMethod(_productRepository.CreateItemAsync, model);

        public async Task<ProductModel> UpdateProductAsync(T model) => await CUMethod(_productRepository.UpdateProductAsync, model);

        public async Task<ProductModel> CUMethod(Func<Product, Task<Product>> createUpdate, T model)
        {
            var product = _mapper.Map<Product>(model);

            var updatedProduct = await createUpdate(product);

            return _mapper.Map<ProductModel>(updatedProduct);
        }

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
    }
}
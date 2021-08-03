using AutoMapper;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Repositories;
using GameStore.WEB.DTO.ProductModels;
using System.Threading.Tasks;

namespace GameStore.BL.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<Product> CreateNewProductAsync(ProductModel productModel)
        {
            var product = _mapper.Map<Product>(productModel);

            var createdProduct = await _productRepository.CreateItemAsync(product);

            return createdProduct;
        }

        public async Task<Product> UpdateProductAsync(ProductModel productModel)
        {
            var product = _mapper.Map<Product>(productModel);

            var updatedProduct = await _productRepository.UpdateProductAsync(product);

            return updatedProduct;
        }

        public async Task<ServiceResult> DeleteProductAsync(int id)
        {
            var result = await _productRepository.DeleteProductAsync(id);

            return result;
        }
    }
}
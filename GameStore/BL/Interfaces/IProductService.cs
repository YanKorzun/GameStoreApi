using GameStore.BL.ResultWrappers;
using GameStore.WEB.DTO.ProductModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface IProductService<T>
    {
        public Task<ProductModel> CreateProductAsync(T productModel);

        public Task<ServiceResult> DeleteProductAsync(int id);

        public Task<ProductModel> UpdateProductAsync(T productModel);

        public Task<ProductModel> FindProductById(int id);

        public Task<List<ProductModel>> GetProductsBySearchTermAsync(string term, int limit, int offset);
    }
}
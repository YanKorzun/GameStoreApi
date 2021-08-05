using GameStore.BL.ResultWrappers;
using GameStore.WEB.DTO.ProductModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface IProductService
    {
        public Task<ProductModel> CreateProductAsync(InputProductModel productModel);

        public Task<ServiceResult> DeleteProductAsync(int id);

        public Task<ProductModel> UpdateProductAsync(ExtendedInputProductModel productModel);

        public Task<ProductModel> FindProductById(int id);

        public Task<List<ProductModel>> GetProductsBySearchTermAsync(string term, int limit, int offset);
    }
}
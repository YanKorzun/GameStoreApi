using GameStore.BL.ResultWrappers;
using GameStore.WEB.DTO.ProductModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface IProductService
    {
        Task<ProductModel> CreateProductAsync(InputProductModel productModel);

        Task<ServiceResult> DeleteProductAsync(int id);

        Task<ProductModel> UpdateProductAsync(ExtendedInputProductModel productModel);

        Task<ProductModel> FindProductById(int id);

        Task<List<ProductModel>> GetProductsBySearchTermAsync(string term, int limit, int offset);
    }
}
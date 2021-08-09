using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BL.ResultWrappers;
using GameStore.WEB.DTO.ProductModels;

namespace GameStore.BL.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<ProductModel>> CreateProductAsync(InputProductModel productModel);

        Task<ServiceResult> DeleteProductAsync(int id);

        Task<ServiceResult<ProductModel>> UpdateProductAsync(ExtendedInputProductModel productModel);

        Task<ProductModel> FindProductById(int id);

        Task<List<ProductModel>> GetProductsBySearchTermAsync(string term, int limit, int offset);
    }
}
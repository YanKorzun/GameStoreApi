using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BL.ResultWrappers;
using GameStore.WEB.DTO.Parameters;
using GameStore.WEB.DTO.Products;

namespace GameStore.BL.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<ProductDto>> CreateProductAsync(InputProductDto productDto);

        Task<ServiceResult> DeleteProductAsync(int id);

        Task<ServiceResult<ProductDto>> UpdateProductAsync(ExtendedInputProductDto productDto);

        Task<ProductDto> FindProductByIdAsync(int id);

        Task<List<ProductDto>> GetProductsBySearchTermAsync(string term, int limit, int offset);

        Task<List<ProductDto>> GetPagedProductListAsync(ProductParametersDto productParametersDto);
    }
}
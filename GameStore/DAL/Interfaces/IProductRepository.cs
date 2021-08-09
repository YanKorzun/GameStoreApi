using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BL.ResultWrappers;
using GameStore.BL.Utilities;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.WEB.DTO;

namespace GameStore.DAL.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<ProductPlatforms>> GetPopularPlatformsAsync(int platformCount);

        Task<List<Product>> GetProductsBySearchTermAsync(string searchTerm, int limit, int skippedCount);

        Task<Product> FindProductById(int productId);

        Task<ServiceResult> DeleteProductAsync(int id);

        Task<Product> UpdateProductAsync(Product newProduct);

        PagedList<Product> GetOwners(ProductParameters productParameters);
    }
}
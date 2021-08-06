using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<ProductPlatforms>> GetPopularPlatformsAsync(int platformCount);

        Task<List<Product>> GetProductsBySearchTermAsync(string searchTerm, int limit, int skipedCount);

        Task<Product> FindProductById(int productId);

        Task<ServiceResult> DeleteProductAsync(int id);

        Task<Product> UpdateProductAsync(Product newProduct);
    }
}
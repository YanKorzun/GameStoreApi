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
        public Task<List<ProductPlatforms>> GetPopularPlatformsAsync(int platformCount);

        public Task<List<Product>> GetProductsBySearchTermAsync(string searchTerm, int limit, int skipedCount);

        public Task<Product> FindProductById(int productId);

        public Task<Product> UpdateProductAsync(Product newProduct);

        public Task<ServiceResult> DeleteProductAsync(int id);
    }
}
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public Task<List<ProductPlatforms>> GetPopularPlatformsAsync(int platformCount);

        public Task<List<Product>> GetProductsBySearchTermAsync(string searchTerm, int limit, int skipedCount);
    }
}
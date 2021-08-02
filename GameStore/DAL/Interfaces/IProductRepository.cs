using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public Task<ServiceResult<IList<string>>> GetPopularPlatforms();
    }
}
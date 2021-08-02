using GameStore.BL.Enums;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ServiceResult<IList<ProductPlatforms>> GetPopularPlatforms()
        {
            var topPlatforms = _entity.GroupBy(x => x.Platform).OrderByDescending(g => g.Count()).Select(p => p.Key).Take(3).ToList();

            return new(ServiceResultType.Success, topPlatforms);
        }

        public async Task<List<Product>> GetProductsBySearchTerm(string searchTerm, int limit, int skipedCount)
        {
            var query = from teams in _entity
                    .AsNoTracking()
                    .Where(x => EF.Functions.Like(x.Name, $"{searchTerm}%"))
                    .Take(limit)
                    .Skip(skipedCount)
                        select teams;

            var teamEntities = await query.ToListAsync();

            return teamEntities;
        }
    }
}
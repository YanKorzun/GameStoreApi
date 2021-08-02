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

        public async Task<List<ProductPlatforms>> GetPopularPlatformsAsync(int platformCount) =>
            await _entity
                    .GroupBy(x => x.Platform)
                    .OrderByDescending(g => g.Count())
                    .Select(p => p.Key)
                    .Take(platformCount)
                    .ToListAsync();

        public async Task<List<Product>> GetProductsBySearchTermAsync(string searchTerm, int limit, int skipedCount) =>
            await _entity
                    .AsNoTracking()
                    .Where(x => EF.Functions.Like(x.Name, $"{searchTerm}%"))
                    .Take(limit)
                    .Skip(skipedCount)
                    .Select(x => x).ToListAsync();
    }
}
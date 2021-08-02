using GameStore.BL.Enums;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ServiceResult<IList<string>>> GetPopularPlatforms()
        {
            var products = await _entity.ToListAsync();
            var topPlatforms = products.GroupBy(x => x.Platform).OrderByDescending(g => g.Count()).SelectMany(x => x).ToList().GroupBy(o => o.Platform).Select(p => p.Key.ToString()).ToList();

            return new(ServiceResultType.Success, topPlatforms);
        }

        private async Task<IList<Product>> GetProductsWithChildren(Expression<Func<Product, bool>> expression)
            => await _entity.AsNoTracking().Include(o => o.ProductLibraries).ThenInclude(o => o.AppUser).ToListAsync();
    }
}
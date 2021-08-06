using GameStore.BL.Enums;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<ProductPlatforms>> GetPopularPlatformsAsync(int platformCount) =>
            await Entity
                    .GroupBy(x => x.Platform)
                    .OrderByDescending(g => g.Count())
                    .Select(p => p.Key)
                    .Take(platformCount)
                    .ToListAsync();

        public async Task<List<Product>> GetProductsBySearchTermAsync(string searchTerm, int limit, int skippedCount) =>
            await Entity
                    .AsNoTracking()
                    .Where(x => EF.Functions.Like(x.Name, $"{searchTerm}%"))
                    .Take(limit)
                    .Skip(skippedCount)
                    .Select(x => x).ToListAsync();

        public async Task<Product> FindProductById(int productId) => await GetProductWithChildrenAsync(o => o.Id == productId);

        public async Task<Product> UpdateProductAsync(Product newProduct)
        {
            var updatedProduct = await UpdateItemAsync(newProduct,
                x => x.Id,
                x => x.DateCreated,
                x => x.PublicationDate);

            return updatedProduct;
        }

        public async Task<ServiceResult> DeleteProductAsync(int id)
        {
            var dbProduct = new Product()
            {
                Id = id,
                IsDeleted = true
            };

            DbContext.Entry(dbProduct).Property(x => x.IsDeleted).IsModified = true;

            await DbContext.SaveChangesAsync();

            return new(ServiceResultType.Success);
        }

        private async Task<Product> GetProductWithChildrenAsync(Expression<Func<Product, bool>> expression)
            => await Entity.AsNoTracking().Include(o => o.ProductLibraries).ThenInclude(o => o.AppUser).Include(o => o.Ratings).FirstOrDefaultAsync(expression);

        private async Task<Product> GetProductAsync(Expression<Func<Product, bool>> expression)
            => await Entity.AsNoTracking().FirstOrDefaultAsync(expression);
    }
}
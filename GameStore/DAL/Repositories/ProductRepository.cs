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

        public async Task<Product> FindProductById(int productID) => await GetProductWithChildrenAsync(o => o.Id == productID);

        public async Task<Product> UpdateProductAsync(Product newProduct)
        {
            var dbProduct = await GetProductAsync(o => o.Id == newProduct.Id);

            _dbContext.Attach(dbProduct);

            dbProduct.Name = newProduct.Name;
            dbProduct.Developers = newProduct.Developers;
            dbProduct.Publishers = newProduct.Publishers;
            dbProduct.Genre = newProduct.Genre;
            dbProduct.Rating = newProduct.Rating;
            dbProduct.Logo = newProduct.Logo;
            dbProduct.Background = newProduct.Background;
            dbProduct.Price = newProduct.Price;
            dbProduct.Count = newProduct.Count;
            dbProduct.DateCreated = newProduct.DateCreated;
            dbProduct.TotalRating = newProduct.TotalRating;
            dbProduct.Platform = newProduct.Platform;
            dbProduct.PublicationDate = newProduct.PublicationDate;

            await _dbContext.SaveChangesAsync();

            return dbProduct;
        }

        public async Task<ServiceResult> DeleteProductAsync(int id)
        {
            var dbProduct = await GetProductAsync(o => o.Id == id);
            if (dbProduct is null)
            {
                return new(ServiceResultType.InvalidData, $"Product with id {id} doesn't exist");
            }

            dbProduct.isDeleted = true;
            _dbContext.Entry(dbProduct).Property(x => x.isDeleted).IsModified = true;

            await _dbContext.SaveChangesAsync();

            return new(ServiceResultType.Success);
        }

        private async Task<Product> GetProductWithChildrenAsync(Expression<Func<Product, bool>> expression)
            => await _entity.AsNoTracking().Include(o => o.ProductLibraries).ThenInclude(o => o.AppUser).FirstOrDefaultAsync(expression);

        private async Task<Product> GetProductAsync(Expression<Func<Product, bool>> expression)
            => await _entity.AsNoTracking().FirstOrDefaultAsync(expression);
    }
}
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Repositories
{
    public class ProductRatingRepository : BaseRepository<ProductRating>, IProductRatingRepository
    {
        public ProductRatingRepository(ApplicationDbContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<ProductRating> CreateRatingAsync(ProductRating rating) => await CreateItemAsync(rating);

        public async Task<List<ProductRating>> GetRatingsAsync(Expression<Func<ProductRating, bool>> expression) =>
            await Entity.AsNoTracking().Where(expression).ToListAsync();
    }
}
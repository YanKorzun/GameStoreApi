using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.DAL.Entities;

namespace GameStore.DAL.Interfaces
{
    public interface IProductRatingRepository : IBaseRepository<ProductRating>
    {
        Task<ProductRating> CreateRatingAsync(ProductRating rating);

        Task<List<ProductRating>> GetRatingsAsync(Expression<Func<ProductRating, bool>> expression);
    }
}
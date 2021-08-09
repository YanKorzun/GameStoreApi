using System.Threading.Tasks;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Repositories
{
    public class ProductRatingRepository : BaseRepository<ProductRating>, IProductRatingRepository
    {
        public ProductRatingRepository(ApplicationDbContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<ProductRating> CreateRatingAsync(ProductRating rating) => await CreateItemAsync(rating);
    }
}
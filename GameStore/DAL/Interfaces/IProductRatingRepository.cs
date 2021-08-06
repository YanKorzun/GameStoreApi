using System.Threading.Tasks;
using GameStore.DAL.Entities;

namespace GameStore.DAL.Interfaces
{
    public interface IProductRatingRepository
    {
        Task<ProductRating> CreateRatingAsync(ProductRating rating);
    }
}
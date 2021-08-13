using System.Threading.Tasks;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Ratings;

namespace GameStore.BL.Interfaces
{
    public interface IProductRatingService
    {
        Task<ProductRating> CreateProductRatingAsync(int userId, RatingDto ratingDto);
    }
}
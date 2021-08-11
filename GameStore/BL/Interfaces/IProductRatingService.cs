using System.Security.Claims;
using System.Threading.Tasks;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.RatingModels;

namespace GameStore.BL.Interfaces
{
    public interface IProductRatingService
    {
        Task<ProductRating> CreateProductRatingAsync(ClaimsPrincipal contextUser, RatingModel ratingModel);
    }
}
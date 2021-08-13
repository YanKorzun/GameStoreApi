using System.Linq;
using System.Threading.Tasks;
using GameStore.BL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO.Ratings;

namespace GameStore.BL.Services
{
    public class ProductRatingService : IProductRatingService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductRatingRepository _ratingRepository;

        public ProductRatingService(IProductRatingRepository ratingRepository,
            IProductRepository productRepository)
        {
            _ratingRepository = ratingRepository;
            _productRepository = productRepository;
        }

        public async Task<ProductRating> CreateProductRatingAsync(int userId, RatingDto ratingDto)
        {
            var updatedRating = await _ratingRepository.CreateRatingAsync(new(userId,
                ratingDto.ProductId,
                ratingDto.Rating));

            var ratings = await _ratingRepository.GetRatingsAsync(o => o.ProductId == ratingDto.ProductId);

            var product = await _productRepository.FindProductByIdAsync(ratingDto.ProductId);

            product.TotalRating = ratings.Average(o => o.Rating);

            await _productRepository.UpdateItemWithModifiedPropsAsync(product, o => o.TotalRating);

            return updatedRating;
        }
    }
}
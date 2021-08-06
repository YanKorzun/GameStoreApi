using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO.RatingModels;

namespace GameStore.BL.Services
{
    public class ProductRatingService : IProductRatingService
    {
        private readonly IClaimsUtility _claimsUtility;
        private readonly IProductRatingRepository _ratingRepository;

        public ProductRatingService(IClaimsUtility claimsUtility, IProductRatingRepository ratingRepository)
        {
            _claimsUtility = claimsUtility;
            _ratingRepository = ratingRepository;
        }

        public async Task<ProductRating> CreateProductRatingAsync(ClaimsPrincipal contextUser, RatingModel ratingModel)
        {
            var getUserIdResult = _claimsUtility.GetUserIdFromClaims(contextUser);
            if (getUserIdResult.Result is not ServiceResultType.Success)
            {
            }

            return await _ratingRepository.CreateRatingAsync(new ProductRating(getUserIdResult.Data, ratingModel.ProductId,
                ratingModel.Rating));
        }
    }
}
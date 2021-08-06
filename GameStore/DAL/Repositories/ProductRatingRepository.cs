using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO.RatingModels;
using Org.BouncyCastle.Asn1.Crmf;

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
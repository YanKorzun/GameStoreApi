using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.WEB.DTO.RatingModels
{
    public class RatingModel
    {
        public RatingModel(int productId, int rating)
        {
            ProductId = productId;
            Rating = rating;
        }

        public int ProductId { get; set; }
        public int Rating { get; set; }
    }
}
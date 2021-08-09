using System.ComponentModel.DataAnnotations;

namespace GameStore.DAL.Entities
{
    public class ProductRating
    {
        public ProductRating(int userId, int productId, int rating)
        {
            UserId = userId;
            ProductId = productId;
            Rating = rating;
        }

        [Key] public int RatingId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int UserId { get; set; }

        public int Rating { get; set; }
    }
}
namespace GameStore.WEB.DTO.Ratings
{
    public class RatingDto
    {
        public RatingDto(int productId, int rating)
        {
            ProductId = productId;
            Rating = rating;
        }

        /// <summary>
        /// Id of the product
        /// </summary>
        /// <example>file</example>
        public int ProductId { get; set; }

        /// <summary>
        /// Rating of the product
        /// </summary>
        /// <example>file</example>
        public int Rating { get; set; }
    }
}
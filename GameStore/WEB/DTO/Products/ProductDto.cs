using System.Collections.Generic;
using GameStore.DAL.Entities;

namespace GameStore.WEB.DTO.Products
{
    public class ProductDto : InputProductDto
    {
        /// <summary>
        /// Ratings of the product
        /// </summary>
        /// <example>
        /// {
        ///     "userId":1,
        ///     "productId":2,
        ///     rating:1
        /// }
        /// </example>
        public ICollection<ProductRating> Ratings { get; set; }

        /// <summary>
        /// The logo of the product link
        /// </summary>
        /// <example>https://res.cloudinary.com/dbu4voh2q/image/upload/v1628497740/jvh8w1o6qxmaz3oelszv.jpg</example>
        public new string Logo { get; set; }

        /// <summary>
        /// The logo background link
        /// </summary>
        /// <example>https://res.cloudinary.com/dbu4voh2q/image/upload/v1628497740/jvh8w1o6qxmaz3oelszv.jpg</example>
        public new string Background { get; set; }
    }
}
using System.Collections.Generic;
using GameStore.DAL.Entities;

namespace GameStore.WEB.DTO.ProductModels
{
    public class ProductModel : InputProductModel
    {
        public ICollection<ProductRating> Ratings { get; set; }
        public new string Logo { get; set; }
        public new string Background { get; set; }
    }
}
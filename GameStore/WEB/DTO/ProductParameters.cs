using GameStore.DAL.Enums;

namespace GameStore.WEB.DTO
{
    public class ProductParameters : QueryStringParameters
    {
        public AgeProductRating AgeRating { get; set; }

        public string Genre { get; set; }

        public string Name { get; set; }
    }
}
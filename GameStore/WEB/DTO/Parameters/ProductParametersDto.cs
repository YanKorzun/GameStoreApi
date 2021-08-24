using GameStore.DAL.Enums;

namespace GameStore.WEB.DTO.Parameters
{
    public class ProductParametersDto : QueryStringParametersDto
    {
        public AgeProductRating? AgeRating { get; set; }

        public Genre? Genre { get; set; }

        public string Name { get; set; }
    }
}
using GameStore.DAL.Enums;
using GameStore.WEB.Constants;

namespace GameStore.WEB.DTO.Parameters
{
    public abstract class QueryStringParametersDto
    {
        public int Limit { get; set; } = FiltersConstants.PageSize;
        public int Offset { get; set; }
        public virtual string OrderBy { get; set; }
        public OrderType OrderType { get; set; }
    }
}
using GameStore.DAL.Enums;
using GameStore.WEB.Constants;

namespace GameStore.WEB.DTO
{
    public abstract class QueryStringParameters
    {
        public int Limit { get; set; } = FiltersConstants.PageSize;
        public int Offset { get; set; }
        public virtual string OrderBy { get; set; }
        public OrderType OrderType { get; set; }
    }
}
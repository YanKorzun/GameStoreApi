using GameStore.WEB.Constants;

namespace GameStore.WEB.DTO
{
    public abstract class QueryStringParameters
    {
        private uint _pageNumber = FiltersConstants.PageNumber;
        private uint _pageSize = FiltersConstants.PageSize;

        /// <summary>
        ///     Page number
        /// </summary>
        /// <example>4</example>
        public uint PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value <= 0 ? FiltersConstants.PageNumber : value;
        }

        /// <summary>
        ///     Page size
        /// </summary>
        /// <example>2</example>
        public uint PageSize
        {
            get => _pageSize;
            set => _pageSize = value > FiltersConstants.MaxPageSize ? FiltersConstants.MaxPageSize : value;
        }

        /// <summary>
        ///     Convertible order by query
        /// </summary>
        /// <example>Name desc</example>
        public string OrderBy { get; set; }
    }
}
namespace GameStore.WEB.DTO.Orders
{
    /// <summary>
    /// Extends basic order model by Id property
    /// </summary>
    public class ExtendedOrderDto : BasicOrderDto
    {
        /// <summary>
        /// Order id
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
    }
}
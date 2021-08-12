namespace GameStore.WEB.DTO.OrderModels
{
    /// <summary>
    /// Extends basic order model by Id property
    /// </summary>
    public class ExtendedOrderModel : BasicOrderModel
    {
        /// <summary>
        /// Order id
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
    }
}
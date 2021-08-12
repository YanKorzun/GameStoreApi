namespace GameStore.WEB.DTO.OrderModels
{
    /// <summary>
    /// Basic order model for  creating and updating entities
    /// <remarks>
    /// <para>
    /// Contains public fields:
    /// </para>
    /// Product id<br/>
    /// ApplicationUserId<br/>
    /// Count
    /// </remarks>
    /// </summary>
    public class BasicOrderModel
    {
        /// <summary>
        /// Product's id
        /// </summary>
        /// <example>8</example>
        public int ProductId { get; set; }

        /// <summary>
        /// User's id
        /// </summary>
        /// <example>2</example>
        public int ApplicationUserId { get; set; }

        /// <summary>
        /// Products count
        /// </summary>
        /// <example>2</example>
        public int Count { get; set; }
    }
}
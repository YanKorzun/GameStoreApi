using System;

namespace GameStore.WEB.DTO.OrderModels
{
    public class ExtendedOrderModel : OrderModel
    {
        /// <summary>
        /// Order id
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// /// <summary>
        /// Order creation date
        /// </summary>
        /// <example>2021-08-11T13:45:32.6473559+03:00</example>
        public DateTime OrderDate { get; set; }
    }
}
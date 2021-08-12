using System;
using GameStore.DAL.Enums;

namespace GameStore.WEB.DTO.OrderModels
{
    /// <summary>
    /// Order model for presentation order data to user
    /// </summary>
    public class OutOrderModel : ExtendedOrderModel
    {
        public OrderStatus Status { get; set; }
        public DateTime CreateOrderDate { get; set; }
        public DateTime? UpdateOrderDate { get; set; }
    }
}
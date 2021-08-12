using System;
using GameStore.DAL.Enums;

namespace GameStore.WEB.DTO.Orders
{
    /// <summary>
    /// Order model for presentation order data to user
    /// </summary>
    public class OutOrderDto : ExtendedOrderDto
    {
        public OrderStatus Status { get; set; }

        public DateTime CreateOrderDate { get; set; }

        public DateTime? UpdateOrderDate { get; set; }
    }
}
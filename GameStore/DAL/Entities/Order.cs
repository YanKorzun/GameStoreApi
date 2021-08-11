using System;
using GameStore.DAL.Enums;

namespace GameStore.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public bool IsDeleted { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductId { get; set; }
        public int ApplicationUserId { get; set; }
        public int Count { get; set; }
    }
}
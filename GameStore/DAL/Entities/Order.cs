using System.ComponentModel.DataAnnotations;
using GameStore.DAL.Enums;

namespace GameStore.DAL.Entities
{
    public class Order
    {
        [Key] public int Id { get; set; }

        public ApplicationUser AppUser { get; set; }

        public bool IsDeleted { get; set; }
        public OrderStatus Status { get; set; }

        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Count { get; set; }
    }
}
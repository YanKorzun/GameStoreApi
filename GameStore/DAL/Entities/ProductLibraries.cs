using System;

namespace GameStore.DAL.Entities
{
    public class ProductLibraries
    {
        public ProductLibraries()
        {
            PurchaseDate = DateTime.Now;
        }

        public ProductLibraries(int userId, int gameId)
        {
            UserId = userId;
            GameId = gameId;
        }

        public bool IsDeleted { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public ApplicationUser AppUser { get; set; }
        public Product Game { get; set; }
    }
}
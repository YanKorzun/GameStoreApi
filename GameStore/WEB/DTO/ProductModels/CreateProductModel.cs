using GameStore.DAL.Enums;
using System;

namespace GameStore.WEB.DTO.ProductModels
{
    public class CreateProductModel
    {
        public string Name { get; set; }
        public string Developers { get; set; }
        public string Publishers { get; set; }
        public string Genre { get; set; }
        public ProductRating Rating { get; set; }
        public string Logo { get; set; }
        public string Background { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }
        public int TotalRating { get; set; }
        public ProductPlatforms Platform { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
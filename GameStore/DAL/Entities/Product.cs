using System;
using System.Collections.Generic;
using GameStore.DAL.Enums;

namespace GameStore.DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Developers { get; set; }
        public string Publishers { get; set; }
        public string Genre { get; set; }
        public AgeProductRating AgeRating { get; set; }
        public string Logo { get; set; }
        public string Background { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }
        public int TotalRating { get; set; }
        public ProductPlatforms Platform { get; set; }
        public DateTime PublicationDate { get; set; }
        public IList<ProductLibraries> ProductLibraries { get; set; } = new List<ProductLibraries>();
        public ICollection<ProductRating> Ratings { get; set; }
    }
}
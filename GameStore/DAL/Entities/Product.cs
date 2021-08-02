using System;
using System.Collections.Generic;

namespace GameStore.DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Developers { get; set; }
        public string Publishers { get; set; }
        public DateTime DateCreated { get; set; }
        public int TotalRating { get; set; }
        public ProductPlatforms Platform { get; set; }
        public DateTime PublicationDate { get; set; }
        public IList<ProductLibraries> ProductLibraries { get; set; } = new List<ProductLibraries>();
    }
}
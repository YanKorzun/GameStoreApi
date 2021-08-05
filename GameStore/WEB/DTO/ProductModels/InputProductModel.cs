using GameStore.DAL.Enums;
using Microsoft.AspNetCore.Http;
using System;

namespace GameStore.WEB.DTO.ProductModels
{
    public class InputProductModel
    {
        public string Name { get; set; }
        public string Developers { get; set; }
        public string Publishers { get; set; }
        public string Genre { get; set; }
        public ProductRating Rating { get; set; }
        public IFormFile Logo { get; set; }
        public IFormFile Background { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }
        public int TotalRating { get; set; }
        public ProductPlatforms Platform { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
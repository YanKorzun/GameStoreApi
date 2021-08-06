using GameStore.DAL.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.DTO.ProductModels
{
    public class InputProductModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Developers { get; set; }

        [Required]
        [StringLength(50)]
        public string Publishers { get; set; }

        [Required]
        [StringLength(50)]
        public string Genre { get; set; }

        public AgeProductRating Rating { get; set; }
        public IFormFile Logo { get; set; }
        public IFormFile Background { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public int TotalRating { get; set; }

        public ProductPlatforms Platform { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }
    }
}
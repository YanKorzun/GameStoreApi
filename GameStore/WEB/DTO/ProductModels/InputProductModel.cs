using System;
using System.ComponentModel.DataAnnotations;
using GameStore.DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace GameStore.WEB.DTO.ProductModels
{
    public class InputProductModel
    {
        /// <summary>
        /// The name of the product
        /// </summary>
        /// <example>Brawl Stars</example>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// The developers of the product
        /// </summary>
        /// <example>Supercell</example>
        [Required]
        [StringLength(50)]
        public string Developers { get; set; }

        /// <summary>
        /// The publishers of the product
        /// </summary>
        /// <example>Supercell</example>
        [Required]
        [StringLength(50)]
        public string Publishers { get; set; }

        /// <summary>
        /// The genre of the product
        /// </summary>
        /// <example>Moba</example>
        [Required]
        [StringLength(50)]
        public string Genre { get; set; }

        /// <summary>
        /// The age rating of the product
        /// </summary>
        /// <example>2</example>
        public AgeProductRating Rating { get; set; }

        /// <summary>
        /// The logo of the product
        /// </summary>
        /// <example>file</example>
        public IFormFile Logo { get; set; }

        /// <summary>
        /// The logo background
        /// </summary>
        /// <example>file</example>
        public IFormFile Background { get; set; }

        /// <summary>
        /// Price of the product
        /// </summary>
        /// <example>140</example>
        public decimal Price { get; set; }

        /// <summary>
        /// Count of the products
        /// </summary>
        /// <example>7</example>
        public int Count { get; set; }

        /// <summary>
        /// Creation of the product date
        /// </summary>
        /// <example>2021-08-05T08:13:56.083Z</example>
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Average rating of the product
        /// </summary>
        /// <example>9.87</example>
        public double TotalRating { get; set; }

        /// <summary>
        /// Platform of the product
        /// </summary>
        /// <example>file</example>
        public ProductPlatforms Platform { get; set; }

        /// <summary>
        /// Publication of the product date
        /// </summary>
        /// <example>2021-08-05T08:13:56.083Z</example>
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }
    }
}
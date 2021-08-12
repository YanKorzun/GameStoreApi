using GameStore.BL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Products;

namespace GameStore.BL.Aggregators
{
    public class ProductAggregator : ICustomProductAggregator
    {
        public Product AggregateProduct(InputProductDto inputBasicProductDto, string backgroundUrl, string logoUrl)
        {
            var product = new Product();
            if (inputBasicProductDto is ExtendedInputProductDto castResult)
            {
                product.Id = castResult.Id;
            }

            product.Name = inputBasicProductDto.Name;
            product.Developers = inputBasicProductDto.Developers;
            product.Publishers = inputBasicProductDto.Publishers;
            product.Genre = inputBasicProductDto.Genre;
            product.AgeRating = inputBasicProductDto.Rating;
            product.Price = inputBasicProductDto.Price;
            product.Count = inputBasicProductDto.Count;
            product.DateCreated = inputBasicProductDto.DateCreated;
            product.TotalRating = inputBasicProductDto.TotalRating;
            product.Platform = inputBasicProductDto.Platform;
            product.PublicationDate = inputBasicProductDto.PublicationDate;

            product.Background = backgroundUrl;
            product.Logo = logoUrl;

            return product;
        }
    }
}
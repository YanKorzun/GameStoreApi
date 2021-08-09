using GameStore.BL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.ProductModels;

namespace GameStore.BL.Aggregators
{
    public class ProductAggregator : ICustomProductAggregator
    {
        public Product AggregateProduct(InputProductModel inputBasicProductModel, string backgroundUrl, string logoUrl)
        {
            var product = new Product();
            if (inputBasicProductModel is ExtendedInputProductModel castResult)
            {
                product.Id = castResult.Id;
            }

            product.Name = inputBasicProductModel.Name;
            product.Developers = inputBasicProductModel.Developers;
            product.Publishers = inputBasicProductModel.Publishers;
            product.Genre = inputBasicProductModel.Genre;
            product.Rating = inputBasicProductModel.Rating;
            product.Price = inputBasicProductModel.Price;
            product.Count = inputBasicProductModel.Count;
            product.DateCreated = inputBasicProductModel.DateCreated;
            product.TotalRating = inputBasicProductModel.TotalRating;
            product.Platform = inputBasicProductModel.Platform;
            product.PublicationDate = inputBasicProductModel.PublicationDate;

            product.Background = backgroundUrl;
            product.Logo = logoUrl;

            return product;
        }
    }
}
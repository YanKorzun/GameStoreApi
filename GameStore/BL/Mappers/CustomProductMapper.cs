using GameStore.BL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.ProductModels;
using System.Threading.Tasks;

namespace GameStore.BL.Mappers
{
    public class CustomProductMapper : ICustomProductMapper

    {
        private readonly ICloudinaryService _cloudinary;

        public CustomProductMapper(ICloudinaryService cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<Product> InputModelToBasic(InputProductModel inputBasicProductModel)
        {
            var product = new Product();
            if (inputBasicProductModel is ExtendedInputProductModel castResult)
            {
                product.Id = castResult.Id;
            }

            product.Name = inputBasicProductModel.Name;
            product.Developers = inputBasicProductModel.Name;
            product.Publishers = inputBasicProductModel.Publishers;
            product.Genre = inputBasicProductModel.Genre;
            product.Rating = inputBasicProductModel.Rating;
            product.Price = inputBasicProductModel.Price;
            product.Count = inputBasicProductModel.Count;
            product.DateCreated = inputBasicProductModel.DateCreated;
            product.TotalRating = inputBasicProductModel.TotalRating;
            product.Platform = inputBasicProductModel.Platform;
            product.PublicationDate = inputBasicProductModel.PublicationDate;

            product.Background = (await _cloudinary.Upload(inputBasicProductModel.Background)).Url.ToString();
            product.Logo = (await _cloudinary.Upload(inputBasicProductModel.Logo)).Url.ToString();

            return product;
        }
    }
}
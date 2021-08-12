using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Products;

namespace GameStore.BL.Interfaces
{
    public interface ICustomProductAggregator
    {
        Product AggregateProduct(InputProductDto inputBasicProductDto, string backgroundUrl, string logoUrl);
    }
}
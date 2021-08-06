using GameStore.DAL.Entities;
using GameStore.WEB.DTO.ProductModels;
using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface ICustomProductAggregator
    {
        Product InputModelToBasic(InputProductModel inputBasicProductModel, (string backgroundUrl, string logoUrl) urlTuple);
    }
}
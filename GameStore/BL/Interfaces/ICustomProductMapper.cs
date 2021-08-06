using GameStore.DAL.Entities;
using GameStore.WEB.DTO.ProductModels;
using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface ICustomProductMapper
    {
        Task<Product> InputModelToBasic(InputProductModel inputBasicProductModel);
    }
}
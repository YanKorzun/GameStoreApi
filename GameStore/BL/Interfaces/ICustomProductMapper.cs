using GameStore.DAL.Entities;
using GameStore.WEB.DTO.ProductModels;
using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface ICustomProductMapper
    {
        public Task<Product> InputModelToBasic(InputProductModel inputBasicProductModel);
    }
}
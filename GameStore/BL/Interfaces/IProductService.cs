using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.ProductModels;
using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface IProductService
    {
        public Task<Product> CreateNewProductAsync(ProductModel productModel);

        Task<ServiceResult> DeleteProductAsync(int id);

        public Task<Product> UpdateProductAsync(ProductModel productModel);
    }
}
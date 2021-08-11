using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.DAL.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<ProductPlatforms>> GetPopularPlatformsAsync(int platformCount);

        Task<List<Product>> GetProductsBySearchTermAsync(string searchTerm, int limit, int skippedCount);

        Task<Product> FindProductByIdAsync(int productId);

        Task<ServiceResult> DeleteProductAsync(int id);

        Task<Product> UpdateProductAsync(Product newProduct);

        Task<Product> UpdateItemWithModifiedPropsAsync(Product item,
            params Expression<Func<Product, object>>[] modifiedProperties);
    }
}
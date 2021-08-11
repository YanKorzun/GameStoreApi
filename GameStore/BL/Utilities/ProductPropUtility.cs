using System;
using System.Linq.Expressions;
using GameStore.BL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;

namespace GameStore.BL.Utilities
{
    public class ProductPropUtility : IProductPropUtility
    {
        public Expression<Func<Product, object>> GetOrderExpression(ProductParameters parameters)
        {
            return parameters.OrderBy switch
            {
                nameof(Product.Name) => o => o.Name,
                nameof(Product.Genre) => o => o.Genre,
                nameof(Product.TotalRating) => o => o.TotalRating,
                nameof(Product.Count) => o => o.Count,
                nameof(Product.Price) => o => o.Price,
                _ => o => o.Name
            };
        }
    }
}
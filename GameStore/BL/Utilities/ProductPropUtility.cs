using System;
using System.Linq.Expressions;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;

namespace GameStore.BL.Utilities
{
    public static class ProductPropUtility
    {
        public static Expression<Func<Product, object>> GetOrderExpression(ProductParameters parameters)
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

        public static Expression<Func<Product, bool>> GetFilterExpression(ProductParameters parameters)
        {
            Expression<Func<Product, bool>> expression = null;

            if (parameters.Genre is not null)
            {
                expression = o => o.Genre == parameters.Genre;
            }

            if (parameters.AgeRating is not null)
            {
                expression = o => o.AgeRating == parameters.AgeRating;
            }

            if (parameters.Genre is not null && parameters.AgeRating is not null)
            {
                expression = o => o.Genre == parameters.Genre && o.AgeRating == parameters.AgeRating;
            }

            return expression;
        }
    }
}
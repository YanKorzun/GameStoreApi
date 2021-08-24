using System;
using System.Linq.Expressions;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Parameters;

namespace GameStore.BL.Utilities
{
    public static class ProductPropUtility
    {
        public static Expression<Func<Product, object>> GetOrderExpression(ProductParametersDto parametersDto)
        {
            return parametersDto.OrderBy switch
            {
                nameof(Product.Name) => o => o.Name,
                nameof(Product.Genre) => o => o.Genre,
                nameof(Product.TotalRating) => o => o.TotalRating,
                nameof(Product.Count) => o => o.Count,
                nameof(Product.Price) => o => o.Price,
                _ => o => o.Name
            };
        }

        public static Expression<Func<Product, bool>> GetFilterExpression(ProductParametersDto parametersDto)
        {
            Expression<Func<Product, bool>> expression = null;

            if (parametersDto.Genre is not null)
            {
                expression = o => o.Genre == parametersDto.Genre;
            }

            if (parametersDto.AgeRating is not null)
            {
                expression = o => o.AgeRating == parametersDto.AgeRating;
            }

            if (parametersDto.Genre is not null && parametersDto.AgeRating is not null)
            {
                expression = o => o.Genre == parametersDto.Genre && o.AgeRating == parametersDto.AgeRating;
            }

            return expression;
        }
    }
}
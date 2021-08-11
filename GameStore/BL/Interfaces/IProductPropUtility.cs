using System;
using System.Linq.Expressions;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;

namespace GameStore.BL.Interfaces
{
    public interface IProductPropUtility
    {
        Expression<Func<Product, object>> GetOrderExpression(ProductParameters parameters);
    }
}
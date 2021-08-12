using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.DAL.Enums;

namespace GameStore.DAL.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression);

        Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression,
            params Expression<Func<T, object>>[] includes);

        Task<T> CreateItemAsync(T entity);

        Task<List<T>> CreateItemsAsync(IEnumerable<T> items);

        Task<List<T>> SearchForMultipleItemsAsync<TK>
        (
            Expression<Func<T, bool>> expression,
            Expression<Func<T, TK>> sort,
            OrderType orderType = OrderType.Asc
        );

        Task<List<T>> SearchForMultipleItemsAsync<TK>
        (
            Expression<Func<T, bool>> expression,
            int offset,
            int limit,
            Expression<Func<T, TK>> sort,
            OrderType orderType
        );

        Task<T> UpdateItemAsync(T item, params Expression<Func<T, object>>[] unmodifiedProperties);
    }
}
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.DAL.Entities;

namespace GameStore.DAL.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<List<Order>> GetOrdersAsync(Expression<Func<Order, bool>> expression);

        Task SoftRangeRemoveAsync(ICollection<Order> orders);

        Task SoftOrderRemoveAsync(int id);

        Task<List<Order>> UpdateItemsAsync(IEnumerable<Order> items);

        //Task<List<Order>> UpdateItemsAsync(IEnumerable<Order> items);
    }
}
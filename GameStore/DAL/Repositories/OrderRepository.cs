using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<List<Order>> GetOrdersAsync(Expression<Func<Order, bool>> expression) =>
            await Entity.AsNoTracking().Where(expression).ToListAsync();

        public async Task RemoveOrderRange(ICollection<Order> orders)
        {
            foreach (var order in orders)
            {
                await RemoveOrderAsync(order.Id);
            }
        }

        public async Task RemoveOrderAsync(int id)
        {
            var dbProduct = new Order
            {
                Id = id,
                IsDeleted = true
            };

            DbContext.Entry(dbProduct).Property(x => x.IsDeleted).IsModified = true;

            await DbContext.SaveChangesAsync();
        }
    }
}
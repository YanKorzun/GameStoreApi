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

        public async Task SoftRangeRemoveAsync(ICollection<Order> orders)
        {
            orders.ToList().ForEach(o =>
            {
                o.IsDeleted = true;
                DbContext.Entry(o).Property(x => x.IsDeleted).IsModified = true;
            });

            await DbContext.SaveChangesAsync();
        }

        public async Task SoftOrderRemoveAsync(int id)
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
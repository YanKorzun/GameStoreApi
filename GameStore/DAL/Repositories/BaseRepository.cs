using GameStore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly DbSet<T> Entity;

        protected BaseRepository(ApplicationDbContext databaseContext)
        {
            DbContext = databaseContext;
            Entity = this.DbContext.Set<T>();
        }

        public async Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression)
        {
            var item = await Entity.AsNoTracking().SingleOrDefaultAsync(expression);

            return item;
        }

        public async Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                var query = Entity.Where(expression).AsNoTracking();

                if (includes.Length != 0)
                {
                    query = includes
                        .Aggregate(query,
                            (
                                current, includeProperty) => current.Include(includeProperty)
                        );
                }

                var item = await query.SingleOrDefaultAsync();

                return item;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException($"More then one item has been found. Error: {ex.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Unable to find item in database. Error: {e.Message}");
            }
        }

        public virtual async Task<T> CreateItemAsync(T entity)
        {
            var createdEntity = await DbContext.AddAsync(entity);

            await DbContext.SaveChangesAsync();

            createdEntity.State = EntityState.Detached;

            return createdEntity.Entity;
        }

        public async Task<T> UpdateItemAsync(T item, params Expression<Func<T, object>>[] unmodifiedProperties)
        {
            try
            {
                Entity.Update(item);
                foreach (var property in unmodifiedProperties)
                {
                    DbContext.Entry(item).Property(property).IsModified = false;
                }

                await DbContext.SaveChangesAsync();

                DbContext.Entry(item).State = EntityState.Detached;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Unable to update item. Error: {e.Message}");
            }

            return item;
        }
    }
}
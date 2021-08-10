using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly DbSet<T> Entity;

        protected BaseRepository(ApplicationDbContext databaseContext)
        {
            DbContext = databaseContext;
            Entity = DbContext.Set<T>();
        }

        public async Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression)
        {
            var item = await Entity.AsNoTracking().SingleOrDefaultAsync(expression);

            return item;
        }

        public async Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression,
            params Expression<Func<T, object>>[] includes)
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
                throw new($"Unable to find item in database. Error: {e.Message}");
            }
        }

        public virtual async Task<T> CreateItemAsync(T entity)
        {
            var createdEntity = await DbContext.AddAsync(entity);

            await DbContext.SaveChangesAsync();

            createdEntity.State = EntityState.Detached;

            return createdEntity.Entity;
        }

        public IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString)
        {
            if (!entities.Any())
            {
                return entities;
            }

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return entities;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    continue;
                }

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi =>
                    pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                {
                    continue;
                }

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return entities.OrderBy(orderQuery);
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
                throw new($"Unable to update item. Error: {e.Message}");
            }

            return item;
        }

        public async Task<List<T>> UpdateItemsAsync(IEnumerable<T> items)
        {
            var entitiesList = items.ToList();

            try
            {
                DbContext.UpdateRange(entitiesList);

                await DbContext.SaveChangesAsync();

                foreach (var entity in entitiesList)
                {
                    DbContext.Entry(entity).State = EntityState.Detached;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new($"Unable to update items. Error: {e.Message}");
            }

            return entitiesList;
        }

        public async Task<T> UpdateItemWithModifiedPropsAsync(T item,
            params Expression<Func<T, object>>[] modifiedProperties)
        {
            try
            {
                Entity.Update(item);
                foreach (var property in modifiedProperties)
                {
                    DbContext.Entry(item).Property(property).IsModified = true;
                }

                await DbContext.SaveChangesAsync();

                DbContext.Entry(item).State = EntityState.Detached;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new($"Unable to update item. Error: {e.Message}");
            }

            return item;
        }
    }
}
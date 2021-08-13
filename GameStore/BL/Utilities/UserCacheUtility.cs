using System;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using Microsoft.Extensions.Caching.Memory;

namespace GameStore.BL.Utilities
{
    public class CacheService<T> : ICacheService<T>
    {
        private readonly IMemoryCache _memory;

        public CacheService(IMemoryCache memory)
        {
            _memory = memory;
        }

        public ServiceResult<T> GetEntity(int id)
        {
            if (_memory.TryGetValue(id, out T entity))
            {
                return new(ServiceResultType.Success, entity);
            }

            return new(ServiceResultType.NotFound);
        }

        public void Set(int id, T entity) =>
            _memory.Set(id, entity, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

        public ServiceResult Remove(int id)
        {
            try
            {
                _memory.Remove(id);
                return new(ServiceResultType.Success);
            }
            catch (Exception)
            {
                return new(ServiceResultType.NotFound);
            }
        }
    }
}
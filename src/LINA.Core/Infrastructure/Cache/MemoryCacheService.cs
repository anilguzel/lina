using System;
using System.Collections.Generic;
using System.Text;
using LINA.Core.Infrastructure.Cache.Abstraction;
using Microsoft.Extensions.Caching.Memory;

namespace LINA.Core.Infrastructure.Cache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            _cache.TryGetValue(key, out var value);
            return (T)value;
        }

        public object Set(string key, object value, DateTimeOffset? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
        {
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(absoluteExpiration ?? DateTimeOffset.MaxValue)
                .SetSlidingExpiration(slidingExpiration ?? TimeSpan.MaxValue)
                .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(key, value, options);
            return value;
        }

        public List<T> GetList<T>(string key)
            where T : class
        {
            _cache.TryGetValue(key, out List<T> value);
            return value;
        }

        public void Clear(string key)
        {
            _cache.Remove(key);
        }
    }
}

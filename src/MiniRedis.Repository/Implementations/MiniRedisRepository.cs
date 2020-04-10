namespace MiniRedis.Repository.Implementations
{
    using System;
    using System.Collections.Concurrent;
    using MiniRedis.Repository.Models;

    public class MiniRedisRepository : ICacheRepository<CacheItemModel>
    {
        private readonly ConcurrentDictionary<string, CacheItemModel> cache;

        public MiniRedisRepository()
        {
            this.cache = new ConcurrentDictionary<string, CacheItemModel>();
        }

        public bool Delete(string key)
        {
            return this.cache.TryRemove(key, out var _);
        }

        public CacheItemModel Get(string key)
        {
            this.cache.TryGetValue(key, out var item);

            return item;
        }

        public int Count()
        {
            return this.cache.Count;
        }

        public void AddOrUpdate(string key, CacheItemModel value)
        {
            _ = this.cache.AddOrUpdate(
                key,
                (_) => value,
                (_, __) => value);
        }
    }
}

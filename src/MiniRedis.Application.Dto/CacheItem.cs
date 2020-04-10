using System;

namespace MiniRedis.Application.Dto
{
    public class CacheItem
    {
        public Guid Id { get; set; }

        public DateTime ExpiresAt { get; set; }

        public object Value { get; set; }
    }
}

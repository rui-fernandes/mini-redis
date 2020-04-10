namespace MiniRedis.Repository.Models
{
    using System;

    public class CacheItemModel
    {
        public DateTime ExpiresAt { get; set; }

        public string Value { get; set; }
    }
}

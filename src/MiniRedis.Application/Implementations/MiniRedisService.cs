namespace MiniRedis.Application.Implementations
{
    using System;
    using System.Linq;
    using MiniRedis.Repository;
    using MiniRedis.Repository.Models;

    public class MiniRedisService : ICacheService
    {
        private const int DefaultSeconds = 3600;
        private const char MembersSeparator = '.';
        private const char ScoreMemberSeparator = '-';
        private readonly ICacheRepository<CacheItemModel> repository;

        public MiniRedisService(ICacheRepository<CacheItemModel> repository)
        {
            this.repository = repository;
        }

        public bool Delete(string key)
        {
            return this.repository.Delete(key);
        }

        public string Get(string key)
        {
            var model = this.repository.Get(key);

            if (model == null)
            {
                return null;
            }

            if (model.ExpiresAt < DateTime.UtcNow)
            {
                this.repository.Delete(key);
                return null;
            }

            return model.Value;
        }

        public int GetSize()
        {
            return this.repository.Count();
        }

        public void Increment(string key)
        {
            var item = this.repository.Get(key);

            if (item == null)
            {
                return;
            }

            if (int.TryParse(item.Value, out int result))
            {
                item.Value = $"{result + 1}";
            }

            this.repository.AddOrUpdate(key, item);
        }

        public void Set(string key, int? seconds, string value)
        {
            var model = new CacheItemModel
            {
                ExpiresAt = DateTime.UtcNow.AddSeconds(seconds ?? DefaultSeconds),
                Value = value
            };

            this.repository.AddOrUpdate(key, model);
        }

        public void SortedSet(string key, int score, string member)
        {
            var model = this.repository.Get(key) ?? new CacheItemModel();

            model.ExpiresAt = DateTime.UtcNow.AddSeconds(DefaultSeconds);

            if (string.IsNullOrWhiteSpace(model.Value))
            {
                model.Value = $"{score}{ScoreMemberSeparator}{member}";
            }
            else
            {
                model.Value = $"{model.Value}{MembersSeparator}{score}{ScoreMemberSeparator}{member}";
            }

            this.repository.AddOrUpdate(key, model);
        }

        public int SortedCardinality(string key)
        {
            var model = this.repository.Get(key);

            if (model == null)
            {
                return 0;
            }

            return model.Value.Split(MembersSeparator).Length;
        }

        public int RankMember(string key, string member)
        {
            var model = this.repository.Get(key);

            if (model == null)
            {
                return -1;
            }

            var members = model.Value.Split(MembersSeparator);

            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].Split(ScoreMemberSeparator).Last() == member)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}

namespace MiniRedis.Application
{
    public interface ICacheService
    {
        void Set(string key, int? seconds, string value);

        string Get(string key);

        bool Delete(string key);

        int GetSize();

        void Increment(string key);

        void SortedSet(string key, int score, string member);

        int SortedCardinality(string key);

        int RankMember(string key, string member);
    }
}

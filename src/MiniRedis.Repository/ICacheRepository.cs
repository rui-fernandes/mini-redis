namespace MiniRedis.Repository
{
    public interface ICacheRepository<T>
    {
        void AddOrUpdate(string key, T value);

        T Get(string key);

        bool Delete(string key);

        int Count();
    }
}

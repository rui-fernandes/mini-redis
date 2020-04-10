namespace MiniRedis.Repository.Setup
{
    using Microsoft.Extensions.DependencyInjection;
    using MiniRedis.Repository.Implementations;
    using MiniRedis.Repository.Models;

    public static class RepositorySetup
    {
        public static IServiceCollection SetupRepositories(
            this IServiceCollection services)
        {
            services
                .AddSingleton<ICacheRepository<CacheItemModel>, MiniRedisRepository>();

            return services;
        }
    }
}

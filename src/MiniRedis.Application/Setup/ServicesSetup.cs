namespace MiniRedis.Application.Setup
{
    using Microsoft.Extensions.DependencyInjection;
    using MiniRedis.Application.Implementations;

    public static class ServicesSetup
    {
        public static IServiceCollection SetupServices(
            this IServiceCollection services)
        {
            services
                .AddSingleton<ICacheService, MiniRedisService>();

            return services;
        }
    }
}

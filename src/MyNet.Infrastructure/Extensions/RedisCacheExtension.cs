using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Application.Common.Cache;
using MyNet.Application.Common.Options;
using MyNet.Infrastructure.Services;

namespace MyNet.Infrastructure.Extensions
{
    public static class RedisCacheExtension
    {
        public static IServiceCollection AddRedisCacheExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<CacheOptions>(service =>
            {
                var configuration = service.GetRequiredService<IConfiguration>();
                var cacheOptions = new CacheOptions();
                configuration.GetSection("Redis").Bind(cacheOptions);
                return cacheOptions;
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Host"];
            });

            services.AddSingleton<ICache, RedisCacheService>();

            return services;
        }
    }
}

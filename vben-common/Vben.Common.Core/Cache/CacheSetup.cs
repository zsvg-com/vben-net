using Microsoft.Extensions.DependencyInjection.Extensions;
using NewLife.Caching;
using NewLife.Caching.Services;

namespace Vben.Common.Core.Cache;

public static class CacheSetup
{
    /// <summary>
    /// 缓存注册（新生命Redis组件）
    /// </summary>
    /// <param name="services"></param>
    public static void AddCache(this IServiceCollection services)
    {
        // var cacheOptions = App.GetConfig<CacheOptions>("Cache", true);
        var cacheOptions = MyApp.GetOptions<CacheOptions>("Cache");
        if (cacheOptions.CacheType.ToString() == CacheTypeEnum.Redis.ToString())
        {
            var redis = new FullRedis(new RedisOptions
            {
                Configuration = cacheOptions.Redis.Configuration,
                Prefix = cacheOptions.Redis.Prefix
            })
            {
                // 自动检测集群节点
                // AutoDetect = App.GetConfig<bool>("Cache:Redis:AutoDetect", true)
                AutoDetect = MyApp.GetOptions<bool>("Cache:Redis:AutoDetect")
            };
            // 最大消息大小
            if (cacheOptions.Redis.MaxMessageSize > 0)
                redis.MaxMessageSize = cacheOptions.Redis.MaxMessageSize;
            // 注入 Redis 缓存提供者
            services.AddSingleton<ICacheProvider>(u => new RedisCacheProvider(u) { Cache = redis });
            services.AddSingleton<Redis>(redis);
        }

        // 内存缓存兜底。在没有配置Redis时，使用内存缓存，逻辑代码无需修改
        services.TryAddSingleton<ICacheProvider, CacheProvider>();
    }
}
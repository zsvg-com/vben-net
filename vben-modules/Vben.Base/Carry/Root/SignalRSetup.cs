// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Furion.Logging.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using StackExchange.Redis;
using Vben.Common.Core.Cache;

namespace Admin.NET.Core;

public static class SignalRSetup
{
    /// <summary>
    /// 即时消息SignalR注册
    /// </summary>
    /// <param name="services"></param>
    /// <param name="SetNewtonsoftJsonSetting"></param>
    public static void AddSignalR(this IServiceCollection services, Action<JsonSerializerSettings> SetNewtonsoftJsonSetting)
    {
        var signalRBuilder = services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
            options.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
            options.KeepAliveInterval = TimeSpan.FromMinutes(1);
            options.MaximumReceiveMessageSize = 1024 * 1024 * 10; // 数据包大小10M，默认最大为32K
        }).AddNewtonsoftJsonProtocol(options => SetNewtonsoftJsonSetting(options.PayloadSerializerSettings));

        // 若未启用Redis缓存，直接返回
        var cacheOptions = App.GetConfig<CacheOptions>("Cache", true);
        if (cacheOptions.CacheType != CacheTypeEnum.Redis.ToString())
            return;

        // 若已开启集群配置，则把SignalR配置为支持集群模式
        var clusterOpt = App.GetConfig<ClusterOptions>("Cluster", true);
        if (!clusterOpt.Enabled)
            return;

        var redisOptions = clusterOpt.SentinelConfig;
        ConnectionMultiplexer connection1;
        if (clusterOpt.IsSentinel) // 哨兵模式
        {
            var redisConfig = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                ServiceName = redisOptions.ServiceName,
                AllowAdmin = true,
                DefaultDatabase = redisOptions.DefaultDb,
                Password = redisOptions.Password
            };
            redisOptions.EndPoints.ForEach(u => redisConfig.EndPoints.Add(u));
            connection1 = ConnectionMultiplexer.Connect(redisConfig);
        }
        else
        {
            connection1 = ConnectionMultiplexer.Connect(clusterOpt.SignalR.RedisConfiguration);
        }
        // 密钥存储（数据保护）
        services.AddDataProtection().PersistKeysToStackExchangeRedis(connection1, clusterOpt.DataProtecteKey);

        signalRBuilder.AddStackExchangeRedis(options =>
        {
            // 此处设置的ChannelPrefix并不会生效，如果两个不同的项目，且[程序集名+类名]一样，使用同一个redis服务，请注意修改 Hub/OnlineUserHub 的类名。
            // 原因请参考下边链接：
            // https://github.com/dotnet/aspnetcore/blob/f9121bc3e976ec40a959818451d126d5126ce868/src/SignalR/server/StackExchangeRedis/src/RedisHubLifetimeManager.cs#L74
            // https://github.com/dotnet/aspnetcore/blob/f9121bc3e976ec40a959818451d126d5126ce868/src/SignalR/server/StackExchangeRedis/src/Internal/RedisChannels.cs#L33
            options.Configuration.ChannelPrefix = new RedisChannel(clusterOpt.SignalR.ChannelPrefix, RedisChannel.PatternMode.Auto);
            options.ConnectionFactory = async writer =>
            {
                ConnectionMultiplexer connection;
                if (clusterOpt.IsSentinel)
                {
                    var config = new ConfigurationOptions
                    {
                        AbortOnConnectFail = false,
                        ServiceName = redisOptions.ServiceName,
                        AllowAdmin = true,
                        DefaultDatabase = redisOptions.DefaultDb,
                        Password = redisOptions.Password
                    };
                    redisOptions.EndPoints.ForEach(u => config.EndPoints.Add(u));
                    connection = await ConnectionMultiplexer.ConnectAsync(config, writer);
                }
                else
                {
                    connection = await ConnectionMultiplexer.ConnectAsync(clusterOpt.SignalR.RedisConfiguration);
                }

                connection.ConnectionFailed += (_, e) =>
                {
                    "连接 Redis 失败".LogError();
                };

                if (!connection.IsConnected)
                {
                    "无法连接 Redis".LogError();
                }
                return connection;
            };
        });
    }
}
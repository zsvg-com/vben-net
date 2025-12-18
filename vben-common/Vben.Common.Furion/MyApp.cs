using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Vben.Common.Core;

public static class MyApp
{

    /// <summary>
    /// 服务提供器
    /// </summary>
    public static IServiceProvider ServiceProvider;
    
    /// <summary>
    /// 获取Web主机环境
    /// </summary>
    public static IWebHostEnvironment WebHostEnvironment;

    /// <summary>
    /// 获取全局配置
    /// </summary>
    public static IConfiguration Configuration;

    /// <summary>
    /// 获取请求上下文
    /// </summary>
    public static HttpContext HttpContext =>
        CatchOrDefault(() => ServiceProvider?.GetService<IHttpContextAccessor>()?.HttpContext);

    /// <summary>
    /// 获取请求上下文用户
    /// </summary>
    public static ClaimsPrincipal User => HttpContext?.User;

    /// <summary>
    /// 获取用户名
    /// </summary>
    public static string UserName => User?.Identity?.Name;


    /// <summary>
    /// 获取请求生命周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static TService GetService<TService>()
        where TService : class
    {
        return GetService(typeof(TService)) as TService;
    }

    /// <summary>
    /// 获取请求生命周期的服务
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object GetService(Type type)
    {
        return ServiceProvider.GetService(type);
    }
    
    public static TService GetHttpService<TService>()
        where TService : class
    {
        return GetHttpService(typeof(TService)) as TService;
    }
    
    public static object GetHttpService(Type type)
    {
        return HttpContext.RequestServices.GetService(type);
    }
    
    /// <summary>
    /// 获取请求生命周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static TService GetRequiredService<TService>()
        where TService : class
    {
        return GetRequiredService(typeof(TService)) as TService;
    }

    /// <summary>
    /// 获取请求生命周期的服务
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object GetRequiredService(Type type)
    {
        return ServiceProvider.GetRequiredService(type);
    }
    
    public static T GetOptions<T>()  
    {
   
        return Configuration.GetSection(typeof(T).Name).Get<T>();
    }
    
    /// <summary>
    /// 获取配置节点并转换成指定类型
    /// </summary>
    /// <typeparam name="T">节点类型</typeparam>
    /// <param name="key">节点路径</param>
    /// <returns>节点类型实例</returns>
    public static T GetOptions<T>(string key)  
    {
        return Configuration.GetSection(key).Get<T>();
    }

    /// <summary>
    /// 处理获取对象异常问题
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="action">获取对象委托</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>T</returns>
    private static T CatchOrDefault<T>(Func<T> action, T defaultValue = null)
        where T : class
    {
        try
        {
            return action();
        }
        catch
        {
            return defaultValue ?? null;
        }
    }

}
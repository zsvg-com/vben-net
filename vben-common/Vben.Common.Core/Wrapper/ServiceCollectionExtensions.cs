namespace Vben.Common.Core.Wrapper;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加全局包装器（中间件方案）
    /// </summary>
    public static IServiceCollection AddGlobalWrapperMiddleware(
        this IServiceCollection services, 
        Action<GlobalWrapperOptions> configure = null)
    {
        var options = new GlobalWrapperOptions();
        configure?.Invoke(options);
            
        services.AddSingleton(Options.Create(options));
        return services;
    }
    
    /// <summary>
    /// 添加模型验证过滤器（配合使用）
    /// </summary>
    public static IServiceCollection AddModelValidationFilter(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
            
        services.AddScoped<ModelValidationFilter>();
        services.Configure<MvcOptions>(options =>
        {
            options.Filters.AddService<ModelValidationFilter>(order: int.MaxValue - 20);
        });
            
        return services;
    }
        
   
}
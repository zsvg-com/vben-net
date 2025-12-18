// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Configuration;
//
// namespace Vben.Common.Core;
//
// public class MyApp2
// {
//     public static HttpContext HttpContext => App.HttpContext;
//
//     public static TService GetService<TService>() where TService : class
//     {
//         return App.GetService<TService>();
//     }
//     
//     public static TOptions GetOptions<TOptions>(IServiceProvider serviceProvider = default)
//         where TOptions : class, new()
//     {
//         return App.GetOptions<TOptions>(serviceProvider);
//     }
//
//     public static T GetOptions<T>(string key)
//     {
//         return App.Configuration.GetSection(key).Get<T>();
//     }
//
//     public static IConfiguration Configuration;
//
//     public static IServiceProvider ServiceProvider;
//
//     public static IWebHostEnvironment WebHostEnvironment;
// }
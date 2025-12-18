// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Hosting;
//
// namespace Vben.Core;
//
// public class Startup : AppStartup
// {
//     public void ConfigureServices(IServiceCollection services)
//     {
//         // services.AddSqlSugar(new ConnectionConfig
//         // {
//         //     //mysql的配置，开启mysql时 ConfigureExternalServices这一段也要放开注释
//         //     ConnectionString =
//         //         "Data Source=localhost;Database=vben-net;User ID=root;Password=123456;pooling=true;port=3306;sslmode=none;CharSet=utf8;", //连接符字串
//         //     DbType = DbType.MySql,
//         //     ConfigureExternalServices = new ConfigureExternalServices
//         //     {
//         //         EntityService = (c, p) =>
//         //         {
//         //             if (p.DataType == "varchar(max)") 
//         //             {
//         //                 p.DataType = "longtext";
//         //             }
//         //         }
//         //     },
//         //
//         //
//         //     //oracle的配置
//         //     //ConnectionString = "Data Source=localhost/orcl;User ID=vben;Password=123456;",//连接符字串
//         //     //DbType = DbType.Oracle,
//         //
//         //
//         //     // sqlserver的配置
//         //     // ConnectionString = "server=.;uid=sa;pwd=123456;database=vben", //连接符字串
//         //     // DbType = DbType.SqlServer,
//         //
//         //
//         //     IsAutoCloseConnection = true,
//         // }, db =>
//         // {
//         //     //处理日志事务
//         //     db.Aop.OnLogExecuting = (sql, pars) =>
//         //     {
//         //         if (sql.StartsWith("SELECT"))
//         //         {
//         //             Console.ForegroundColor = ConsoleColor.Green;
//         //         }
//         //
//         //         if (sql.StartsWith("UPDATE") || sql.StartsWith("INSERT"))
//         //         {
//         //             Console.ForegroundColor = ConsoleColor.Blue;
//         //         }
//         //
//         //         if (sql.StartsWith("DELETE"))
//         //         {
//         //             Console.ForegroundColor = ConsoleColor.Magenta;
//         //         }
//         //
//         //         Console.WriteLine(sql);
//         //         Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));
//         //         Console.WriteLine();
//         //         App.PrintToMiniProfiler("SqlSugar", "Info",
//         //             sql + "\r\n" +
//         //             db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
//         //     };
//         // });
//         //
//         //
//         // 注册EventBus服务
//         // services.AddEventBus(builder =>
//         // {
//         //     // 注册 Log 日志订阅者
//         //     builder.AddSubscriber<LogEventSubscriber>();
//         // });
//     }
//
//     public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//     {
//         if (env.IsDevelopment())
//         {
//         }
//     }
// }
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Serilog;
using Vben.Admin.Launch.Init;
using Vben.Base.Auth.Root;
using Vben.Base.Mon.Job.Main;
using Vben.Base.Mon.Log.Root;
using Vben.Common.Core.Attribute;
using Vben.Common.Core.Cache;
using Vben.Common.Core.Filter;
using Vben.Common.Core.Token;
using Vben.Common.Sqlsugar.Config;

namespace Vben.Admin.Launch;

public class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version;
        Console.WriteLine("Application Version: "+version.Major + "." + version.Minor + "." + version.Build);
        Console.WriteLine(@"
8b           d8 88                                    88888888ba,                                              
`8b         d8' 88                                    88      `""8b                                             
 `8b       d8'  88                                    88        `8b                                            
  `8b     d8'   88,dPPYba,   ,adPPYba, 8b,dPPYba,     88         88  ,adPPYba, 88,dPYba,,adPYba,   ,adPPYba,   
   `8b   d8'    88P'    ""8a a8P_____88 88P'   `""8a    88         88 a8P_____88 88P'   ""88""    ""8a a8""     ""8a  
    `8b d8'     88       d8 8PP"""""""""""""" 88       88    88         8P 8PP"""""""""""""" 88      88      88 8b       d8  
     `888'      88b,   ,a8"" ""8b,   ,aa 88       88    88      .a8P  ""8b,   ,aa 88      88      88 ""8a,   ,a8""  
      `8'       8Y""Ybbd8""'   `""Ybbd8""' 88       88    88888888Y""'    `""Ybbd8""' 88      88      88  `""YbbdP""'   
");
        // 统一配置项目选项注册
        // services.AddProjectOptions();
        services.AddHttpRemote();
        // services.AddJwt<AuthzHandler>(enableGlobalAuthorize: true);
        
        // JWT
        // services.AddJwt<AuthzHandler>(enableGlobalAuthorize: true, jwtBearerConfigure: options =>
        // {
        //     // 实现 JWT 身份验证过程控制
        //     options.Events = new JwtBearerEvents
        //     {
        //         OnMessageReceived = context =>
        //         {
        //             var httpContext = context.HttpContext;
        //             // 若请求 Url 包含 token 参数，则设置 Token 值
        //             if (httpContext.Request.Query.ContainsKey("token"))
        //                 context.Token = httpContext.Request.Query["token"];
        //             // if (httpContext.Request.Query.ContainsKey("Authorization"))
        //             // {
        //             //     context.Token = httpContext.Request.Query["Authorization"];
        //             //     context.Token=context.Token.Substring(7);
        //             // }
        //             return Task.CompletedTask;
        //         }
        //     };
        // });
        //     .AddSignatureAuthentication(options =>  // 添加 Signature 身份验证
        // {
        //     options.Events = SysOpenAccessService.GetSignatureAuthenticationEventImpl();
        // });
        
        // 缓存注册
        services.AddCache();

        services.AddCorsAccessor();
        
        //app服务注册
        services.AddService();
      
        // Json序列化设置
        // static void SetNewtonsoftJsonSetting(JsonSerializerSettings setting)
        // {
        //     setting.DateFormatHandling = DateFormatHandling.IsoDateFormat;
        //     setting.DateTimeZoneHandling = DateTimeZoneHandling.Local;
        //     //setting.Converters.AddDateTimeTypeConverters(localized: false); // 时间本地化
        //     setting.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 时间格式化
        //     setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 忽略循环引用
        //     // setting.ContractResolver = new CamelCasePropertyNamesContractResolver(); // 解决动态对象属性名大写
        //     // setting.NullValueHandling = NullValueHandling.Ignore; // 忽略空值
        //     setting.Converters.AddLongTypeConverters(); // long转string（防止js精度溢出） 超过17位开启
        //     // setting.MetadataPropertyHandling = MetadataPropertyHandling.Ignore; // 解决DateTimeOffset异常
        //     // setting.DateParseHandling = DateParseHandling.None; // 解决DateTimeOffset异常
        //     // setting.Converters.Add(new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }); // 解决DateTimeOffset异常
        // }
        
        // Json序列化设置
        static void SetNewtonsoftJsonSetting(JsonSerializerSettings setting)
        {
            setting.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            setting.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            //setting.Converters.AddDateTimeTypeConverters(localized: false); // 时间本地化
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 时间格式化
            setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 忽略循环引用
            // setting.ContractResolver = new CamelCasePropertyNamesContractResolver(); // 解决动态对象属性名大写
            // setting.NullValueHandling = NullValueHandling.Ignore; // 忽略空值
            setting.Converters.AddLongTypeConverters(); // long转string（防止js精度溢出） 超过17位开启
            // setting.MetadataPropertyHandling = MetadataPropertyHandling.Ignore; // 解决DateTimeOffset异常
            // setting.DateParseHandling = DateParseHandling.None; // 解决DateTimeOffset异常
            // setting.Converters.Add(new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }); // 解决DateTimeOffset异常
        }
        
        
        // 即时通讯
        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
            options.KeepAliveInterval = TimeSpan.FromSeconds(15); // 服务器端向客户端ping的间隔
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(30); // 客户端向服务器端ping的间隔
            options.MaximumReceiveMessageSize = 1024 * 1014 * 10; // 数据包大小10M，默认最大为32K
        }).AddNewtonsoftJsonProtocol(options => SetNewtonsoftJsonSetting(options.PayloadSerializerSettings));
            
        
        services.AddControllers()
            .AddMvcFilter<RequestActionFilter>()
            .AddMvcFilter<TransactionalFilter>()
            .AddInjectWithUnifyResult<RestResultProvider>()
            // .AddNewtonsoftJson(options => SetNewtonsoftJsonSetting(options.SerializerSettings))
            .AddJsonOptions(options =>
            {
                //options.JsonSerializerOptions.DefaultBufferSize = 10_0000;//返回较大数据数据序列化时会截断，原因：默认缓冲区大小（以字节为单位）为16384。
                options.JsonSerializerOptions.Converters.AddDateTimeTypeConverters("yyyy-MM-dd HH:mm:ss");
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; 
                //long转换
                options.JsonSerializerOptions.Converters.AddLongTypeConverters();
            });
    

        // 注册日志事件订阅者(支持自定义消息队列组件)
        services.AddEventBus(builder =>
        {
            builder.AddSubscriber<LogEventSubscriber>();
        });

        // services.AddEventBus(builder =>
        // {
        //     builder.AddSubscriber<SiEkpTodoSubscriber>();
        // });
        
        
        services.AddSchedule(options =>
        {
            // 添加作业执行监视器
            options.AddMonitor<MonJobMonitor>();
        });
        //
        // services.AddSchedule(options =>
        // {
        //     // 注册作业，并配置作业触发器
        //     options.AddJob<MyJob>(Triggers.Secondly()); // 表示每秒执行
        // });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        YitIdHelper.SetIdGenerator(new IdGeneratorOptions
        {
            WorkerId = 1, // 工作节点ID
            WorkerIdBitLength = 10, // 默认10，支持最多1024个节点
            SeqBitLength = 12, // 默认12，每毫秒可生成4096个ID
            BaseTime = DateTime.Parse("2020-01-01") // 基准时间
        });

        app.UseStaticFiles();

        app.UseSerilogRequestLogging(); // 请求日志必须在 UseStaticFiles 和 UseRouting 之间
        app.UseRouting();

        app.UseCorsAccessor();

        app.UseAuthentication();
        
        app.UseMiddleware<JwtAuthMiddleware>();

        app.UseAuthorization();

        app.UseInject(string.Empty);

        // app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        App.GetService<DataInit>().Init().GetAwaiter().GetResult();
        App.GetService<MonJobMainService>().StartAllJobs().GetAwaiter().GetResult();
        
        app.UseEndpoints(endpoints =>
        {
            // 注册集线器
            endpoints.MapHubs();

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
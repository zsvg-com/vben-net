using Vben.Base.Mon.Oper.Log;
using Vben.Common.Core.Const;
using Vben.Common.Core.Filter;

namespace Vben.Base.Mon.Log.Root;

/// <summary>
/// 请求日志拦截
/// </summary>
public class RequestActionFilter : IAsyncActionFilter
{
    private readonly IEventPublisher _eventPublisher;

    public RequestActionFilter(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var httpRequest = httpContext.Request;

        var sw = new Stopwatch();
        sw.Start();
        var actionContext = await next();
        sw.Stop();

        // 判断是否请求成功（没有异常就是请求成功）
        var isRequestSucceed = actionContext.Exception == null;
        var headers = httpRequest.Headers;
        var clientInfo = headers.ContainsKey("User-Agent") ? Parser.GetDefault().Parse(headers["User-Agent"]) : null;
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        var ip = httpRequest.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ip))
        {
            ip = httpRequest.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }


        foreach (var metadata in actionDescriptor.EndpointMetadata)
        {
            if (metadata.GetType() == typeof(OplogAttribute))
            {
                //禁用操作日志，直接返回
                OplogAttribute attribute = (OplogAttribute)metadata;
                await _eventPublisher.PublishAsync(new ChannelEventSource("Create:OperLog",
                    new MonOperLog
                    {
                        id = YitIdHelper.NextId() + "",
                        // name = httpContext.User?.FindFirstValue(ClaimConst.CLAINM_NAME),
                        // opmod = attribute.name,
                        opmod = attribute.title,
                        butyp = attribute.type,
                        sutag = isRequestSucceed,
                        opip = ip,
                        // Location = httpRequest.GetRequestUrlAddress(),
                        agbro = clientInfo?.UA.Family + clientInfo?.UA.Major,
                        ageos = clientInfo?.OS.Family + clientInfo?.OS.Major,
                        clazz = context.Controller.ToString(),
                        // MethodName = actionDescriptor?.ActionName,
                        remet = httpRequest.Method,
                        repar = JSON.Serialize(context.ActionArguments.Count < 1 ? "" : context.ActionArguments),
                        // Result =
                        //     actionContext.Result?.GetType() == typeof(JsonResult) ? JSON.Serialize(actionContext.Result) : "",
                        cotim = sw.ElapsedMilliseconds,
                        optim = DateTime.Now,
                        usnam = httpContext.User?.FindFirstValue(ClaimConst.CLAINM_ACCOUNT),
                        opuid = httpContext.User?.FindFirstValue(ClaimConst.CLAINM_USERID),
                        opuna = httpContext.User?.FindFirstValue(ClaimConst.CLAINM_NAME)
                    }));


                return;
            }
        }

        //默认不开启
        //判断是否需有禁用操作日志属性
        // foreach (var metadata in actionDescriptor.EndpointMetadata)
        // {                
        //     if (metadata.GetType() == typeof(DisableOpLogAttribute))
        //     {
        //         //禁用操作日志，直接返回
        //         return;
        //     }
        // }
        // await _eventPublisher.PublishAsync(new ChannelEventSource("Create:OpLog",
        //     new MonLogOp
        //     {
        //         Id = YitIdHelper.NextId()+"",
        //         Name = httpContext.User?.FindFirstValue(ClaimConst.CLAINM_NAME),
        //         Success = isRequestSucceed ? YesOrNot.Y : YesOrNot.N,
        //         Ip = ip,
        //         Location = httpRequest.GetRequestUrlAddress(),
        //         Browser = clientInfo?.UA.Family + clientInfo?.UA.Major,
        //         Os = clientInfo?.OS.Family + clientInfo?.OS.Major,
        //         Url = httpRequest.Path,
        //         ClassName = context.Controller.ToString(),
        //         MethodName = actionDescriptor?.ActionName,
        //         ReqMethod = httpRequest.Method,
        //         Param = JSON.Serialize(context.ActionArguments.Count < 1 ? "" : context.ActionArguments),
        //         Result =
        //             actionContext.Result?.GetType() == typeof(JsonResult) ? JSON.Serialize(actionContext.Result) : "",
        //         ElapsedTime = sw.ElapsedMilliseconds,
        //         OpTime = DateTime.Now,
        //         Account = httpContext.User?.FindFirstValue(ClaimConst.CLAINM_ACCOUNT)
        //     }));
    }
}
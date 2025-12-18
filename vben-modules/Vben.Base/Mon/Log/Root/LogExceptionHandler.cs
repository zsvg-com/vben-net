using Vben.Base.Mon.Log.Error;
using Vben.Common.Core.Const;

namespace Vben.Base.Mon.Log.Root;

/// <summary>
/// 全局异常处理
/// </summary>
public class LogExceptionHandler : IGlobalExceptionHandler, ISingleton
{
    private readonly IEventPublisher _eventPublisher;

    public LogExceptionHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        Console.WriteLine("出错了");
        var userContext = App.User;
        _eventPublisher.PublishAsync(new ChannelEventSource("Create:ErrorLog",
            new MonLogError
            {
                id = YitIdHelper.NextId() + "",
                useid = userContext?.FindFirstValue(ClaimConst.CLAINM_USERID),
                usnam = userContext?.FindFirstValue(ClaimConst.CLAINM_ACCOUNT),
                usena = userContext?.FindFirstValue(ClaimConst.CLAINM_NAME),
                name = "未定义的操作",
                clazz = context.Exception.TargetSite.DeclaringType?.FullName,
                method = context.Exception.TargetSite.Name,
                ExceptionName = context.Exception.Message,
                ExceptionMsg = context.Exception.Message,
                ExceptionSource = context.Exception.Source,
                error = context.Exception.StackTrace,
                param = context.Exception.TargetSite.GetParameters().ToString(),
                crtim = DateTime.Now
            }));
        Serilog.Log.Error(context.Exception.ToString());
        return Task.CompletedTask;
    }
}
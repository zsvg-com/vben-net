using Furion.Schedule;
using Microsoft.Extensions.Logging;
using Vben.Base.Mon.Job.Log;

namespace Vben.Base.Mon.Job.Main;

public class MonJobMonitor : IJobMonitor
{
    private readonly ILogger<MonJobMonitor> _logger;
    
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public MonJobMonitor(
        ILogger<MonJobMonitor> logger,
        IServiceScopeFactory serviceScopeFactory
        )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task OnExecutingAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>().CopyNew();
        MonJobLog log = new MonJobLog();
        log.id=YitIdHelper.NextId().ToString();
        log.msg=context.RunId;
        log.sttim=DateTime.Now;
        log.name=context.JobDetail.Description;
        log.ret="执行中";
        db.Insertable(log).ExecuteCommand();
        _logger.LogInformation("执行之前：{context}", context);
        return Task.CompletedTask;
    }

    public Task OnExecutedAsync(JobExecutedContext context, CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>().CopyNew();
        _logger.LogInformation("执行之后：{context}", context);
        MonJobLog log =db.Queryable<MonJobLog>().First(it=>it.msg==context.RunId);
        log.entim=DateTime.Now;
        log.ret="成功";
        log.msg=context.Result;
        if (context.Exception != null)
        {
            log.ret="失败";   
            log.msg=context.Exception.Message+context.Exception.InnerException;
            _logger.LogError(context.Exception, "执行出错啦：{context}", context);
        }
        db.Updateable(log).ExecuteCommand();
        return Task.CompletedTask;
    }
}
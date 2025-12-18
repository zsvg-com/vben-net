using Vben.Base.Mon.Log.Error;
using Vben.Base.Mon.Log.Login;
using Vben.Base.Mon.Oper.Log;
using Vben.Base.Sys.Org.User;

namespace Vben.Base.Mon.Log.Root;

public class LogEventSubscriber : IEventSubscriber
{
    public LogEventSubscriber(IServiceProvider services)
    {
        Services = services;
    }

    public IServiceProvider Services { get; }

    [EventSubscribe("Create:OperLog")]
    public async Task CreateOperLog(EventHandlerExecutingContext context)
    {
        using var scope = Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<MonOperLog>>();
        var log = (MonOperLog)context.Source.Payload;
        await repository.InsertAsync(log);
    }

    [EventSubscribe("Create:ErrorLog")]
    public async Task CreateErrorLog(EventHandlerExecutingContext context)
    {
        using var scope = Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<MonLogError>>();
        var log = (MonLogError)context.Source.Payload;
        await repository.InsertAsync(log);
    }

    [EventSubscribe("Create:LoginLog")]
    public async Task CreateLoginLog(EventHandlerExecutingContext context)
    {
        using var scope = Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<MonLoginLog>>();
        var log = (MonLoginLog)context.Source.Payload;
        await repository.InsertAsync(log);
    }

    [EventSubscribe("Update:UserLoginInfo")]
    public async Task UpdateUserLoginInfo(EventHandlerExecutingContext context)
    {
        using var scope = Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysOrgUser>>();
        var log = (SysOrgUser)context.Source.Payload;
        await repository.Context.Updateable(log).UpdateColumns(m => new { m.lotim, m.loip })
            .ExecuteCommandAsync();
    }
}
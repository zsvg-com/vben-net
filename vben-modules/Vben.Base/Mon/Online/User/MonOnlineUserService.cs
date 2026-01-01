using Admin.NET.Core;
using Microsoft.AspNetCore.SignalR;

namespace Vben.Base.Mon.Online.User;

[Service]
public class MonOnlineUserService(
    IHubContext<OnlineUserHub, IOnlineUserHub> onlineUserHubContext,
    SqlSugarRepository<MonOnlineUser> sysOnlineUerRep)
{
    
    
    
    public async Task SingleLogin(string userId)
    {
        // if (await _sysConfigService.GetConfigValue<bool>(ConfigConst.SysSingleLogin))
        // {
        //     var users = await _sysOnlineUerRep.GetListAsync(u => u.UserId == userId);
        //     foreach (var user in users)
        //     {
        //         await ForceOffline(user);
        //     }
        // }
        var users = await sysOnlineUerRep.GetListAsync(u => u.useid == userId);
        foreach (var user in users)
        {
            await ForceOffline(user);
        }
    }
    
    /// <summary>
    /// 通过用户ID踢掉在线用户
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task ForceOffline(string userId)
    {
        var users = await sysOnlineUerRep.GetListAsync(u => u.useid == userId);
        foreach (var user in users)
        {
            await ForceOffline(user);
        }
    }
    
    public async Task ForceOffline(MonOnlineUser user)
    {
        await onlineUserHubContext.Clients.Client(user.conid ?? "").ForceOffline("强制下线");
        await sysOnlineUerRep.DeleteAsync(user);
    }
}
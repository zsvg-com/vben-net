// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Admin.NET.Core.Service;
using Furion.InstantMessaging;
using Microsoft.AspNetCore.SignalR;
using NewLife;
using Vben.Base.Mon.Online.User;
using Vben.Common.Core.Const;
using Vben.Common.Core.Token;

namespace Admin.NET.Core;

/// <summary>
/// 在线用户集线器
/// </summary>
[MapHub("/hubs/onlineUser")]
// [MapHub("/resource/sse")]
public class OnlineUserHub : Hub<IOnlineUserHub>
{
    private const string GROUP_ONLINE = "GROUP_ONLINE_"; // 租户分组前缀

    private readonly SqlSugarRepository<MonOnlineUser> _sysOnlineUerRep;
    private readonly SysMessageService _sysMessageService;
    private readonly IHubContext<OnlineUserHub, IOnlineUserHub> _onlineUserHubContext;
    private readonly SysCacheService _sysCacheService;
    // private readonly SysConfigService _sysConfigService;

    public OnlineUserHub(SqlSugarRepository<MonOnlineUser> sysOnlineUerRep,
        SysMessageService sysMessageService,
        IHubContext<OnlineUserHub, IOnlineUserHub> onlineUserHubContext,
        SysCacheService sysCacheService)
        // SysConfigService sysConfigService)
    {
        _sysOnlineUerRep = sysOnlineUerRep;
        _sysMessageService = sysMessageService;
        _onlineUserHubContext = onlineUserHubContext;
        _sysCacheService = sysCacheService;
        // _sysConfigService = sysConfigService;
    }

    /// <summary>
    /// 连接
    /// </summary>
    /// <returns></returns>
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var userxx=LoginHelper.UserId;
        
        var userId = httpContext.User.FindFirst(ClaimConst.CLAINM_USERID)?.Value;
        var account = httpContext.User.FindFirst(ClaimConst.CLAINM_ACCOUNT)?.Value;
        var realName = httpContext.User.FindFirst(ClaimConst.CLAINM_NAME)?.Value;
        var tenantId = (httpContext.User.FindFirst(ClaimConst.TENANT_ID)?.Value).ToLong();
        var clent = Parser.GetDefault().Parse(httpContext.Request.Headers["User-Agent"]);
        var browser = clent.UA.Family + clent.UA.Major;
        var os = clent.OS.Family + clent.OS.Major;
        
        
        if (userId == null || string.IsNullOrWhiteSpace(account)) return;
        var user = new MonOnlineUser
        {
            id = YitIdHelper.NextId(),
            conid = Context.ConnectionId,
            useid = userId,
            usena = account,
            nicna = realName,
            cotim = DateTime.Now,
            ip = httpContext.GetRemoteIpAddressToIPv4(true),
            browser = browser,
            os = os,
            tenid = tenantId,
        };
        await _sysOnlineUerRep.InsertAsync(user);

        // 是否开启单用户登录
        // if (await _sysConfigService.GetConfigValue<bool>(ConfigConst.SysSingleLogin))
        // {
        //     _sysCacheService.HashAddOrUpdate(CacheConst.KeyUserOnline, "" + user.UserId, user);
        // }
        // else  // 非单用户登录则绑定用户连接Id
        // {
        //     _sysCacheService.HashAdd(CacheConst.KeyUserOnline, user.UserId + Context.ConnectionId, user);
        // }
        
        _sysCacheService.HashAdd(CacheConst.KeyUserOnline, user.useid + Context.ConnectionId, user);

        // 以租户Id进行分组
        var groupName = $"{GROUP_ONLINE}{user.tenid}";
        await _onlineUserHubContext.Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        var userList = await _sysOnlineUerRep.AsQueryable().Filter("", true)
            .Where(u => u.tenid == user.tenid).Take(10).ToListAsync();

        await _onlineUserHubContext.Clients.Groups(groupName).OnlineUserList(new OnlineUserList
        {
            RealName = user.nicna,
            Online = true,
            UserList = userList
        });
        
        // if (await _sysConfigService.GetConfigValue<bool>(ConfigConst.SysLoginOutReminder))
        //     await _onlineUserHubContext.Clients.Groups(groupName).OnlineUserList(new OnlineUserList
        //     {
        //         RealName = user.RealName,
        //         Online = true,
        //         UserList = userList
        //     });
    }

    /// <summary>
    /// 断开
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (string.IsNullOrEmpty(Context.ConnectionId)) return;

        var httpContext = Context.GetHttpContext();

        var user = await _sysOnlineUerRep.AsQueryable().Filter("", true).FirstAsync(u => u.conid == Context.ConnectionId);
        if (user == null) return;

        await _sysOnlineUerRep.DeleteByIdAsync(user.id);

        // 是否开启单用户登录
        // if (await _sysConfigService.GetConfigValue<bool>(ConfigConst.SysSingleLogin))
        // {
        //     _sysCacheService.HashDel<SysOnlineUser>(CacheConst.KeyUserOnline, "" + user.UserId);
        //     // _sysCacheService.Remove(CacheConst.KeyUserOnline + user.UserId);
        // }
        // else
        // {
        //     _sysCacheService.HashDel<SysOnlineUser>(CacheConst.KeyUserOnline, user.UserId + Context.ConnectionId);
        //     // _sysCacheService.Remove(CacheConst.KeyUserOnline + user.UserId + Context.ConnectionId);
        // }
        
        _sysCacheService.HashDel<MonOnlineUser>(CacheConst.KeyUserOnline, user.useid + Context.ConnectionId);

        // 通知当前组用户变动
        var userList = await _sysOnlineUerRep.AsQueryable().Filter("", true)
            .Where(u => u.tenid == user.tenid).Take(10).ToListAsync();

        await _onlineUserHubContext.Clients.Groups($"{GROUP_ONLINE}{user.tenid}").OnlineUserList(new OnlineUserList
        {
            RealName = user.nicna,
            Online = false,
            UserList = userList
        });
        
        // if (await _sysConfigService.GetConfigValue<bool>(ConfigConst.SysLoginOutReminder))
        //     await _onlineUserHubContext.Clients.Groups($"{GROUP_ONLINE}{user.TenantId}").OnlineUserList(new OnlineUserList
        //     {
        //         RealName = user.RealName,
        //         Online = false,
        //         UserList = userList
        //     });
    }

    /// <summary>
    /// 强制下线
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task ForceOffline(OnlineUserHubInput input)
    {
        await _onlineUserHubContext.Clients.Client(input.ConnectionId).ForceOffline("强制下线");
    }

    /// <summary>
    /// 发送信息给某个人
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessage(MessageInput message)
    {
        await _sysMessageService.SendUser(message);
    }

    /// <summary>
    /// 发送信息给所有人
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessageToAll(MessageInput message)
    {
        await _sysMessageService.SendAllUser(message);
    }

    /// <summary>
    /// 发送消息给某些人（除了本人）
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessageToOther(MessageInput message)
    {
        await _sysMessageService.SendOtherUser(message);
    }

    /// <summary>
    /// 发送消息给某些人
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessageToUsers(MessageInput message)
    {
        await _sysMessageService.SendUsers(message);
    }
}
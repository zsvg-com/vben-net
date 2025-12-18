using Admin.NET.Core;
using Admin.NET.Core.Service;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.SignalR;
using Vben.Base.Mon.online.user;
using Vben.Base.Sys.Notice;

namespace Vben.Base.Mon.Online.User;

/// <summary>
/// 系统在线用户服务 🧩
/// </summary>
[ApiDescriptionSettings(Order = 300)]
public class MonOnlineUserApi : IDynamicApiController, ITransient
{
    // private readonly UserManager _userManager;
    // private readonly SysConfigService _sysConfigService;
    private readonly IHubContext<OnlineUserHub, IOnlineUserHub> _onlineUserHubContext;
    private readonly SqlSugarRepository<MonOnlineUser> _sysOnlineUerRep;

    public MonOnlineUserApi(
        // SysConfigService sysConfigService,
        IHubContext<OnlineUserHub, IOnlineUserHub> onlineUserHubContext,
        SqlSugarRepository<MonOnlineUser> sysOnlineUerRep
        // UserManager userManager
        )
    {
        // _userManager = userManager;
        // _sysConfigService = sysConfigService;
        _onlineUserHubContext = onlineUserHubContext;
        _sysOnlineUerRep = sysOnlineUerRep;
    }

    /// <summary>
    /// 获取在线用户分页列表 🔖
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取在线用户分页列表")]
    public async Task<dynamic> GetList()
    {  
        // var pp = XreqUtil.GetPp();
        // var items = await _sysOnlineUerRep.AsQueryable()
        //     .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        // return RestPageResult.Build(pp.total.Value, items);
        
        var items = await _sysOnlineUerRep.GetListAsync();
        
        return items;
    }
    
    // /// <summary>
    // /// 获取在线用户分页列表 🔖
    // /// </summary>
    // /// <returns></returns>
    // [DisplayName("获取在线用户分页列表")]
    // public async Task<SqlSugarPagedList<MonOnlineUser>> GetList(int pageNum,int pageSize)
    // {
    //     Console.WriteLine(1111);
    //     return await _sysOnlineUerRep.AsQueryable()
    //         // .WhereIF(_userManager.SuperAdmin && input.TenantId > 0, u => u.TenantId == input.TenantId)
    //         // .WhereIF(!string.IsNullOrWhiteSpace(input.UserName), u => u.usena.Contains(input.UserName))
    //         // .WhereIF(!string.IsNullOrWhiteSpace(input.RealName), u => u.nicna.Contains(input.RealName))
    //         .ToPagedListAsync(pageNum,pageSize);
    // }

    /// <summary>
    /// 强制下线 🔖
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [NonValidation]
    [DisplayName("强制下线")]
    public async Task ForceOffline(MonOnlineUser user)
    {
        await _onlineUserHubContext.Clients.Client(user.conid ?? "").ForceOffline("强制下线");
        await _sysOnlineUerRep.DeleteAsync(user);
    }

    /// <summary>
    /// 发布站内消息
    /// </summary>
    /// <param name="notice"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    [NonAction]
    public async Task PublicNotice(SysNotice notice, List<string> userIds)
    {
        var userList = await _sysOnlineUerRep.GetListAsync(u => userIds.Contains(u.useid));
        if (userList.Count == 0) return;

        foreach (var item in userList)
        {
            await _onlineUserHubContext.Clients.Client(item.conid ?? "").PublicNotice(notice);
        }
    }

    /// <summary>
    /// 单用户登录
    /// </summary>
    /// <returns></returns>
    [NonAction]
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
        var users = await _sysOnlineUerRep.GetListAsync(u => u.useid == userId);
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
    [NonAction]
    public async Task ForceOffline(string userId)
    {
        var users = await _sysOnlineUerRep.GetListAsync(u => u.useid == userId);
        foreach (var user in users)
        {
            await ForceOffline(user);
        }
    }
}
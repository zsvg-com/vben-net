using Admin.NET.Core;
using Microsoft.AspNetCore.SignalR;
using Vben.Base.Sys.Notice;

namespace Vben.Base.Mon.Online.User;

/// <summary>
/// 系统在线用户服务 🧩
/// </summary>
[Route("mon/online/user")]
[ApiDescriptionSettings("Mon", Tag = "在线用户")]
public class MonOnlineUserApi( 
    IHubContext<OnlineUserHub, IOnlineUserHub> onlineUserHubContext,
    SqlSugarRepository<MonOnlineUser> sysOnlineUerRep)
    : ControllerBase
{
    // private readonly UserManager _userManager;
    // private readonly SysConfigService _sysConfigService;

    // UserManager userManager
    // _userManager = userManager;
    // _sysConfigService = sysConfigService;

    /// <summary>
    /// 获取在线用户分页列表 🔖
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取在线用户分页列表")]
    [HttpGet("list")]
    public async Task<dynamic> GetList()
    {  
        // var pp = XreqUtil.GetPp();
        // var items = await _sysOnlineUerRep.AsQueryable()
        //     .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        // return RestPageResult.Build(pp.total.Value, items);
        
        var items = await sysOnlineUerRep.GetListAsync();
        
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
    // [HttpGet("kick")]
    [NonAction]
    public async Task ForceOffline(MonOnlineUser user)
    {
        await onlineUserHubContext.Clients.Client(user.conid ?? "").ForceOffline("强制下线");
        await sysOnlineUerRep.DeleteAsync(user);
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
        var userList = await sysOnlineUerRep.GetListAsync(u => userIds.Contains(u.useid));
        if (userList.Count == 0) return;

        foreach (var item in userList)
        {
            await onlineUserHubContext.Clients.Client(item.conid ?? "").PublicNotice(notice);
        }
    }

    // /// <summary>
    // /// 单用户登录
    // /// </summary>
    // /// <returns></returns>
    // [NonAction]
    // public async Task SingleLogin(string userId)
    // {
    //     // if (await _sysConfigService.GetConfigValue<bool>(ConfigConst.SysSingleLogin))
    //     // {
    //     //     var users = await _sysOnlineUerRep.GetListAsync(u => u.UserId == userId);
    //     //     foreach (var user in users)
    //     //     {
    //     //         await ForceOffline(user);
    //     //     }
    //     // }
    //     var users = await sysOnlineUerRep.GetListAsync(u => u.useid == userId);
    //     foreach (var user in users)
    //     {
    //         await ForceOffline(user);
    //     }
    // }
    //
    // /// <summary>
    // /// 通过用户ID踢掉在线用户
    // /// </summary>
    // /// <param name="userId"></param>
    // /// <returns></returns>
    // [NonAction]
    // public async Task ForceOffline(string userId)
    // {
    //     var users = await sysOnlineUerRep.GetListAsync(u => u.useid == userId);
    //     foreach (var user in users)
    //     {
    //         await ForceOffline(user);
    //     }
    // }
}
using Vben.Common.Core.Attribute;
using Vben.Common.Core.Const;
using Vben.Common.Core.Enum;
using Vben.Common.Core.Filter;

namespace Vben.Common.Core.Manager;

/// <summary>
/// 用户管理
/// </summary>
[Service(ServiceLifetime = LifeTime.Scoped, ServiceType = typeof(IUserManager))]
public class UserManager : IUserManager
{
    // private readonly IHttpContextAccessor _httpContextAccessor;

    public string UserId
    {
        // get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_USERID)?.Value;
        get => MyApp.HttpContext.User.FindFirst(ClaimConst.CLAINM_USERID)?.Value;
    }

    public string Perms
    {
        // get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_PERMS)?.Value;
        get => MyApp.HttpContext.User.FindFirst(ClaimConst.CLAINM_PERMS)?.Value;
    }

    public string Account
    {
        // get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_ACCOUNT)?.Value;
        get => MyApp.HttpContext.User.FindFirst(ClaimConst.CLAINM_ACCOUNT)?.Value;
    }

    public string Name
    {
        // get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_NAME)?.Value;
        get => MyApp.HttpContext.User.FindFirst(ClaimConst.CLAINM_NAME)?.Value;
    }

    public bool SuperAdmin
    {
        // get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_SUPERADMIN)?.Value ==
        //        ((int)AdminType.SuperAdmin).ToString();
        get => MyApp.HttpContext.User.FindFirst(ClaimConst.CLAINM_SUPERADMIN)?.Value ==
               ((int)AdminType.SuperAdmin).ToString();
    }

    public string DeptId
    {
        // get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_DEPTID)?.Value;
        get => MyApp.HttpContext.User.FindFirst(ClaimConst.CLAINM_DEPTID)?.Value;
    }

    public string Type
    {
        // get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_TYPE)?.Value;
        get => MyApp.HttpContext.User.FindFirst(ClaimConst.CLAINM_TYPE)?.Value;
    }

    public string Label
    {
        // get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_LABEL)?.Value;
        get => MyApp.HttpContext.User.FindFirst(ClaimConst.CLAINM_LABEL)?.Value;
    }
    public string Conds
    {
        // get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_CONDS)?.Value;
        get => MyApp.HttpContext.User.FindFirst(ClaimConst.CLAINM_CONDS)?.Value;
    }

    // public SysOrgUser User
    // {
    //     get => _sysUserRep.FirstOrDefault(u => u.id == UserId);
    // }

    // public UserManager(IHttpContextAccessor httpContextAccessor)
    // {
    //     _httpContextAccessor = httpContextAccessor;
    // }

    // /// <summary>
    // /// 获取用户信息
    // /// </summary>
    // /// <param name="userId"></param>
    // /// <param name="tracking"></param>
    // /// <returns></returns>
    // public async Task<SysOrgUser> CheckUserAsync(string userId, bool tracking = true)
    // {
    //     var user = await _sysUserRep.GetFirstAsync(u => u.id == userId);
    //     return user ?? throw Oops.Oh(ErrorCode.D1002);
    // }
    //
    // /// <summary>
    // /// 获取用户员工信息
    // /// </summary>
    // /// <param name="userId"></param>
    // /// <returns></returns>
    // public async Task<SysOrgUser> GetUserEmpInfo(string userId)
    // {
    //     var emp = await _sysUserRep.GetFirstAsync(u => u.id == userId);
    //     return emp ?? throw Oops.Oh(ErrorCode.D1002);
    // }
}
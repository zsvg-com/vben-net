using Microsoft.AspNetCore.Mvc.Filters;
using NewLife.Caching;
using Vben.Common.Core.Token;

namespace Vben.Common.Core.Filter;

/// <summary>
/// API授权判断
/// </summary>
public class SaCheckPermission : ActionFilterAttribute //, IAsyncActionFilter
{
    // private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    /// <summary>
    /// 权限字符串，多个用逗号隔开，例如 system:user:view,system:user:add
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// 角色字符串，例如 common,admin
    /// </summary>
    public string RolePermi { get; set; } = string.Empty;

    private bool HasPermi { get; set; }

    public SaCheckPermission()
    {
    }

    public SaCheckPermission(string value)
    {
        Value = value;
        HasPermi = !string.IsNullOrEmpty(Value);
    }

    /// <summary>
    /// 执行Action前校验是否有权限访问
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Console.WriteLine(Value);
        TokenModel info = JwtUtil.GetLoginUser(context.HttpContext);
        if (info.IsAdmin())
        {
            return base.OnActionExecutionAsync(context, next);
        }
        string userId = info?.UserId;
        
        // Console.WriteLine(info);
        ICache cache = MyApp.GetRequiredService<ICacheProvider>().Cache;
        var perms = cache.Get<HashSet<string>>("perms:"+userId);
        if (perms != null)
        {
            HasPermi = perms.Contains(Value);
        }

        if (!HasPermi)
        {
            throw new Exception("对不起，你没有此操作权限");
        }
        
        //
        // if (info != null && info?.UserId > 0)
        // {
        //     long userId = info.UserId;
        //     List<string> perms = CacheService.GetUserPerms(GlobalConstant.UserPermKEY + userId);
        //     List<string> rolePerms = info.RoleKeys;
        //
        //     if (perms == null)
        //     {
        //         var sysPermissionService = App.GetService<ISysPermissionService>();
        //         perms = sysPermissionService.GetMenuPermission(new SysUserDto() { UserId = userId });
        //         logger.Info("从数据库读取权限");
        //         CacheService.SetUserPerms(GlobalConstant.UserPermKEY + userId, perms);
        //     }
        //     info.Permissions = perms;
        //     if (perms.Exists(f => f.Equals(GlobalConstant.AdminPerm)))
        //     {
        //         HasPermi = true;
        //     }
        //     else if (rolePerms.Exists(f => f.Equals(GlobalConstant.AdminRole)))
        //     {
        //         HasPermi = true;
        //     }
        //     else if (!string.IsNullOrEmpty(Permission))
        //     {
        //         //HasPermi = perms.Exists(f => f.ToLower() == Permission.ToLower());
        //         var requiredPerms = Permission.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        //                       .Select(p => p.ToLower()).ToList();
        //         HasPermi = perms.Select(p => p.ToLower()).Intersect(requiredPerms).Any();
        //     }
        //     if (!HasPermi && !string.IsNullOrEmpty(RolePermi))
        //     {
        //         HasPermi = info.RoleKeys.Contains(RolePermi);
        //     }
        //     bool isDemoMode = AppSettings.GetAppConfig("DemoMode", false);
        //     var url = context.HttpContext.Request.Path;
        //     //演示公开环境屏蔽权限
        //     string[] denyPerms = ["update", "add", "remove", "add", "edit", "delete", "import", "run", "start", "stop", "clear", "send", "export", "upload", "common", "gencode", "reset", "forceLogout", "batchLogout"];
        //     if (isDemoMode)
        //     {
        //         var requiredPerms = Permission.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        //         if (requiredPerms.Any(p => denyPerms.Any(d => p.Contains(d, StringComparison.OrdinalIgnoreCase))))
        //         {
        //             context.Result = new JsonResult(new { code = (int)ResultCode.FORBIDDEN, msg = "演示模式 , 不允许操作" });
        //         }
        //     }
        //     if (!HasPermi && !Permission.Equals("common"))
        //     {
        //         logger.Info($"用户{info.UserName}没有权限访问{url}，当前权限[{Permission}]");
        //         var apiResult = new ApiResult((int)ResultCode.FORBIDDEN, $"你当前没有权限访问,请联系管理员", url);
        //         apiResult.Put("permi", Permission);
        //         JsonResult result = new(apiResult)
        //         {
        //             StatusCode = 403,
        //             ContentType = "application/json",
        //             Value = JsonConvert.SerializeObject(apiResult)
        //         };
        //         context.HttpContext.Response.StatusCode = 403;
        //         context.Result = result;
        //     }
        // }

        return base.OnActionExecutionAsync(context, next);
    }
}
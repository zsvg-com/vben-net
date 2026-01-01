using System.Security.Claims;

namespace Vben.Common.Core.Token;

public static class LoginHelper
{
 

    /// <summary>
    /// 获取请求上下文用户
    /// </summary>
    public static ClaimsPrincipal User => MyApp.HttpContext.User;

    public static string UserId => MyApp.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid);
    
}
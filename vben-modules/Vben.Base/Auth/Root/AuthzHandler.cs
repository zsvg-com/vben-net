using Furion.Authorization;
using Vben.Base.Sys.Perm.Api;

namespace Vben.Base.Auth.Root;

public class AuthzHandler : AppAuthorizeHandler
{
    /// <summary>
    /// 重写 Handler 添加自动刷新
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    //public override async Task HandleAsync(AuthorizationHandlerContext context)
    //{
    //    // Console.WriteLine(1111);
    //    // 自动刷新Token
    //    if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext()))
    //    {
    //        await AuthorizeHandleAsync(context);
    //    }
    //    else context.Fail(); // 授权失败
    //}

    /// <summary>
    /// 授权判断逻辑，授权通过返回 true，否则返回 false
    /// </summary>
    public override async Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
    {
        // 此处已经自动验证 Jwt Token的有效性了，无需手动验证

        // 管理员跳过判断httpContext.RequestServices
        var userManager = httpContext.RequestServices.GetRequiredService<IUserManager>();
        if (userManager.SuperAdmin) return true;
        var url = httpContext.Request.Path.Value;
        // if (url.StartsWith("/sys"))
        // {
        //     //只有管理员有sys权限
        //     return false;
        // }
        if (url.StartsWith("/pub")) //pub开头的通用访问，直接放行
        {
            // return true; 用下面这个是为了去除启动时async的警告
            return await Task.Run(() =>
            {
                return true;
            });
        }
        return checkPerm(userManager.Perms, httpContext.Request.Method, url);
    }

    /// <summary>
    /// 权限校验核心逻辑
    /// </summary>
    private bool checkPerm(string perms, string method, string url)
    {
        
        
        return true;
        //1.根据method分类快速定位到请求url的权限码
        int pos = -1;
        long code = -1;
        if (method == "GET")
        {
            if (url.Contains("info"))
            {
                url = url.Substring(0, url.LastIndexOf("/"));
            }

            foreach (var yperm in SysPermApiCache.GET_URLS)
            {
                if (yperm.url == url)
                {
                    pos = yperm.pos;
                    code = yperm.code;
                    break;
                }
            }
        }
        else if (method == "POST")
        {
            foreach (var yperm in SysPermApiCache.POST_URLS)
            {
                if (yperm.url == url)
                {
                    pos = yperm.pos;
                    code = yperm.code;
                    break;
                }
            }
        }
        else if (method == "PUT")
        {
            foreach (var yperm in SysPermApiCache.PUT_URLS)
            {
                if (yperm.url == url)
                {
                    pos = yperm.pos;
                    code = yperm.code;
                    break;
                }
            }
        }
        else if (method == "DELETE")
        {
            foreach (var yperm in SysPermApiCache.DELETE_URLS)
            {
                if (yperm.url == url)
                {
                    pos = yperm.pos;
                    code = yperm.code;
                    break;
                }
            }

            if (pos == -1)
            {
                url = url.Substring(0, url.LastIndexOf("/"));
                foreach (var yperm in SysPermApiCache.DELETE_URLS)
                {
                    if (yperm.url == url)
                    {
                        pos = yperm.pos;
                        code = yperm.code;
                        break;
                    }
                }
            }
        }

        //2.通过二进制&计算快速校验用户是否有权限
        if (pos != -1)
        {
            string[] permStrArr = perms.Split(";");
            long[] permArr = new long[permStrArr.Length];
            for (int i = 0; i < permStrArr.Length; i++)
            {
                permArr[i] = long.Parse(permStrArr[i]);
            }

            if (permArr.Length <= pos)
            {
                return false;
            }

            return (permArr[pos] & code) != 0;
        }

        return false;
    }


    private void GetPerm(string url,string method)
    {
        // if (method == "GET")
        // {
        //     if (url.Contains("info"))
        //     {
        //         url = url.Substring(0, url.LastIndexOf("/"));
        //     }
        //
        //     foreach (var yperm in SysPermApiCache.GET_URLS)
        //     {
        //         if (yperm.url == url)
        //         {
        //             pos = yperm.pos;
        //             code = yperm.code;
        //             break;
        //         }
        //     }
        // }
        // else if (method == "POST")
        // {
        //     foreach (var yperm in SysPermApiCache.POST_URLS)
        //     {
        //         if (yperm.url == url)
        //         {
        //             pos = yperm.pos;
        //             code = yperm.code;
        //             break;
        //         }
        //     }
        // }
        // else if (method == "PUT")
        // {
        //     foreach (var yperm in SysPermApiCache.PUT_URLS)
        //     {
        //         if (yperm.url == url)
        //         {
        //             pos = yperm.pos;
        //             code = yperm.code;
        //             break;
        //         }
        //     }
        // }
        // else if (method == "DELETE")
        // {
        //     foreach (var yperm in SysPermApiCache.DELETE_URLS)
        //     {
        //         if (yperm.url == url)
        //         {
        //             pos = yperm.pos;
        //             code = yperm.code;
        //             break;
        //         }
        //     }
        //
        //     if (pos == -1)
        //     {
        //         url = url.Substring(0, url.LastIndexOf("/"));
        //         foreach (var yperm in SysPermApiCache.DELETE_URLS)
        //         {
        //             if (yperm.url == url)
        //             {
        //                 pos = yperm.pos;
        //                 code = yperm.code;
        //                 break;
        //             }
        //         }
        //     }
        // }
    }
}
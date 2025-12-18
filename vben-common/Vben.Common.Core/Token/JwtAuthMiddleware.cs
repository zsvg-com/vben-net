using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Vben.Common.Core.Wrapper;

namespace Vben.Common.Core.Token
{
    /// <summary>
    /// jwt认证中间件
    /// </summary>
    public class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtAuthMiddleware> _logger;
        private static readonly string[] _whitelistPaths = new[]
        {
            ".png",
            "/msgHub"
        };

        public JwtAuthMiddleware(RequestDelegate next, ILogger<JwtAuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            
            // 如果请求是带扩展名的（即包含 .）
            if (path.Contains('.'))
            {
                await _next(context);
                return;
            }
            if (_whitelistPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null;

            if (allowAnonymous)
            {
                await _next(context);
                return;
            }

            string ip = HttpContextExtension.GetClientUserIp(context);
            string url = context.Request.Path;
            string osType = context.Request.Headers["os"];

            TokenModel loginUser = JwtUtil.GetLoginUser(context);
            if (loginUser != null)
            {
                var now = DateTime.UtcNow;
                var ts = loginUser.ExpireTime - now;
                var cacheKey = $"token_{loginUser.UserId}";

                // if (!CacheHelper.Exists(cacheKey) && ts.TotalMinutes < 5 && ts.TotalMinutes > 0)
                // {
                //     var newToken = JwtUtil.GenerateJwtToken(JwtUtil.AddClaims(loginUser));
                //     CacheHelper.SetCache(cacheKey, cacheKey, 1);
                //
                //     if (!string.IsNullOrEmpty(osType))
                //     {
                //         context.Response.Headers.Append("Access-Control-Expose-Headers", "X-Refresh-Token");
                //     }
                //
                //     context.Response.Headers.Append("X-Refresh-Token", newToken);
                //     _logger.LogInformation($"刷新Token: {loginUser.UserName}");
                // }

                // 还可以挂载到 context.User
                var identity = new ClaimsIdentity(JwtUtil.AddClaims(loginUser), "jwt");
                context.User = new ClaimsPrincipal(identity);
                await _next(context);
            }
            else
            {
                string msg = $"请求访问[{url}]失败，Token无效或未登录";
                _logger.LogWarning(msg);
                //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(ApiResult.Fail( msg,401));
            }
        }
    }
}

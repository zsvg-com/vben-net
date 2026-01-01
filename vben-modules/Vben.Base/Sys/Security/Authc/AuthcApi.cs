// using Vben.Base.Sys.Security.Pojo;
// using Vben.Core.Module.Sys;
//
// namespace Vben.Base.Sys.Security.Authc;
//
// //[ApiDescriptionSettings("Auth",Tag = "登录与注销")]
// /// <summary>
// /// 认证API:主要包含登录登出、token刷新、菜单获取、用户信息获取
// /// </summary>
// public class AuthcApi : IDynamicApiController, ITransient
// {
//     private readonly AuthcService _authcService;
//     private readonly IHttpContextAccessor _httpContextAccessor;
//     private readonly IEventPublisher _eventPublisher;
//     private readonly IUserManager _userManager; // 用户管理
//     private readonly SysCacheService _cache;
//
//     public AuthcApi(AuthcService authcService,
//         IHttpContextAccessor httpContextAccessor,
//         IEventPublisher eventPublisher,
//         IUserManager userManager,
//         SysCacheService sysCacheService)
//     {
//         _authcService = authcService;
//         _httpContextAccessor = httpContextAccessor;
//         _eventPublisher = eventPublisher;
//         _userManager = userManager;
//         _cache = sysCacheService;
//     }
//
//     /// <summary>
//     /// 登录
//     /// </summary>
//     [HttpPost("/login")]
//     [AllowAnonymous]
//     public async Task<Dictionary<string, object>> LoginAsync(LoginVo loginVo)
//     {
//         //1.登录验证
//         var duser = await _authcService.getDbUser(loginVo.username);
//         var password = SecureUtils.PasswordEncrypt(loginVo.password);
//         if (password != duser.password)
//         {
//             throw Oops.Oh(ErrorCode.D1000);
//         }
//
//         Dictionary<string, object> backDict = new Dictionary<string, object>();
//
//         //2.初始化用户信息，获取门户，菜单与按钮
//         Zuser zuser = new Zuser(duser);
//         _authcService.InitUser(zuser, duser, backDict); //这个方法非常重要
//
//         // 使用redis
//         // string redisKey = "" + YitIdHelper.NextId();
//         // _cache.Set(redisKey,zuser);
//
//         //3.创建accessToken
//         var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
//         {
//             {ClaimConst.CLAINM_USERID, duser.id},
//             {ClaimConst.TENANT_ID, "1"},
//             {ClaimConst.CLAINM_ACCOUNT, duser.username},
//             {ClaimConst.CLAINM_NAME, duser.name},
//             {ClaimConst.CLAINM_SUPERADMIN, zuser.IsAdmin ? 1 : 3},
//             {ClaimConst.CLAINM_PERMS, zuser.perms},
//             {ClaimConst.CLAINM_DEPTID, zuser.depid},
//             {ClaimConst.CLAINM_TYPE, zuser.type},
//             {ClaimConst.CLAINM_LABEL, zuser.label},
//             {ClaimConst.CLAINM_CONDS, zuser.conds}
//         }, 600);
//
//         backDict.Add("token", accessToken);
//         // backDict.Add("rtoken", accessToken);
//
//         //设置swagger自动登录
//         _httpContextAccessor.HttpContext.SigninToSwagger(accessToken);
//
//         //记录用户最后一次登录的ip与地点
//         var httpContext = App.HttpContext;
//         await _eventPublisher.PublishAsync(new ChannelEventSource("Update:UserLoginInfo",
//             new SysUser { id = duser.id, laloi = httpContext.GetLocalIpAddressToIPv4(), lalot = DateTime.Now }));
//
//         // 异步方式记录登录日志
//         //var ip = httpContext.GetRemoteIpAddressToIPv4();
//         //var crtim = DateTime.Now;
//         //var clent = Parser.GetDefault().Parse(httpContext.Request.Headers["User-Agent"]);
//         //var agbro = clent.UA.Family + clent.UA.Major;
//         //var ageos = clent.OS.Family + clent.OS.Major;
//         //await _eventPublisher.PublishAsync(new ChannelEventSource("Create:LoginLog",
//         //    new MonLogLogin
//         //    {
//         //        id = YitIdHelper.NextId() + "", name = zuser.name, ip = ip,
//         //        agbro = agbro, ageos = ageos, crtim = crtim, username = zuser.username
//         //    }));
//
//         return backDict;
//     }
//
//     /// <summary>
//     /// 登录
//     /// </summary>
//     [HttpPost("/token")]
//     [AllowAnonymous]
//     public async Task<string> GetToken(LoginVo loginVo)
//     {
//         //1.登录验证
//         var duser = await _authcService.getDbUser(loginVo.username);
//         var password = SecureUtils.PasswordEncrypt(loginVo.password);
//         if (password != duser.password)
//         {
//             throw Oops.Oh(ErrorCode.D1000);
//         }
//
//         Dictionary<string, object> backDict = new Dictionary<string, object>();
//
//         //2.初始化用户信息，获取门户，菜单与按钮
//         Zuser zuser = new Zuser(duser);
//         _authcService.InitUser(zuser, duser, backDict); //这个方法非常重要
//
//         // 使用redis
//         // string redisKey = "" + YitIdHelper.NextId();
//         // _cache.Set(redisKey,zuser);
//
//         //3.创建accessToken
//         var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
//         {
//             {ClaimConst.CLAINM_USERID, duser.id},
//             {ClaimConst.TENANT_ID, "1"},
//             {ClaimConst.CLAINM_ACCOUNT, duser.username},
//             {ClaimConst.CLAINM_NAME, duser.name},
//             {ClaimConst.CLAINM_SUPERADMIN, zuser.IsAdmin ? 1 : 3},
//             {ClaimConst.CLAINM_PERMS, zuser.perms},
//             {ClaimConst.CLAINM_DEPTID, zuser.depid},
//             {ClaimConst.CLAINM_TYPE, zuser.type},
//             {ClaimConst.CLAINM_LABEL, zuser.label},
//             {ClaimConst.CLAINM_CONDS, zuser.conds}
//         }, 600);
//
//         return accessToken;
//     }
//
//     /// <summary>
//     /// 注销
//     /// </summary>
//     [HttpGet("/logout")]
//     public void LogoutAsync()
//     {
//
//     }
//
//     /// <summary>
//     /// 获取用户信息
//     /// </summary>
//     [HttpGet("/getUserInfo")]
//     [AllowAnonymous]
//     public async Task<Zuser> getUserInfo()
//     {
//         Dictionary<string, object> backDict = new Dictionary<string, object>();
//
//         if (_userManager.UserId == null)
//         {
//             return null;
//         }
//
//         Zuser zuser = new Zuser
//         {
//             id = _userManager.UserId,
//             name = _userManager.Name,
//             username = _userManager.Account
//         };
//         var duser = await _authcService.getDbUser(zuser.username);
//         _authcService.InitUser(zuser, duser, backDict);
//
//         return zuser;
//     }
//
//     /// <summary>
//     /// 获取菜单树
//     /// </summary>
//     [HttpGet("/getMenuList")]
//     [AllowAnonymous]
//     public async Task<Dictionary<string, object>> getMenuList(string porid)
//     {
//         Dictionary<string, object> backDict = new Dictionary<string, object>();
//         if (_userManager.UserId == null)
//         {
//             throw Oops.Oh("登录过期").StatusCode(401);
//         }
//
//         Zuser zuser = new Zuser
//         {
//             id = _userManager.UserId,
//             name = _userManager.Name,
//             username = _userManager.Account,
//         };
//
//         if (string.IsNullOrEmpty(porid))
//         {
//             var duser = await _authcService.getDbUser(zuser.username);
//             _authcService.InitUser(zuser, duser, backDict);
//         }
//         else
//         {
//             await _authcService.SwitchPortal(zuser, backDict, porid);
//         }
//
//         return backDict;
//     }
// }
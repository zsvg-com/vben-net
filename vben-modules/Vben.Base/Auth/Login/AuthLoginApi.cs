using Admin.NET.Core.Service;
using Furion;
using Furion.EventBus;
using Vben.Base.Mon.Log.Login;
using Vben.Base.Mon.Online.User;
using Vben.Base.Sys.Security.Pojo;
using Vben.Base.Sys.User;
using Vben.Common.Core.Token;

namespace Vben.Base.Auth.Login;

[Route("")]
[ApiDescriptionSettings("Auth", Tag = "登录管理")]
public class AuthLoginApi(
    SqlSugarRepository<SysUser> userRepo,
    IHttpContextAccessor httpContextAccessor,
    IEventPublisher eventPublisher,
    SysCacheService cacheService,
    MonOnlineUserService monOnlineUserService
    )
    : ControllerBase
{

    [HttpPost("/auth/login")]
    [AllowAnonymous]
    public async Task<LoginVo> LoginAsync([FromBody] PasswordLoginBo bo)
    {
        string sql = @"select t.id,t.name,t.username,t.monum,t.password,t.tier,t.type,t.depid,d.name depna
         from sys_user t left join sys_dept d on d.id=t.depid 
         where t.username=@username and t.avtag="+Db.True;
        UserDo dbUser = await userRepo.Context.Ado.SqlQuerySingleAsync<UserDo>(sql, new { bo.username });
        if (dbUser==null || !BCrypt.Net.BCrypt.Verify(bo.password, dbUser.password))
        {
            throw new Exception("用户名或密码不正确");
        }

        // var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
        // {
        //     { "tenantId", "1" },
        //     { "userId", dbUser.id },
        //     { "Account", dbUser.username },
        //     { "name", dbUser.name },
        //     { "deptId", dbUser.depid },
        //     { "deptName", dbUser.depna },
        //     { "deptCategory", "1" },
        // }, 600);
        // //_httpContextAccessor.HttpContext.SigninToSwagger(accessToken);
        // LoginVo vo = new LoginVo();
        // vo.access_token = accessToken;
        // vo.expire_in = 604800;
        
        TokenModel model = new TokenModel();
        model.UserId = dbUser.id;
        model.UserName = dbUser.username;
        model.NickName = dbUser.name;
        model.DeptId = dbUser.depid;
        model.Permissions = new List<string>();
        model.RoleKeys = new List<string>();

        var accessToken= JwtUtil.GenerateJwtToken(JwtUtil.AddClaims(model));
        LoginVo vo = new LoginVo();
        vo.access_token = accessToken;
        vo.expire_in = 604800;
        
        // MyApp.HttpContext.SigninToSwagger(accessToken);
        
        // 单用户登录
        await monOnlineUserService.SingleLogin(dbUser.id);
        
        
        //记录用户最后一次登录的ip与地点
        var httpContext = App.HttpContext;
        await eventPublisher.PublishAsync(new ChannelEventSource("Update:UserLoginInfo",
            new SysUser {id = dbUser.id, loip = httpContext.GetLocalIpAddressToIPv4(), lotim = DateTime.Now}));

        // 异步方式记录登录日志
        var loip = httpContext.GetRemoteIpAddressToIPv4();
        var clent = Parser.GetDefault().Parse(httpContext.Request.Headers["User-Agent"]);
        var browser = clent.UA.Family + clent.UA.Major;
        var os = clent.OS.Family + clent.OS.Major;
        await eventPublisher.PublishAsync(new ChannelEventSource("Create:LoginLog",
            new MonLoginLog
            {
                id = YitIdHelper.NextId() + "", name = dbUser.name, loip = loip,sutag = true,
                browser = browser, os = os, lotim = DateTime.Now, username = dbUser.username
            }));

        return vo;
    }
    
    [HttpPost("/auth/logout")]
    public async Task Logout()
    {
        // var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) throw new Exception("退出异常");
        // httpContext.SignoutToSwagger();
        // await httpContext.SignOutAsync();
        cacheService.Remove("perms:"+LoginHelper.UserId);
    }

    // [HttpPost("/auth/login")]
    // [AllowAnonymous]
    // public async Task<LoginVo> LoginAsync()
    // {
    //      var request = App.HttpContext.Request;
    //
    //      // 1. 从请求头获取加密密钥
    //      if (!request.Headers.TryGetValue("Encrypt-Key", out var encryptKeyValues))
    //      {
    //           throw Oops.Oh("缺少加密密钥");
    //      }
    //
    //      string encryptKey = encryptKeyValues.ToString();
    //      if (string.IsNullOrEmpty(encryptKey))
    //      {
    //           throw Oops.Oh("加密密钥为空");
    //      }
    //      
    //      // 2. 读取请求体字符串
    //      using var reader = new StreamReader(request.Body, Encoding.UTF8);
    //      var encryptedData = await reader.ReadToEndAsync();
    //
    //      if (string.IsNullOrEmpty(encryptedData))
    //      {
    //           throw Oops.Oh("加密数据为空");
    //      }
    //
    //      // 3. 解密数据
    //      Console.WriteLine(encryptKey);
    //      Console.WriteLine(encryptedData);
    //      
    //      //解密再处理，有点难
    //      // string publicKey ="MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAJnNwrj4hi/y3CCJu868ghCG5dUj8wZK++RNlTLcXoMmdZWEQ/u02RgD5LyLAXGjLOjbMtC+/J9qofpSGTKSx/MCAwEAAQ==";
    //      // string privateKey ="MIIBVAIBADANBgkqhkiG9w0BAQEFAASCAT4wggE6AgEAAkEAqhHyZfSsYourNxaY7Nt+PrgrxkiA50efORdI5U5lsW79MmFnusUA355oaSXcLhu5xxB38SMSyP2KvuKNPuH3owIDAQABAkAfoiLyL+Z4lf4Myxk6xUDgLaWGximj20CUf+5BKKnlrK+Ed8gAkM0HqoTt2UZwA5E2MzS4EI2gjfQhz5X28uqxAiEA3wNFxfrCZlSZHb0gn2zDpWowcSxQAgiCstxGUoOqlW8CIQDDOerGKH5OmCJ4Z21v+F25WaHYPxCFMvwxpcw99EcvDQIgIdhDTIqD2jfYjPTY8Jj3EDGPbH2HHuffvflECt3Ek60CIQCFRlCkHpi7hthhYhovyloRYsM+IS9h/0BzlEAuO0ktMQIgSPT3aFAgJYwKpqRYKlLDVcflZFCKY7u3UP8iWi1Qw0Y=";
    //      // // string aa= AESEncryption.Decrypt(encryptKey, privateKey);
    //      // string aa= RSAEncryption.Decrypt(encryptedData, privateKey); 
    //      // Console.WriteLine(aa);
    //      
    //      var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
    //     {
    //         {"tenantId", "1"},
    //         {"userId", "1"},
    //         {"userName", "admin"},
    //         {"deptId", 1},
    //         {"deptName", "XX科技"},
    //         {"deptCategory", "1"},
    //     }, 600);
    //      
    //      LoginVo vo= new LoginVo();
    //      vo.access_token = accessToken;
    //      vo.expire_in = 604800;
    //
    //      return vo;
    // }
}
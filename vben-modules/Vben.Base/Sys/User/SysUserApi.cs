using Vben.Base.Sys.Dept;
using Vben.Base.Sys.User.bo;

namespace Vben.Base.Sys.User;

/// <summary>
/// 用户管理
/// </summary>
[Route("sys/user")]
[ApiDescriptionSettings("Sys", Tag = "系统用户")]
public class SysUserApi(SysUserService userService) : ControllerBase
{
    /// <summary>
    /// 用户分页查询
    /// </summary>
    /// <param name="bo">查询对象</param>
    /// <returns></returns>
    [HttpGet]
    [SaCheckPermission("sys:user:query")]
    public async Task<dynamic> Get(UserSearchBo bo)
    {
        var pp = XreqUtil.GetPp();
        var expable = Expressionable.Create<SysUser>();
        if (bo.IsAllNullWithoutDepit())
        {
            expable.And(t => t.depid == bo.depid);
        }
        else
        {
            expable.AndIF(!string.IsNullOrWhiteSpace(bo.name),t => t.name.Contains(bo.name.Trim()));
            expable.AndIF(!string.IsNullOrWhiteSpace(bo.username),t => t.username.Contains(bo.username.Trim()));
            expable.AndIF(!string.IsNullOrWhiteSpace(bo.email),t => t.email.Contains(bo.email.Trim()));
            expable.AndIF(!string.IsNullOrWhiteSpace(bo.monum),t => t.monum.Contains(bo.monum.Trim()));
            expable.AndIF(!string.IsNullOrWhiteSpace(bo.notes),t => t.notes.Contains(bo.notes.Trim()));
        }
        var items = await userService.repo.Context.Queryable<SysUser,SysDept>((t, d)
                => new JoinQueryInfos(JoinType.Left, d.id == t.depid))
            .Where(expable.ToExpression())
            .OrderBy(t => t.ornum)
            .Select((t,d) => new { t.id, t.name, t.notes, t.crtim, t.uptim,t.avtag,t.monum,t.username,depna = d.name })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    [SaCheckPermission("sys:user:query")]
    public async Task<SysUser> GetInfo(string id)
    {
        var user = await userService.SingleAsync(id);
        return user;
    }

    // //得到当前登录的用户的信息
    // [HttpGet("user")]
    // public async Task<dynamic> GetUser(DefaultHttpContext httpContext)
    // {
    //     var userManager = MyApp.GetService<IUserManager>(); 有问题,要用下面的
    //     var userManager = httpContext.RequestServices.GetRequiredService<IUserManager>();
    //     string uid = userManager.UserId;   //用户id
    //     string name = userManager.Name;    //姓名
    //     string account = userManager.Account;  //工号
    //     string rolesql = "SELECT spr.id \"id\",spr.name \"name\" FROM SYS_PORTAL_ROLE_ORG spro LEFT JOIN SYS_PORTAL_ROLE spr ON spro.rid=spr.id WHERE spro.oid =@userid";
    //     List<dynamic> roles = await _userService.repo.Context.Ado.SqlQueryAsync<dynamic>(rolesql, new { userid = uid });
    //     return new { uid, name, account, roles };
    // }

    [HttpPost]
    [SaCheckPermission("sys:user:edit")]
    public async Task Post([FromBody] SysUser user)
    {
        await userService.InsertAsync(user);
    }

    [HttpPut("password")]
    [SaCheckPermission("sys:user:password")]
    public async Task PutPassword([FromBody] IdPasswordBo bo)
    {
        await userService.ResetPassword(bo);
    }
    
    // [HttpPut("password")]
    // public async Task PutPassword()
    // {
    //     var request = MyApp.HttpContext.Request;
    //     // 1. 从请求头获取加密密钥
    //     if (!request.Headers.TryGetValue("Encrypt-Key", out var encryptKeyValues))
    //     {
    //         throw new Exception("缺少加密密钥");
    //     }
    //     
    //     string encryptKey = encryptKeyValues.ToString();
    //     if (string.IsNullOrEmpty(encryptKey))
    //     {
    //         throw new Exception("加密密钥为空");
    //     }
    //     
    //     // 2. 读取请求体字符串
    //     using var reader = new StreamReader(request.Body, Encoding.UTF8);
    //     
    //     var encryptedData = await reader.ReadToEndAsync();
    //     
    //     if (string.IsNullOrEmpty(encryptedData))
    //     {
    //         throw new Exception("加密数据为空");
    //     }
    //     
    //     // 3. 解密数据
    //     Console.WriteLine(encryptKey);
    //     Console.WriteLine(encryptedData);
    //     
    //     // await _userService.ResetPassword(bo);
    // }
    
    [HttpPut("avtag")]
    [SaCheckPermission("sys:user:avtag")]
    public async Task PutAvtag([FromBody] IdAvtagBo bo)
    {
        await userService.ChangeAvtag(bo);
    }

    [HttpPut]
    [SaCheckPermission("sys:user:edit")]
    public async Task Put([FromBody] SysUser user)
    {
        await userService.UpdateAsync(user);
    }

    [HttpDelete("{ids}")]
    [SaCheckPermission("sys:user:delete")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await userService.DeleteAsync(ids);
    }
}
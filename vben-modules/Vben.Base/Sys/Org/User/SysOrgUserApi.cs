using System.Text;
using Vben.Base.Sys.Org.Dept;
using Vben.Base.Sys.Org.User.bo;

namespace Vben.Base.Sys.Org.User;

[Route("sys/org/user")]
[ApiDescriptionSettings("Sys", Tag = "组织架构-用户")]
public class SysOrgUserApi : ControllerBase
{
    private readonly SysOrgUserService _userService;
    public SysOrgUserApi(SysOrgUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<dynamic> Get(string name, string depid)
    {
        var pp = XreqUtil.GetPp();
        var expable = Expressionable.Create<SysOrgUser>();
        if (!string.IsNullOrWhiteSpace(name))
        {
            expable.And(t => t.name.Contains(name.Trim()));
        }
        else
        {
            if (depid == "")
            {
                expable.And(t => t.depid == null);
            }
            else if (!string.IsNullOrWhiteSpace(depid))
            {
                expable.And(t => t.depid == depid);
            }
        }

        var items = await _userService.repo.Context.Queryable<SysOrgUser,SysOrgDept>((t, d)
                => new JoinQueryInfos(JoinType.Left, d.id == t.depid))
            .Where(expable.ToExpression())
            .OrderBy(t => t.ornum)
            .Select((t,d) => new { t.id, t.name, t.notes, t.crtim, t.uptim,t.avtag,t.monum,t.usnam,depna = d.name })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    public async Task<SysOrgUser> GetInfo(string id)
    {
        var user = await _userService.SingleAsync(id);
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
    public async Task Post([FromBody] SysOrgUser user)
    {
        await _userService.InsertAsync(user);
    }

    [HttpPut("pacod")]
    public async Task PutPacod([FromBody] IdPacodBo bo)
    {
        await _userService.ResetPacod(bo);
    }
    
    // [HttpPut("pacod")]
    // public async Task PutPacod()
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
    //     // await _userService.ResetPacod(bo);
    // }
    
    [HttpPut("avtag")]
    public async Task PutAvtag([FromBody] IdAvtagBo bo)
    {
        await _userService.ChangeAvtag(bo);
    }

    [HttpPut]
    public async Task Put([FromBody] SysOrgUser user)
    {
        await _userService.UpdateAsync(user);
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await _userService.DeleteAsync(ids);
    }
}
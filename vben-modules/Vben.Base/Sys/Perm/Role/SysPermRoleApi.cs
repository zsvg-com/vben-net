using Dm.util;
using Mapster;
using Vben.Base.Sys.Org.User;
using Vben.Base.Sys.Perm.Api;
using Vben.Base.Sys.Perm.Menu;
using Vben.Base.Sys.Perm.Role.bo;
using Vben.Base.Sys.Perm.Role.vo;

namespace Vben.Base.Sys.Perm.Role;

[Route("sys/perm/role")]
[ApiDescriptionSettings("Sys", Tag = "权限管理-角色")]
public class SysPermRoleApi(
    SqlSugarRepository<SysPermRole> repo,
    SqlSugarRepository<SysPermRoleToMenu> roleToMenuRepo,
    SqlSugarRepository<SysPermRoleToApi> roleToApiRepo)
    : ControllerBase
{
    [HttpGet]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await repo.Context.Queryable<SysPermRole>()
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .OrderBy(u => u.ornum)
            .Select((t) => new { t.id, t.name, t.notes, t.crtim, t.uptim })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }
    
    /// <summary>
    /// 获取分类treeTable数据
    /// </summary>
    [HttpGet("perms")]
    public async Task<List<MenuVo>> GetPerms()
    {
        string menuSql = "select id,name,pid,icon,type from sys_perm_menu where avtag="+Db.True+" order by ornum";
        List<MenuVo> menus= await repo.Context.Ado.SqlQueryAsync<MenuVo>(menuSql);
        
        string apiSql = "select id,name,menid from sys_perm_api where avtag="+Db.True+" order by ornum";
        List<ApiVo> apis= await repo.Context.Ado.SqlQueryAsync<ApiVo>(apiSql);

        foreach (MenuVo menu in menus)
        {
            foreach (ApiVo api in apis)
            {
                if (api.menid == menu.id)
                {
                    menu.apis.Add(api);
                }
            }
        }

        return menus;
    }

    [HttpGet("info/{id}")]
    public async Task<SysPermRoleVo> GetInfo(long id)
    {
        SysPermRoleVo role =await repo.Context.SqlQueryable<SysPermRoleVo>("select * from sys_perm_role where id=@id")
            .AddParameters(new SugarParameter[] { new("@id", id) }).SingleAsync();
        
        List<long> menus= await roleToMenuRepo.AsQueryable()
            .Where(t => t.rid == id).Select(t => t.mid).ToListAsync();
        
        List<long> apis= await roleToApiRepo.AsQueryable()
            .Where(t => t.rid == id).Select(t => t.aid).ToListAsync();
        
        List<SysOrg> orgs =await repo.Context.SqlQueryable<SysOrg>(@"select t.id,t.name,t.type from sys_org t 
                inner join sys_perm_role_org o on o.oid=t.id where o.rid=@rid")
            .AddParameters(new SugarParameter[] { new("@rid", id) }).ToListAsync();
        
        role.menus = menus;
        role.apis = apis;
        role.orgs = orgs;
        return role;
    }

    [HttpPost]
    public async Task Post([FromBody] SysPermRoleBo bo)
    {
        SysPermRole role = bo.Adapt<SysPermRole>();
        role.menus=new List<SysPermMenu>();
        foreach (var mid in bo.menus)  
        {
            role.menus.Add(new SysPermMenu(mid));
        }
        role.apis=new List<SysPermApi>();
        foreach (var aid in bo.apis)  
        {
            role.apis.Add(new SysPermApi(aid));
        }
        role.id = YitIdHelper.NextId();
        role.crtim = DateTime.Now;
        await repo.Context.InsertNav(role)
            .Include(it => it.orgs)
            .Include(it => it.menus)
            .Include(it => it.apis)
            .ExecuteCommandAsync();
        await PostFlush();
    }

    [HttpPut]
    public async Task Put([FromBody] SysPermRoleBo bo)
    {
        SysPermRole role = bo.Adapt<SysPermRole>();
        role.menus=new List<SysPermMenu>();
        foreach (var mid in bo.menus)  
        {
            role.menus.Add(new SysPermMenu(mid));
        }
        role.apis=new List<SysPermApi>();
        foreach (var aid in bo.apis)  
        {
            role.apis.Add(new SysPermApi(aid));
        }
        role.uptim = DateTime.Now;
        await repo.Context.UpdateNav(role)
            .Include(it => it.orgs,new UpdateNavOptions { ManyToManyIsUpdateA=true })
            .Include(it => it.menus)
            .Include(it => it.apis)
            .ExecuteCommandAsync();
        await PostFlush();
    }

    [HttpPut("flush")]
    public async Task PostFlush()
    {
        await repo.Context.Updateable<SysOrgUser>().SetColumns(it => it.catag == false)
            .Where(it => it.catag == true)
            .ExecuteCommandAsync();
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await repo.Context.Deleteable<SysPermRole>().In(idArr).ExecuteCommandAsync();
    }
}
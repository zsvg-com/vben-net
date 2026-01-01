using Mapster;
using Vben.Base.Sys.Api;
using Vben.Base.Sys.Menu;
using Vben.Base.Sys.Role.bo;
using Vben.Base.Sys.Role.vo;
using Vben.Base.Sys.User;

namespace Vben.Base.Sys.Role;

[Route("sys/role")]
[ApiDescriptionSettings("Sys", Tag = "权限管理-角色")]
public class SysRoleApi(
    SqlSugarRepository<SysRole> repo,
    SqlSugarRepository<SysRoleToMenu> roleToMenuRepo,
    SqlSugarRepository<SysRoleToApi> roleToApiRepo)
    : ControllerBase
{
    [HttpGet]
    [SaCheckPermission("sys:role:query")]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await repo.Context.Queryable<SysRole>()
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
        string menuSql = "select id,name,pid,icon,type from sys_menu where avtag=" + Db.True + " order by ornum";
        List<MenuVo> menus = await repo.Context.Ado.SqlQueryAsync<MenuVo>(menuSql);

        string apiSql = "select id,name,menid from sys_api where avtag=" + Db.True + " order by ornum";
        List<ApiVo> apis = await repo.Context.Ado.SqlQueryAsync<ApiVo>(apiSql);

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
    [SaCheckPermission("sys:role:query")]
    public async Task<SysRoleVo> GetInfo(long id)
    {
        SysRoleVo role = await repo.Context.SqlQueryable<SysRoleVo>("select * from sys_role where id=@id")
            .AddParameters(new SugarParameter[] { new("@id", id) }).SingleAsync();

        List<long> menus = await roleToMenuRepo.AsQueryable()
            .Where(t => t.rid == id).Select(t => t.mid).ToListAsync();

        List<long> apis = await roleToApiRepo.AsQueryable()
            .Where(t => t.rid == id).Select(t => t.aid).ToListAsync();

        List<SysOrg> orgs = await repo.Context.SqlQueryable<SysOrg>(@"select t.id,t.name,t.type from sys_org t 
                inner join sys_role_org o on o.oid=t.id where o.rid=@rid")
            .AddParameters(new SugarParameter[] { new("@rid", id) }).ToListAsync();

        role.menus = menus;
        role.apis = apis;
        role.orgs = orgs;
        return role;
    }

    [HttpPost]
    [SaCheckPermission("sys:role:edit")]
    public async Task Post([FromBody] SysRoleBo bo)
    {
        SysRole role = bo.Adapt<SysRole>();
        role.menus = new List<SysMenu>();
        foreach (var mid in bo.menus)
        {
            role.menus.Add(new SysMenu(mid));
        }

        role.apis = new List<SysApi>();
        foreach (var aid in bo.apis)
        {
            role.apis.Add(new SysApi(aid));
        }

        role.id = YitIdHelper.NextId();
        role.crtim = DateTime.Now;
        await repo.Context.InsertNav(role)
            .Include(it => it.orgs)
            .Include(it => it.menus)
            .Include(it => it.apis)
            .ExecuteCommandAsync();
        await PutCache();
    }

    [HttpPut]
    [SaCheckPermission("sys:role:edit")]
    public async Task Put([FromBody] SysRoleBo bo)
    {
        SysRole role = bo.Adapt<SysRole>();
        role.menus = new List<SysMenu>();
        foreach (var mid in bo.menus)
        {
            role.menus.Add(new SysMenu(mid));
        }

        role.apis = new List<SysApi>();
        foreach (var aid in bo.apis)
        {
            role.apis.Add(new SysApi(aid));
        }

        role.uptim = DateTime.Now;
        await repo.Context.UpdateNav(role)
            .Include(it => it.orgs, new UpdateNavOptions { ManyToManyIsUpdateA = true })
            .Include(it => it.menus)
            .Include(it => it.apis)
            .ExecuteCommandAsync();
        await PutCache();
    }

    [HttpPut("cache")]
    [SaCheckPermission("sys:role:cache")]
    public async Task PutCache()
    {
        //todo redis处理
        await repo.Context.Updateable<SysUser>().SetColumns(it => it.catag == false)
            .Where(it => it.catag == true)
            .ExecuteCommandAsync();
    }

    [HttpDelete("{ids}")]
    [SaCheckPermission("sys:role:delete")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await repo.Context.Deleteable<SysRole>().In(idArr).ExecuteCommandAsync();
    }
}
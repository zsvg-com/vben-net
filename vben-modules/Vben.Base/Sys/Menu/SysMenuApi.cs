using Vben.Base.Sys.User;
using Vben.Common.Core.Token;

namespace Vben.Base.Sys.Menu;

[Route("sys/menu")]
[ApiDescriptionSettings("Sys", Tag = "权限管理-菜单")]
public class SysMenuApi(SqlSugarRepository<SysMenu> repo) : ControllerBase
{
    [HttpGet("list")]
    [SaCheckPermission("sys:menu:query")]
    public async Task<dynamic> GetList(string name)
    {
        //select id,pid,name,type,crtim,uptim,notes,comp,shtag,path from sys_portal_menu where porid=@porid order by ornum
        var list = await repo.Context
            .Queryable<SysMenu>()
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .OrderBy(it => it.ornum)
            .ToListAsync();
        return list;
    }
    
    /// <summary>
    /// 获取分类treeTable数据
    /// </summary>
    [HttpGet("tree")]
    [SaCheckPermission("sys:menu:query")]
    public async Task<List<Ltree>> GetTree(long id)
    {
        // var treeList = await _repo.Context
        //     .Queryable<SysMenu>()
        //     .Where(t => t.id != id)
        //     .OrderBy("ornum")
        //     .ToTreeAsync(it => it.children, it => it.pid, null);
        // return treeList;
        
        string sql = "select t.id,t.pid,t.name from sys_menu t order by t.ornum";
        List<Ltree> list= await repo.Context.Ado.SqlQueryAsync<Ltree>(sql);
       return TreeUtils.BuildLtree(list);
    }

    [HttpGet("info/{id}")]
    [SaCheckPermission("sys:menu:query")]
    public async Task<SysMenu> GetInfo(long id)
    {
        var menu = await repo.GetSingleAsync(t => t.id == id);
        return menu;
    }

    [HttpPost]
    [SaCheckPermission("sys:menu:edit")]
    public async Task Post([FromBody] SysMenu menu)
    {
        menu.id = YitIdHelper.NextId();
        menu.cruid = LoginHelper.UserId;
        menu.crtim = DateTime.Now;
        await repo.InsertAsync(menu);
        await PostFlush();
    }

    [HttpPut]
    [SaCheckPermission("sys:menu:edit")]
    public async Task Put([FromBody] SysMenu menu)
    {
        menu.uptim = DateTime.Now;
        menu.upuid = LoginHelper.UserId;
        await repo.UpdateAsync(menu);
        await PostFlush();
    }

    [HttpDelete("{ids}")]
    [SaCheckPermission("sys:menu:delete")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        foreach (var id in idArr)
        {
            var count = await repo.Context.Queryable<SysMenu>().Where(it => it.pid ==long.Parse(id)).CountAsync();
            if (count > 0)
            {
                throw new Exception("有子菜单或按钮无法删除");
            }
        }
        await repo.Context.Deleteable<SysMenu>().In(idArr).ExecuteCommandAsync();
        await PostFlush();
    }

    [NonAction]
    [SaCheckPermission("sys:menu:flush")]
    private async Task PostFlush()
    {
        await repo.Context.Updateable<SysUser>().SetColumns(it => it.catag == false)
            .Where(it => it.catag == true)
            .ExecuteCommandAsync();
    }
}
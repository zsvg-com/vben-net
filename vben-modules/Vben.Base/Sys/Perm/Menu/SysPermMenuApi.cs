using Vben.Base.Sys.Org.User;

namespace Vben.Base.Sys.Perm.Menu;

[Route("sys/perm/menu")]
[ApiDescriptionSettings("Sys", Tag = "权限管理-菜单")]
public class SysPermMenuApi(SqlSugarRepository<SysPermMenu> repo) : ControllerBase
{
    [HttpGet("list")]
    public async Task<dynamic> GetList(string name)
    {
        //select id,pid,name,type,crtim,uptim,notes,comp,shtag,path from sys_portal_menu where porid=@porid order by ornum
        var list = await repo.Context
            .Queryable<SysPermMenu>()
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .OrderBy(it => it.ornum)
            .ToListAsync();
        return list;
    }
    
    /// <summary>
    /// 获取分类treeTable数据
    /// </summary>
    [HttpGet("tree")]
    public async Task<List<Ltree>> GetTree(long id)
    {
        // var treeList = await _repo.Context
        //     .Queryable<SysPermMenu>()
        //     .Where(t => t.id != id)
        //     .OrderBy("ornum")
        //     .ToTreeAsync(it => it.children, it => it.pid, null);
        // return treeList;
        
        string sql = "select t.id,t.pid,t.name from sys_perm_menu t order by t.ornum";
        List<Ltree> list= await repo.Context.Ado.SqlQueryAsync<Ltree>(sql);
       return TreeUtils.BuildLtree(list);
    }

    [HttpGet("info/{id}")]
    public async Task<SysPermMenu> GetInfo(long id)
    {
        var menu = await repo.GetSingleAsync(t => t.id == id);
        return menu;
    }

    [HttpPost]
    public async Task Post([FromBody] SysPermMenu menu)
    {
        menu.id = YitIdHelper.NextId();
        menu.cruid = XuserUtil.getUserId();
        menu.crtim = DateTime.Now;
        await repo.InsertAsync(menu);
        await PostFlush();
    }

    [HttpPut]
    public async Task Put([FromBody] SysPermMenu menu)
    {
        menu.uptim = DateTime.Now;
        menu.upuid = XuserUtil.getUserId();
        await repo.UpdateAsync(menu);
        await PostFlush();
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        foreach (var id in idArr)
        {
            var count = await repo.Context.Queryable<SysPermMenu>().Where(it => it.pid ==long.Parse(id)).CountAsync();
            if (count > 0)
            {
                throw new Exception("有子菜单或按钮无法删除");
            }
        }
        await repo.Context.Deleteable<SysPermMenu>().In(idArr).ExecuteCommandAsync();
        await PostFlush();
    }

    [NonAction]
    private async Task PostFlush()
    {
        await repo.Context.Updateable<SysOrgUser>().SetColumns(it => it.catag == false)
            .Where(it => it.catag == true)
            .ExecuteCommandAsync();
    }
}
using Vben.Base.Sys.Group;

namespace Vben.Base.Pub.Org.Group;

[Route("pub/org/group")]
[ApiDescriptionSettings("Pub", Tag = "群组查询")]
public class PubOrgGroupApi(SqlSugarRepository<SysGroup> repo) : ControllerBase
{
    [HttpGet("tree")]
    public async Task<dynamic> GetTree()
    {
        var treeList = await repo.Context.SqlQueryable<Stree>
            ("select id,pid,name,'cate' type from sys_group_cate " +
             "union all select id,catid as pid,name,'group' type from sys_group")
            .ToTreeAsync(it => it.children, it => it.pid, null);
        return treeList;
    }

    [HttpGet("list")]
    public async Task<dynamic> GetList(string pid, string type, string name)
    {
        if (type == "cate")
        {
            var list = await repo.Context.SqlQueryable<SidName>
                    ("select id,name from sys_group where catid=@catid order by ornum")
                .AddParameters(new SugarParameter[] { new SugarParameter("@catid", pid) })
                .ToListAsync();
            return list;
        }
        else if (type == "group")
        {
            var list = await repo.Context.SqlQueryable<SidName>
                    ("select t.id,t.name from sys_org t inner join sys_group_org o on o.oid=t.id where o.gid=@gid")
                .AddParameters(new SugarParameter[] { new SugarParameter("@gid", pid) })
                .ToListAsync();
            return list;
        }
        else
        {
            return new List<SidName>();
        }
    }


}
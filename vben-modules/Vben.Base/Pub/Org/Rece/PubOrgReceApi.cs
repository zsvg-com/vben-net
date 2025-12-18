using Vben.Base.Sys.Org.Dept;
using Vben.Base.Sys.Org.Post;
using Vben.Base.Sys.Org.Rece;
using Vben.Base.Sys.Org.User;

namespace Vben.Base.Pub.Org.Rece;

[Route("pub/org/rece")]
[ApiDescriptionSettings("Pub", Tag = "组织架构最近使用")]
public class PubOrgReceApi(SysOrgReceService service) : ControllerBase
{
    [HttpPost]
    public async Task Post([FromBody] List<SysOrgRece> reces)
    {
        if (reces != null && reces.Count > 0)
        {
            await service.update(reces);
        }
    }

    [HttpGet]
    public async Task<dynamic> Get(int type)
    {
        string userId = XuserUtil.getUserId();
        List<dynamic> list = new List<dynamic>();
        
        if ( (type & 1) != 0)  //部门
        {
            var deptList = await service._repo.Context.Queryable<SysOrgRece, SysOrgDept>((t, d)
                    => new JoinQueryInfos(JoinType.Inner, d.id == t.oid))
                .OrderBy(t => t.uptim, OrderByType.Desc)
                .Where(t => t.useid == userId)
                .Select((t, d) => new { id = t.oid, name = d.name })
                .ToListAsync();
            list.AddRange(deptList);
        }
        
        if ((type & 2) != 0)//用户
        {
            var userList = await service._repo.Context.Queryable<SysOrgRece, SysOrgUser, SysOrgDept>((t, u, d)
                    => new JoinQueryInfos(JoinType.Inner, u.id == t.oid, JoinType.Inner, d.id == u.depid))
                .OrderBy(t => t.uptim, OrderByType.Desc)
                .Where(t => t.useid == userId)
                .Select((t, u, d) => new { id = t.oid, name = u.name, dept = d.name })
                .ToListAsync();
            list.AddRange(userList);
        }

        if ((type & 4) != 0) //岗位
        {
            var postList = await service._repo.Context.Queryable<SysOrgRece, SysOrgPost, SysOrgDept>((t, u, d)
                    => new JoinQueryInfos(JoinType.Inner, u.id == t.oid, JoinType.Inner, d.id == u.depid))
                .OrderBy(t => t.uptim, OrderByType.Desc)
                .Where(t => t.useid == userId)
                .Select((t, u, d) => new { id = t.oid, name = u.name, dept = d.name })
                .ToListAsync();
            list.AddRange(postList);
        }

        return list;
    }
}
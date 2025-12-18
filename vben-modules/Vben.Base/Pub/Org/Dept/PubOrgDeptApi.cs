using System;
using System.Collections.Generic;
using Vben.Base.Sys.Org.Dept;

namespace Vben.Base.Pub.Org.Dept;

[Route("pub/org/dept")]
[ApiDescriptionSettings("Pub", Tag = "部门查询")]
public class PubOrgDeptApi(SqlSugarRepository<SysOrgDept> repo) : ControllerBase
{
    [HttpGet("tree")]
    public List<Stree> GetTree()
    {
        var trees = repo.Context.SqlQueryable<Stree>("select id,pid,name from sys_org_dept where avtag="+Db.True)
            .ToTreeAsync(it => it.children, it => it.pid, null).Result;
        Console.WriteLine(trees);
        return trees;
    }
}
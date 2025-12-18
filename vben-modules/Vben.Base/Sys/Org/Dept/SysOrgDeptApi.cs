namespace Vben.Base.Sys.Org.Dept;

[Route("sys/org/dept")]
[ApiDescriptionSettings("Sys", Tag = "组织架构-部门")]
public class SysOrgDeptApi(SysOrgDeptService service) : ControllerBase
{
    [HttpGet]
    public async Task<dynamic> Get(string name, string pid)
    {
        var pp = XreqUtil.GetPp();
        var expable = Expressionable.Create<SysOrgDept>();
        if (!string.IsNullOrWhiteSpace(name))
        {
            expable.And(t => t.name.Contains(name.Trim()));
        }
        else
        {
            if (pid == "")
            {
                expable.And(t => t.pid == null);
            }
            else if (!string.IsNullOrWhiteSpace(pid))
            {
                expable.And(t => t.pid == pid);
            }
        }

        var items = await service.repo.Context.Queryable<SysOrgDept>()
            .Where(expable.ToExpression())
            .OrderBy(u => u.ornum)
            .Select((t) => new { t.id, t.name, t.notes, t.crtim, t.uptim })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("tree")]
    public List<Stree> GetTree(string id)
    {
        Sqler sqler = new Sqler("sys_org_dept");
        sqler.addWhere("t.avtag = "+Db.True).addSelect("t.pid");
        if (id != null) {
            sqler.addWhere("t.tier not like @tier", "@tier","%" + id + "%");
        }
        sqler.addOrder("t.ornum");
        var trees = service.repo.Context.SqlQueryable<Stree>(sqler.getSql()).AddParameters(sqler.getSugarParams())
            .ToTreeAsync(it => it.children, it => it.pid, null).Result;
        return trees;
    }

    [HttpGet("list")]
    public async Task<List<SysOrgDeptVo>> GetList()
    {
        string sql = @"select t.id,t.pid,t.name,t.crtim,t.uptim,t.notes,o1.name cruna,o2.name upuna from sys_org_dept t 
                     left join sys_org o1 on o1.id=t.cruid left join sys_org o2 on o2.id=t.upuid 
                     where t.avtag="+Db.True+" order by t.ornum";
        List<SysOrgDeptVo> list=await service.repo.Context.Ado.SqlQueryAsync<SysOrgDeptVo>(sql);
        // List<dynamic> list = await _service.repo.Context.Ado.SqlQueryAsync<dynamic>(sql);
        return list;
    }

    [HttpGet("info/{id}")]
    public async Task<SysOrgDept> GetInfo(string id)
    {
        var sysOrgDept = await service.SingleAsync(id);
        return sysOrgDept;
    }

    [HttpPost]
    public async Task<string> Post([FromBody] SysOrgDept dept)
    {
        await service.InsertAsync(dept);
        return dept.id;
    }

    [HttpPut]
    public async Task<string> Put([FromBody] SysOrgDept dept)
    {
        await service.UpdateAsync(dept);
        return dept.id;
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await service.DeleteAsync(idArr);
    }

    [HttpPost("move")]
    public async Task PostMove([FromBody] TreeMovePo po)
    {
        await service.PostMove(po);
    }


}
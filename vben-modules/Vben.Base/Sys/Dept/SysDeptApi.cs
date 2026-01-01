namespace Vben.Base.Sys.Dept;

[Route("sys/dept")]
[ApiDescriptionSettings("Sys", Tag = "组织架构-部门")]
public class SysDeptApi(SysDeptService service) : ControllerBase
{
    [HttpGet]
    [SaCheckPermission("sys:dept:query")]
    public async Task<dynamic> Get(string name, string pid)
    {
        var pp = XreqUtil.GetPp();
        var expable = Expressionable.Create<SysDept>();
        if (!string.IsNullOrWhiteSpace(name))
        {
            expable.And(t => t.name.Contains(name.Trim()));
        }
        else if(!string.IsNullOrWhiteSpace(pid))
        {
            expable.And(t => t.pid == pid);
        }
        var items = await service.repo.Context.Queryable<SysDept>()
            .Where(expable.ToExpression())
            .OrderBy(u => u.ornum)
            .Select((t) => new { t.id, t.name, t.notes, t.crtim, t.uptim })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("tree")]
    [SaCheckPermission("sys:dept:query")]
    public List<Stree> GetTree(string id)
    {
        Sqler sqler = new Sqler("sys_dept");
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
    [SaCheckPermission("sys:dept:query")]
    public async Task<List<SysDeptVo>> GetList()
    {
        string sql = @"select t.id,t.pid,t.name,t.crtim,t.uptim,t.notes,o1.name cruna,o2.name upuna from sys_dept t 
                     left join sys_org o1 on o1.id=t.cruid left join sys_org o2 on o2.id=t.upuid 
                     where t.avtag="+Db.True+" order by t.ornum";
        List<SysDeptVo> list=await service.repo.Context.Ado.SqlQueryAsync<SysDeptVo>(sql);
        // List<dynamic> list = await _service.repo.Context.Ado.SqlQueryAsync<dynamic>(sql);
        return list;
    }

    [HttpGet("info/{id}")]
    [SaCheckPermission("sys:dept:query")]
    public async Task<SysDept> GetInfo(string id)
    {
        var sysDept = await service.SingleAsync(id);
        return sysDept;
    }

    [HttpPost]
    [SaCheckPermission("sys:dept:edit")]
    public async Task<string> Post([FromBody] SysDept dept)
    {
        await service.InsertAsync(dept);
        return dept.id;
    }

    [HttpPut]
    [SaCheckPermission("sys:dept:edit")]
    public async Task<string> Put([FromBody] SysDept dept)
    {
        await service.UpdateAsync(dept);
        return dept.id;
    }

    [HttpDelete("{ids}")]
    [SaCheckPermission("sys:dept:delete")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await service.DeleteAsync(idArr);
    }

    [HttpPost("move")]
    [SaCheckPermission("sys:dept:edit")]
    public async Task PostMove([FromBody] TreeMovePo po)
    {
        await service.PostMove(po);
    }


}
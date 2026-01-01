using Vben.Base.Tool.Form;

[Route("tool/form")]
[ApiDescriptionSettings("Tool", Tag = "在线表单")]
public class ToolFormApi(ToolFormService service) : ControllerBase
{
    [HttpGet]
    [SaCheckPermission("tool:form:query")]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await service.Repo.Context.Queryable<ToolForm>()
            .LeftJoin<SysOrg>((t, c) => c.id == t.cruid)
            .LeftJoin<SysOrg>((t, c, u) => u.id == t.upuid)
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .Select((t, c, u) => new
            {
                t.id, t.name, t.crtim, t.uptim, t.notes,
                cruna = c.name, upuna = u.name
            })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    [SaCheckPermission("tool:form:query")]
    public async Task<ToolForm> GetInfo(long id)
    {
        var main = await service.Repo.Context.Queryable<ToolForm>()
            .Where(it => it.id == id).FirstAsync();
        return main;
    }

    [HttpPost]
    [SaCheckPermission("tool:form:edit")]
    public async Task<long> Post([FromBody] ToolForm main)
    {
        DateTime now = DateTime.Now;
        main.name = now.ToString("yyyy-MM-dd HH:mm:ss");;
        return await service.Insert(main);
    }

    [HttpPut]
    [SaCheckPermission("tool:form:edit")]
    public async Task<long> Put([FromBody] ToolForm main)
    {
        return await service.Update(main);
    }

    [HttpDelete]
    [SaCheckPermission("tool:form:delete")]
    public async Task<int> Delete(string ids)
    {
        return await service.Delete(ids);
    }
}
namespace Vben.Base.Tool.Num;

[Route("tool/num")]
[ApiDescriptionSettings("Tool", Tag = "编号工具")]
public class ToolNumApi(ToolNumService service) : ControllerBase
{
    [HttpGet("list")]
    [SaCheckPermission("tool:num:query")]
    public async Task<dynamic> GetList()
    {
        return await service.repo.GetListAsync();
    }

    [HttpGet]
    [SaCheckPermission("tool:num:query")]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await service.repo.Context.Queryable<ToolNum>()
            .WhereIF(!string.IsNullOrWhiteSpace(name), t
                => t.name.Contains(name.Trim()) || t.id.Contains(name.Trim()))
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    [SaCheckPermission("tool:num:query")]
    public async Task<ToolNum> GetInfo(string id)
    {
        return await service.SingleAsync(id);
    }

    [HttpPost]
    [SaCheckPermission("tool:num:edit")]
    public async Task Post([FromBody] ToolNum main)
    {
        await service.InsertAsync(main);
    }

    [HttpPut]
    [SaCheckPermission("tool:num:edit")]
    public async Task Put([FromBody] ToolNum main)
    {
        await service.UpdateAsync(main);
    }

    [HttpDelete("{ids}")]
    [SaCheckPermission("tool:num:delete")]
    public async Task Delete(string ids)
    {
        await service.DeleteAsync(ids);
    }
}
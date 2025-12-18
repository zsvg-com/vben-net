namespace Vben.Base.Tool.Dict.Data;

[Route("tool/dict/data")]
[ApiDescriptionSettings("Tool", Tag = "字典配置")]
public class ToolDictDataApi(ToolDictDataService service) : ControllerBase
{
    [HttpGet]
    public async Task<dynamic> Get(string dicid, string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await service.repo.Context.Queryable<ToolDictData>()
            .WhereIF(!string.IsNullOrWhiteSpace(name), t
                => t.name.Contains(name.Trim()) || t.dalab.Contains(name.Trim()))
            .Where((t) => t.dicid == dicid)
            .OrderBy(u => u.ornum)
            .Select((t) => new { t.id, t.name, t.dalab, t.daval, t.crtim, t.uptim, t.notes })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }
    
    [HttpGet("list")]
    public async Task<dynamic> GetList(string dicid)
    {
        return await service.repo.Context.Queryable<ToolDictData>()
            .Where(it => it.dicid == dicid).ToListAsync();
    }
    
    [HttpGet("info/{id}")]
    public async Task<ToolDictData> GetInfo(string id)
    {
        var data = await service.repo.Context.Queryable<ToolDictData>()
            .Where(it => it.id == id).FirstAsync();
        return data;
    }
    
    [HttpPost]
    public async Task Post([FromBody] ToolDictData data)
    {
        await service.InsertAsync(data);
    }
    
    [HttpPut]
    public async Task Put([FromBody] ToolDictData data)
    {
        await service.UpdateAsync(data);
    }
    
    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        await service.DeleteAsync(ids);
    }
}
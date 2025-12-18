using Vben.Base.Tool.Dict.Data;

namespace Vben.Base.Tool.Dict.Main;

[Route("tool/dict/main")]
[ApiDescriptionSettings("Tool", Tag = "字典数据")]
public class ToolDictMainApi(ToolDictMainService service) : ControllerBase
{
    [HttpGet("list")]
    public async Task<dynamic> GetList()
    {
        return await service.repo.GetListAsync();
    }

    [HttpGet]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await service.repo.Context.Queryable<ToolDictMain>()
            .WhereIF(!string.IsNullOrWhiteSpace(name), t
                => t.name.Contains(name.Trim()) || t.id.Contains(name.Trim()))
            .OrderBy(u => u.ornum)
            .Select((t) => new { t.id, t.name, t.crtim, t.uptim, t.notes })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    public async Task<ToolDictMain> GetInfo(string id)
    {
        return await service.SingleAsync(id); ;
    }
    
    [HttpGet("data")]
    public async Task<List<ToolDictDataVo>> GetData(string code)
    {
        return await service.findData(code); 
    }

    [HttpPost]
    public async Task Post([FromBody] ToolDictMain main)
    {
        await service.InsertAsync(main);
    }

    [HttpPut]
    public async Task Put([FromBody] ToolDictMain main)
    {
        await service.UpdateAsync(main);
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        await service.DeleteAsync(ids);
    }
}
namespace Vben.Base.Tool.Dict.Cate;

[Route("tool/dict/cate")]
[ApiDescriptionSettings("Tool", Tag = "字典分类")]
public class ToolDictCateApi(ToolDictCateService service) : ControllerBase
{
    [HttpGet]
    public async Task<dynamic> Get()
    {
        var pp = XreqUtil.GetPp();
        var items = await service.repo.Context.Queryable<ToolDictCate>()
            .OrderBy(u => u.ornum)
            .Select((t) => new { t.id, t.name, t.notes })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    public async Task<ToolDictCate> GetInfo(string id)
    {
        var cate = await service.repo.Context.Queryable<ToolDictCate>()
            .Where(it => it.id == id).FirstAsync();
        return cate;
    }

    [HttpPost]
    public async Task Post([FromBody] ToolDictCate cate)
    {
        await service.InsertAsync(cate);
    }

    [HttpPut]
    public async Task Put([FromBody] ToolDictCate cate)
    {
        await service.UpdateAsync(cate);
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        await service.DeleteAsync(ids);
    }
}
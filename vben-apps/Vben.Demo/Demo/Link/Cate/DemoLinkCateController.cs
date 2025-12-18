namespace Vben.Admin.Demo.Link.Cate;

/// <summary>
/// 关联分类表案例
/// </summary>
[Route("demo/link/cate")]
[ApiDescriptionSettings(Tag = "关联分类表案例")]
public class DemoLinkCateConroller(DemoLinkCateService service) : ControllerBase
{
    private const string TABLE = "demo_link_cate";

    /// <summary>
    /// 关联分类表-列表查询
    /// </summary>
    [HttpGet("list")]
    public async Task<dynamic> GetList(string name)
    {
        var list = await service.Repo.Context
            .Queryable<DemoLinkCate>()
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .OrderBy(it => it.ornum)
            .ToListAsync();
        return list;
    }
    
    /// <summary>
    /// 关联分类表-树状查询
    /// </summary>
    [HttpGet("tree")]
    public async Task<List<Ltree>> GetTree(long id)
    {
        Sqler sqler=new Sqler(TABLE);
        return await service.findTreeList(sqler,id);
    }

    /// <summary>
    /// 关联分类表-详情查询
    /// </summary>
    [HttpGet("info/{id}")]
    public async Task<DemoLinkCate> GetInfo(long id)
    {
        return await service.Select(id);
    }

    /// <summary>
    /// 关联分类表-新增
    /// </summary>
    [HttpPost]
    public async Task<long> Post([FromBody] DemoLinkCate cate)
    {
        return await service.Insert(cate);
    }

    /// <summary>
    /// 关联分类表-修改
    /// </summary>
    [HttpPut]
    public async Task<long> Put([FromBody] DemoLinkCate cate)
    {
        return await service.Update(cate,TABLE);
    }

    /// <summary>
    /// 关联分类表-删除
    /// </summary>
    [HttpDelete("{ids}")]
    public async Task<int> Delete(string ids)
    {
        return await service.Delete(ids);
    }
    
    /// <summary>
    /// 关联分类表-移动
    /// </summary>
    [HttpPost("move")]
    public async Task PostMove([FromBody] Lmove po)
    {
        await service.Move(po,TABLE);
    }
}
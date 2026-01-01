namespace Vben.Admin.Demo.Single.Cate;

/// <summary>
/// 单一树表案例
/// </summary>
[Route("demo/single/cate")]
[ApiDescriptionSettings(Tag = "单一树表案例")]
public class DemoSingleCateConroller(DemoSingleCateService service) : ControllerBase
{
    private const string TABLE = "demo_single_cate";

    /// <summary>
    /// 单一树表-列表查询
    /// </summary>
    [HttpGet("list")]
    [SaCheckPermission("single:cate:query")]
    public async Task<dynamic> GetList(string name)
    {
        var list = await service.Repo.Context
            .Queryable<DemoSingleCate>()
            .LeftJoin<SysOrg>((t, c) => c.id == t.cruid)
            .LeftJoin<SysOrg>((t, c, u) => u.id == t.upuid)
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .OrderBy(t => t.ornum)
            .Select((t, c, u) => new
            {
                t.id, t.name, t.crtim, t.uptim, t.notes, t.pid, t.ornum,
                cruna = c.name, upuna = u.name
            })
            .ToListAsync();
        return list;
    }

    /// <summary>
    /// 单一树表-树状查询
    /// </summary>
    [HttpGet("tree")]
    [SaCheckPermission("single:cate:query")]
    public async Task<List<Ltree>> GetTree(long id, string name)
    {
        Sqler sqler = new Sqler(TABLE);
        sqler.addLike("t.name", name);
        return await service.findTreeList(sqler, id);
    }

    /// <summary>
    /// 单一树表-详情查询
    /// </summary>
    [HttpGet("info/{id}")]
    [SaCheckPermission("single:cate:query")]
    public async Task<DemoSingleCate> GetInfo(long id)
    {
        return await service.Select(id);
    }

    /// <summary>
    /// 单一树表-新增
    /// </summary>
    [HttpPost]
    [SaCheckPermission("single:cate:edit")]
    public async Task<long> Post([FromBody] DemoSingleCate cate)
    {
        return await service.Insert(cate);
    }

    /// <summary>
    /// 单一树表-修改
    /// </summary>
    [HttpPut]
    [SaCheckPermission("single:cate:edit")]
    public async Task<long> Put([FromBody] DemoSingleCate cate)
    {
        return await service.Update(cate, TABLE);
    }

    /// <summary>
    /// 单一树表-删除
    /// </summary>
    [HttpDelete("{ids}")]
    [SaCheckPermission("single:cate:delete")]
    public async Task<int> Delete(string ids)
    {
        return await service.Delete(ids);
    }
}
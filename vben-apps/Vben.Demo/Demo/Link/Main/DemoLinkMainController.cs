using Vben.Admin.Demo.Link.Cate;

namespace Vben.Admin.Demo.Link.Main;

/// <summary>
/// 关联主表案例
/// </summary>
[Route("demo/link/main")]
[ApiDescriptionSettings(Tag = "关联主表案例")]
public class DemoLinkMainConroller(DemoLinkMainService service) : ControllerBase
{
    /// <summary>
    /// 关联主表-分页查询
    /// </summary>
    [HttpGet]
    public async Task<dynamic> Get(string name,long catid)
    {
        var pp = XreqUtil.GetPp();
        var items = await service.Repo.Context.Queryable<DemoLinkMain>()
            .LeftJoin<SysOrg>((t, c) => c.id == t.cruid)
            .LeftJoin<SysOrg>((t, c, u) => u.id == t.upuid)
            .LeftJoin<DemoLinkCate>((t, c, u,v) => v.id == t.catid)
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .WhereIF(catid!=0, t => t.catid.Equals(catid))
            .Select((t, c, u,v) => new
            {
                t.id, t.name, t.crtim, t.uptim, t.notes,
                cruna = c.name, upuna = u.name, catna=v.name
            })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    /// <summary>
    /// 关联主表-详情查询
    /// </summary>
    [HttpGet("info/{id}")]
    public async Task<DemoLinkMain> GetInfo(long id)
    {
        var main = await service.Select(id);
        main.items.Sort((p1, p2) => p1.ornum.CompareTo(p2.ornum));
        return main;
    }

    /// <summary>
    /// 关联主表-新增
    /// </summary>
    [HttpPost]
    public async Task<long> Post([FromBody] DemoLinkMain main)
    {
        return await service.Insert(main);
    }

    /// <summary>
    /// 关联主表-修改
    /// </summary>
    [HttpPut]
    public async Task<long> Put([FromBody] DemoLinkMain main)
    {
        return await service.Update(main);
    }

    /// <summary>
    /// 关联主表-删除
    /// </summary>
    [HttpDelete("{ids}")]
    public async Task<int> Delete(string ids)
    {
        return await service.Delete(ids);
    }
}
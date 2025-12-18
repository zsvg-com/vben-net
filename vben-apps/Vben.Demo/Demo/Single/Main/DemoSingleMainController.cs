using Vben.Common.Core.Filter;

namespace Vben.Admin.Demo.Single.Main;

/// <summary>
/// 单一主表案例
/// </summary>
[Route("demo/single/main")]
[ApiDescriptionSettings(Tag = "单一主表案例")]
public class DemoEasyMainConroller(DemoSingleMainService service) : ControllerBase
{
    /// <summary>
    /// 单一主表-分页查询
    /// </summary>
    [HttpGet]
    [ActionPermissionFilter(Permission = "system:user:query")]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await service.Repo.Context.Queryable<DemoSingleMain>()
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

    /// <summary>
    /// 单一主表-详情查询
    /// </summary>
    [HttpGet("info/{id}")]
    public async Task<DemoSingleMain> GetInfo(long id)
    {
        // var main= await repo.Context.Queryable<TEntity>().IncludesAllFirstLayer().SingleAsync(t => t.id == id);
        // if (main == null) throw new Exception("记录不存在");
        // SelectCuInfo(main);
        // return main;
        return await service.Select(id);
    }

    /// <summary>
    /// 单一主表-新增
    /// </summary>
    [HttpPost]
    [Oplog(title="单一主表案例",type = 1)]
    public async Task<long> Post([FromBody] DemoSingleMain main)
    {
        return await service.Insert(main);
    }

    /// <summary>
    /// 单一主表-修改
    /// </summary>
    [HttpPut]
    [Oplog(title="单一主表案例",type = 2)]
    public async Task<long> Put([FromBody] DemoSingleMain main)
    {
        return await service.Update(main);
    }

    /// <summary>
    /// 单一主表-删除
    /// </summary>
    [HttpDelete("{ids}")]
    [Oplog(title="单一主表案例",type = 3)]
    public async Task<int> Delete(string ids)
    {
        return await service.Delete(ids);
    }
}
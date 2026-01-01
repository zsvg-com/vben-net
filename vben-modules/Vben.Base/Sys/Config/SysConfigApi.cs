using NewLife.Caching;

namespace Vben.Base.Sys.Config;

[Route("sys/config")]
[ApiDescriptionSettings("Sys", Tag = "参数管理")]
public class SysConfigApi(SqlSugarRepository<SysConfig> repo) : ControllerBase
{
    
    [HttpGet("list")]
    [SaCheckPermission("sys:config:query")]
    public async Task<dynamic> GetList()
    {
        return await repo.GetListAsync();
    }

    [HttpGet]
    [SaCheckPermission("sys:config:query")]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await repo.Context.Queryable<SysConfig>()
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            // .Select((t) => new { t.id, t.name, t.crtim, t.uptim })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    [SaCheckPermission("sys:config:query")]
    public async Task<SysConfig> GetInfo(long id)
    {
        ICache cache = MyApp.GetRequiredService<ICacheProvider>().Cache;
        var main = cache.Get<SysConfig>("bbb");
        if (main != null)
        {
            return main;
        }
        main = await repo.Context.Queryable<SysConfig>()
            .Where(it => it.id == id).FirstAsync();
        cache.Add("bbb", main);
        return main;
    }

    [HttpPost]
    [SaCheckPermission("sys:config:edit")]
    public async Task<long> Post([FromBody] SysConfig main)
    {
        main.id = YitIdHelper.NextId();
        await repo.InsertAsync(main);
        return main.id;
    }

    [HttpPut]
    [SaCheckPermission("sys:config:edit")]
    public async Task<long> Put([FromBody] SysConfig main)
    {
        main.uptim = DateTime.Now;
        await repo.UpdateAsync(main);
        return main.id;
    }

    [HttpDelete("{ids}")]
    [SaCheckPermission("sys:config:delete")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await repo.Context.Deleteable<SysConfig>().In(idArr).ExecuteCommandAsync();
    }
}
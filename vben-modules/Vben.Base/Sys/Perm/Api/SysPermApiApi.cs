namespace Vben.Base.Sys.Perm.Api;

[Route("sys/perm/api")]
[ApiDescriptionSettings("Sys", Tag = "权限管理-接口")]
public class SysPermApiApi(SqlSugarRepository<SysPermApi> repo) : ControllerBase
{
    [HttpGet("list")]
    public async Task<dynamic> GetList()
    {
        return await repo.GetListAsync();
    }

    [HttpGet]
    public async Task<dynamic> Get(string name, string menid)
    {
        var pp = XreqUtil.GetPp();
        var items = await repo.Context.Queryable<SysPermApi>()
            .WhereIF(!string.IsNullOrEmpty(menid) && menid != "0", t => t.menid.Equals(menid))
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .Select((t) => new { t.id, t.perm, t.type, t.crtim, t.uptim, t.name })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    public async Task<SysPermApi> GetInfo(long id)
    {
        var main = await repo.Context.Queryable<SysPermApi>()
            .Where(it => it.id == id).FirstAsync();
        return main;
    }

    [HttpPost]
    public async Task<long> Post([FromBody] SysPermApi api)
    {
        api.id = YitIdHelper.NextId();
        await repo.InsertAsync(api);
        return api.id;
    }

    [HttpPut]
    public async Task<long> Put([FromBody] SysPermApi api)
    {
        api.uptim = DateTime.Now;
        await repo.UpdateAsync(api);
        return api.id;
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await repo.Context.Deleteable<SysPermApi>().In(idArr).ExecuteCommandAsync();
    }
}
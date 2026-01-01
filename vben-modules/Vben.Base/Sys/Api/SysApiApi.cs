namespace Vben.Base.Sys.Api;

[Route("sys/api")]
[ApiDescriptionSettings("Sys", Tag = "权限管理-接口")]
public class SysApiApi(SqlSugarRepository<SysApi> repo) : ControllerBase
{
    [HttpGet("list")]
    [SaCheckPermission("sys:api:query")]
    public async Task<dynamic> GetList()
    {
        return await repo.GetListAsync();
    }

    [HttpGet]
    [SaCheckPermission("sys:api:query")]
    public async Task<dynamic> Get(string name, long menid)
    {
        var pp = XreqUtil.GetPp();
        var items = await repo.Context.Queryable<SysApi>()
            .WhereIF(menid != 0, t => t.menid.Equals(menid))
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .Select((t) => new { t.id, t.perm, t.type, t.crtim, t.uptim, t.name })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    [SaCheckPermission("sys:api:query")]
    public async Task<SysApi> GetInfo(long id)
    {
        var main = await repo.Context.Queryable<SysApi>()
            .Where(it => it.id == id).FirstAsync();
        return main;
    }

    [HttpPost]
    [SaCheckPermission("sys:api:edit")]
    public async Task<long> Post([FromBody] SysApi api)
    {
        api.id = YitIdHelper.NextId();
        await repo.InsertAsync(api);
        return api.id;
    }

    [HttpPut]
    [SaCheckPermission("sys:api:edit")]
    public async Task<long> Put([FromBody] SysApi api)
    {
        api.uptim = DateTime.Now;
        await repo.UpdateAsync(api);
        return api.id;
    }

    [HttpDelete("{ids}")]
    [SaCheckPermission("sys:api:edit")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await repo.Context.Deleteable<SysApi>().In(idArr).ExecuteCommandAsync();
    }
}
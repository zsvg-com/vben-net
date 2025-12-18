namespace Vben.Base.Sys.Notice;

[Route("sys/notice")]
[ApiDescriptionSettings("Sys", Tag = "通知管理")]
public class SysNoticeApi(SqlSugarRepository<SysNotice> repo) : ControllerBase
{
    [HttpGet("list")]
    public async Task<dynamic> GetList()
    {
        return await repo.GetListAsync();
    }

    [HttpGet]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await repo.Context.Queryable<SysNotice>()
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .Select((t) => new { t.id, t.name, t.crtim, t.uptim,t.avtag })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    public async Task<SysNotice> GetInfo(long id)
    {
        var main = await repo.Context.Queryable<SysNotice>()
            .Where(it => it.id == id).FirstAsync();
        return main;
    }

    [HttpPost]
    public async Task<long> Post([FromBody] SysNotice main)
    {
        main.id = YitIdHelper.NextId() ;
        await repo.InsertAsync(main);
        return main.id;
    }

    [HttpPut]
    public async Task<long> Put([FromBody] SysNotice main)
    {
        main.uptim = DateTime.Now;
        await repo.UpdateAsync(main);
        return main.id;
    }
    
    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await repo.Context.Deleteable<SysNotice>().In(idArr).ExecuteCommandAsync();
    }

}
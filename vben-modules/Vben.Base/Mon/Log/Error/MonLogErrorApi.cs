namespace Vben.Base.Mon.Log.Error;

[Route("mon/log/error")]
[ApiDescriptionSettings("Mon", Tag = "错误日志")]
public class MonLogErrorApi(SqlSugarRepository<MonLogError> repo) : ControllerBase
{
    [HttpGet]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await repo.Context.Queryable<MonLogError>()
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .OrderBy(t => t.crtim, OrderByType.Desc)
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    public async Task<MonLogError> GetInfo(string id)
    {
        return await repo.GetSingleAsync(t => t.id == id);
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await repo.Context.Deleteable<MonLogError>().In(idArr).ExecuteCommandAsync();
    }

    [HttpDelete("all")]
    public async Task DeleteAll()
    {
        await repo.Context.Deleteable<MonLogError>().ExecuteCommandAsync();
    }
}
namespace Vben.Base.Mon.Oper.Log;

[Route("mon/oper/log")]
[ApiDescriptionSettings("Mon", Tag = "操作日志")]
public class MonOperLogApi(SqlSugarRepository<MonOperLog> repo) : ControllerBase
{
    [HttpGet]
    public async Task<dynamic> Get()
    {
        var pp = XreqUtil.GetPp();
        var items = await repo.Context.Queryable<MonOperLog>()
            .OrderBy(t => t.optim, OrderByType.Desc)
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    public async Task<MonOperLog> GetInfo(string id)
    {
        return await repo.GetSingleAsync(t => t.id == id);
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {

        var idArr = ids.Split(",");
        await repo.Context.Deleteable<MonOperLog>().In(idArr).ExecuteCommandAsync();
    }

    [HttpDelete("all")]
    public async Task DeleteAll()
    {
        await repo.Context.Deleteable<MonOperLog>().ExecuteCommandAsync();
    }
}
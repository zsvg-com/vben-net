namespace Vben.Base.Mon.Job.Log;

[Route("mon/job/log")]
[ApiDescriptionSettings("Mon", Tag = "定时任务-日志")]
public class MonJobLogApi(MonJobLogService monJobLogService) : ControllerBase
{
    [HttpGet]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await monJobLogService.repo.Context.Queryable<MonJobLog>()
            .Select((t) => new { t.id, t.name, t.sttim, t.entim, t.msg, t.ret })
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .OrderByDescending(t=>t.sttim)
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    public async Task<MonJobLog> GetInfo(string id)
    {
        var cate = await monJobLogService.SingleAsync(id);
        return cate;
    }
    
    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await monJobLogService.DeleteAsync(idArr);
    }
}
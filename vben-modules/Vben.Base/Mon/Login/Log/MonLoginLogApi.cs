using Vben.Base.Mon.Log.Login;

namespace Vben.Base.Mon.Login.Log;

[Route("mon/login/log")]
[ApiDescriptionSettings("Mon", Tag = "登录日志")]
public class MonLoginLogApi(MonLoginLogService service) : ControllerBase
{
    [HttpGet]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await service.repo.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    public async Task<MonLoginLog> GetInfo(string id)
    {
        return await service.SingleAsync(id);
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        await service.DeleteAsync(ids);
    }

    [HttpDelete("all")]
    public async Task DeleteAll()
    {
        await service.repo.Context.Deleteable<MonLoginLog>().ExecuteCommandAsync();
    }
}
namespace Vben.Base.Sys.Group;

[Route("sys/group")]
[ApiDescriptionSettings("Sys", Tag = "组织架构-群组")]
public class SysGroupApi(SysGroupService groupService) : ControllerBase
{
    [HttpGet]
    [SaCheckPermission("sys:group:query")]
    public async Task<dynamic> Get(string name, string catid)
    {
        var pp = XreqUtil.GetPp();
        if (string.IsNullOrEmpty(catid))
        {
            var items = await groupService.repo.Context.Queryable<SysGroup>()
                .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
                .OrderBy(u => u.ornum)
                .Select((t) => new { t.id, t.name, t.notes, t.crtim, t.uptim })
                .ToPageListAsync(pp.page, pp.pageSize, pp.total);
            return RestPageResult.Build(pp.total.Value, items);
        }
        else
        {
            var items = await groupService.repo.Context.Queryable<SysGroup>()
                .Where(t => t.catid == catid)
                .OrderBy(u => u.ornum)
                .Select((t) => new { t.id, t.name, t.notes, t.crtim, t.uptim })
                .ToPageListAsync(pp.page, pp.pageSize, pp.total);
            return RestPageResult.Build(pp.total.Value, items);
        }
    }

    [HttpGet("info/{id}")]
    [SaCheckPermission("sys:group:query")]
    public async Task<SysGroup> GetInfo(string id)
    {
        var group= await groupService.repo.Context.Queryable<SysGroup>()
            .Where(it => it.id == id)
            .Includes(t => t.crman)
            .Includes(t => t.upman)
            .Includes(t => t.members)
            .SingleAsync();
        return group;
    }

    [HttpPost]
    [SaCheckPermission("sys:group:edit")]
    public async Task Post([FromBody] SysGroup group)
    {
        await groupService.InsertAsync(group);
    }

    [HttpPut]
    [SaCheckPermission("sys:group:edit")]
    public async Task Put([FromBody] SysGroup group)
    {
        await groupService.UpdateAsync(group);
    }

    [HttpDelete("{ids}")]
    [SaCheckPermission("sys:group:delete")]
    public async Task Delete(string ids)
    {
        await groupService.DeleteAsync(ids);
    }
}
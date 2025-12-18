namespace Vben.Base.Sys.Org.Group;

[Route("sys/org/group")]
[ApiDescriptionSettings("Sys", Tag = "组织架构-群组")]
public class SysOrgGroupApi(SysOrgGroupService groupService) : ControllerBase
{
    [HttpGet]
    public async Task<dynamic> Get(string name, string catid)
    {
        var pp = XreqUtil.GetPp();
        if (string.IsNullOrEmpty(catid))
        {
            var items = await groupService.repo.Context.Queryable<SysOrgGroup>()
                .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
                .OrderBy(u => u.ornum)
                .Select((t) => new { t.id, t.name, t.notes, t.crtim, t.uptim })
                .ToPageListAsync(pp.page, pp.pageSize, pp.total);
            return RestPageResult.Build(pp.total.Value, items);
        }
        else
        {
            var items = await groupService.repo.Context.Queryable<SysOrgGroup>()
                .Where(t => t.catid == catid)
                .OrderBy(u => u.ornum)
                .Select((t) => new { t.id, t.name, t.notes, t.crtim, t.uptim })
                .ToPageListAsync(pp.page, pp.pageSize, pp.total);
            return RestPageResult.Build(pp.total.Value, items);
        }
    }

    [HttpGet("info/{id}")]
    public async Task<SysOrgGroup> GetInfo(string id)
    {
        var group= await groupService.repo.Context.Queryable<SysOrgGroup>()
            .Where(it => it.id == id)
            .Includes(t => t.crman)
            .Includes(t => t.upman)
            .Includes(t => t.members)
            .SingleAsync();
        return group;
    }

    [HttpPost]
    public async Task Post([FromBody] SysOrgGroup group)
    {
        await groupService.InsertAsync(group);
    }

    [HttpPut]
    public async Task Put([FromBody] SysOrgGroup group)
    {
        await groupService.UpdateAsync(group);
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        await groupService.DeleteAsync(ids);
    }
}
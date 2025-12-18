using Vben.Base.Sys.Org.Dept;

namespace Vben.Base.Sys.Org.Post;

[Route("sys/org/post")]
[ApiDescriptionSettings("Sys", Tag = "组织架构-岗位")]
public class SysOrgPostApi(SysOrgPostService postService) : ControllerBase
{
    [HttpGet]
    public async Task<dynamic> Get(string name, string depid)
    {
        var pp = XreqUtil.GetPp();
        var expable = Expressionable.Create<SysOrgPost>();
        if (!string.IsNullOrWhiteSpace(name))
        {
            expable.And(t => t.name.Contains(name.Trim()));
        }
        else
        {
            if (depid == "")
            {
                expable.And(t => t.depid == null);
            }
            else if (!string.IsNullOrWhiteSpace(depid))
            {
                expable.And(t => t.depid == depid);
            }
        }
        
        var items = await postService.repo.Context.Queryable<SysOrgPost,SysOrgDept>((t, d)
                => new JoinQueryInfos(JoinType.Left, d.id == t.depid))
            .Where(expable.ToExpression())
            .OrderBy(t => t.ornum)
            .Select((t,d) => new { t.id, t.name, t.notes, t.crtim, t.uptim,depna = d.name })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    public async Task<SysOrgPost> GetInfo(string id)
    {
        var post= await postService.repo.Context.Queryable<SysOrgPost>()
            .Where(it => it.id == id)
            .Includes(t => t.crman)
            .Includes(t => t.upman)
            .Includes(t => t.users)
            .SingleAsync();
        return post;
    }

    [HttpPost]
    public async Task Post([FromBody] SysOrgPost post)
    {
        await postService.InsertAsync(post);
    }

    [HttpPut]
    public async Task Put([FromBody] SysOrgPost post)
    {
        await postService.UpdateAsync(post);
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        await postService.DeleteAsync(ids);
    }
}
using Vben.Base.Sys.Dept;

namespace Vben.Base.Sys.Post;

[Route("sys/post")]
[ApiDescriptionSettings("Sys", Tag = "组织架构-岗位")]
public class SysPostApi(SysPostService postService) : ControllerBase
{
    [HttpGet]
    [SaCheckPermission("sys:post:query")]
    public async Task<dynamic> Get(string name, string depid)
    {
        var pp = XreqUtil.GetPp();
        var expable = Expressionable.Create<SysPost>();
        if (!string.IsNullOrWhiteSpace(name))
        {
            expable.And(t => t.name.Contains(name.Trim()));
        }
        else if(!string.IsNullOrWhiteSpace(depid))
        {
            expable.And(t => t.depid == depid);
        }
        var items = await postService.repo.Context.Queryable<SysPost,SysDept>((t, d)
                => new JoinQueryInfos(JoinType.Left, d.id == t.depid))
            .Where(expable.ToExpression())
            .OrderBy(t => t.ornum)
            .Select((t,d) => new { t.id, t.name, t.notes, t.crtim, t.uptim,depna = d.name })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    [SaCheckPermission("sys:post:query")]
    public async Task<SysPost> GetInfo(string id)
    {
        var post= await postService.repo.Context.Queryable<SysPost>()
            .Where(it => it.id == id)
            .Includes(t => t.crman)
            .Includes(t => t.upman)
            .Includes(t => t.users)
            .SingleAsync();
        return post;
    }

    [HttpPost]
    [SaCheckPermission("sys:post:edit")]
    public async Task Post([FromBody] SysPost post)
    {
        await postService.InsertAsync(post);
    }

    [HttpPut]
    [SaCheckPermission("sys:post:edit")]
    public async Task Put([FromBody] SysPost post)
    {
        await postService.UpdateAsync(post);
    }

    [HttpDelete("{ids}")]
    [SaCheckPermission("sys:post:delete")]
    public async Task Delete(string ids)
    {
        await postService.DeleteAsync(ids);
    }
}
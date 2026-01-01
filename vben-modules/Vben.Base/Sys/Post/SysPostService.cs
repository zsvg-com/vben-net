using Vben.Base.Sys.Dept;

namespace Vben.Base.Sys.Post;

[Service]
public class SysPostService : BaseStrMainService<SysPost>
{
    public SysPostService(SqlSugarRepository<SysPost> repo)
    {
        base.repo = repo;
    }

    public async Task InsertAsync(SysPost post)
    {
        post.id = YitIdHelper.NextId() + "";
        if (post.depid != null)
        {
            var deptTier = await repo.Context.Queryable<SysDept>()
                .Where(it => it.id == post.depid).Select(it => it.tier).SingleAsync();
            post.tier = deptTier  + post.id + "_";
        }
        await repo.Context.InsertNav(post).Include(it => it.users).ExecuteCommandAsync();
        await repo.Context.Insertable(new SysOrg { id = post.id, name = post.name, type = 4 }).ExecuteCommandAsync();
    }
    public async Task UpdateAsync(SysPost post)
    {
        if (post.depid != null)
        {
            var deptTier = await repo.Context.Queryable<SysDept>()
                .Where(it => it.id == post.depid).Select(it => it.tier).SingleAsync();
            post.tier = deptTier + post.id + "_" ;
        }
        await repo.Context.UpdateNav(post)
            .Include(it => it.users,new UpdateNavOptions { ManyToManyIsUpdateA=true })
            .ExecuteCommandAsync();
        await repo.Context.Updateable(new SysOrg { id = post.id, name = post.name, type = 4 })
            .UpdateColumns(it => new { it.name }).ExecuteCommandAsync();
    }
}
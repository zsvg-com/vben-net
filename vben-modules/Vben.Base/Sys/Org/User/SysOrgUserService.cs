using Vben.Base.Sys.Org.Dept;
using Vben.Base.Sys.Org.User.bo;

namespace Vben.Base.Sys.Org.User;

[Service]
public class SysOrgUserService : BaseStrMainService<SysOrgUser>
{
    public SysOrgUserService(SqlSugarRepository<SysOrgUser> repo)
    {
        base.repo = repo;
    }

    public new async Task InsertAsync(SysOrgUser user)
    {
        user.id = YitIdHelper.NextId() + "";
        user.pacod = BCrypt.Net.BCrypt.HashPassword(user.pacod);
        if (user.depid != null)
        {
            var deptTier = await repo.Context.Queryable<SysOrgDept>()
                .Where(it => it.id == user.depid).Select(it => it.tier).SingleAsync();
            user.tier = deptTier + user.id+ "_";
        }
        await repo.Context.Ado.BeginTranAsync();
        await base.InsertAsync(user);
        await repo.Context.Insertable(new SysOrg { id = user.id, name = user.name, type = 2 }).ExecuteCommandAsync();
        await repo.Context.Ado.CommitTranAsync();
    }

    public new async Task UpdateAsync(SysOrgUser user)
    {
        if (user.depid != null)
        {
            var deptTier = await repo.Context.Queryable<SysOrgDept>()
                .Where(it => it.id == user.depid).Select(it => it.tier).SingleAsync();
            user.tier = deptTier+ user.id + "_";
        }
        await repo.Context.Ado.BeginTranAsync();
        await base.UpdateAsync(user);
        await repo.Context.Updateable(new SysOrg { id = user.id, name = user.name, type = 2 })
            .UpdateColumns(it => new { it.name }).ExecuteCommandAsync();
        await repo.Context.Ado.CommitTranAsync();
    }

    public async Task ResetPacod(IdPacodBo bo)
    {
        bo.pacod = BCrypt.Net.BCrypt.HashPassword(bo.pacod);
        string sql = "update sys_org_user set pacod=@pacod where id=@id";
        await repo.Context.Ado.ExecuteCommandAsync(sql, new { id = bo.id, pacod = bo.pacod });
    }

    public async Task ChangeAvtag(IdAvtagBo bo)
    {
        string sql = "update sys_org_user set avtag=@avtag where id=@id";
        await repo.Context.Ado.ExecuteCommandAsync(sql, new { id = bo.id, avtag = bo.avtag });
    }
}
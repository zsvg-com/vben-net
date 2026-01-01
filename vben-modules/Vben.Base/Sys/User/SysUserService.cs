using Vben.Base.Sys.Dept;
using Vben.Base.Sys.User.bo;

namespace Vben.Base.Sys.User;

[Service]
public class SysUserService : BaseStrMainService<SysUser>
{
    public SysUserService(SqlSugarRepository<SysUser> repo)
    {
        base.repo = repo;
    }
    
    public async Task<SysUser> SingleAsync(string id)
    {
        SysUser user=await base.SingleAsync(id);
        if (user.depid != null)
        {
            user.depna = await repo.Context.Queryable<SysOrg>()
                .Where(it => it.id == user.depid).Select(t=>t.name).SingleAsync();
        }
        return user;
    }

    public new async Task InsertAsync(SysUser user)
    {
        user.id = YitIdHelper.NextId() + "";
        user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
        if (user.depid != null)
        {
            var deptTier = await repo.Context.Queryable<SysDept>()
                .Where(it => it.id == user.depid).Select(it => it.tier).SingleAsync();
            user.tier = deptTier + user.id+ "_";
        }
        await repo.Context.Ado.BeginTranAsync();
        await base.InsertAsync(user);
        await repo.Context.Insertable(new SysOrg { id = user.id, name = user.name, type = 2 }).ExecuteCommandAsync();
        await repo.Context.Ado.CommitTranAsync();
    }

    public new async Task UpdateAsync(SysUser user)
    {
        if (user.depid != null)
        {
            var deptTier = await repo.Context.Queryable<SysDept>()
                .Where(it => it.id == user.depid).Select(it => it.tier).SingleAsync();
            user.tier = deptTier+ user.id + "_";
        }
        await repo.Context.Ado.BeginTranAsync();
        await base.UpdateAsync(user);
        await repo.Context.Updateable(new SysOrg { id = user.id, name = user.name, type = 2 })
            .UpdateColumns(it => new { it.name }).ExecuteCommandAsync();
        await repo.Context.Ado.CommitTranAsync();
    }

    public async Task ResetPassword(IdPasswordBo bo)
    {
        bo.password = BCrypt.Net.BCrypt.HashPassword(bo.password);
        string sql = "update sys_user set password=@password where id=@id";
        await repo.Context.Ado.ExecuteCommandAsync(sql, new { id = bo.id, password = bo.password });
    }

    public async Task ChangeAvtag(IdAvtagBo bo)
    {
        string sql = "update sys_user set avtag=@avtag where id=@id";
        await repo.Context.Ado.ExecuteCommandAsync(sql, new { id = bo.id, avtag = bo.avtag });
    }
    
    public async Task UpdateAvatar(string id, string avatar)
    {
        string sql = "update sys_user set avatar=@avatar where id=@id";
        await repo.Context.Ado.ExecuteCommandAsync(sql, new { avatar, id });
    }
}
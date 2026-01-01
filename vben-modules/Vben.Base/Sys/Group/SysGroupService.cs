namespace Vben.Base.Sys.Group;

[Service]
public class SysGroupService : BaseStrMainService<SysGroup>
{
    public SysGroupService(SqlSugarRepository<SysGroup> repo)
    {
        base.repo = repo;
    }

    public async Task InsertAsync(SysGroup group)
    {
        group.id = YitIdHelper.NextId() + "";
        await repo.Context.Ado.BeginTranAsync();
        await repo.Context.InsertNav(group).Include(it => it.members).ExecuteCommandAsync();
        await repo.Context.Insertable(new SysOrg { id = group.id, name = group.name, type = 16 }).ExecuteCommandAsync();
        await repo.Context.Ado.CommitTranAsync();
    }

    public async Task UpdateAsync(SysGroup group)
    {
        await repo.Context.Ado.BeginTranAsync();
        await repo.Context.UpdateNav(group).Include(it => it.members,new UpdateNavOptions { ManyToManyIsUpdateA=true }).ExecuteCommandAsync();
        await repo.Context.Updateable(new SysOrg { id = group.id, name = group.name, type = 16 })
            .UpdateColumns(it => new { it.name }).ExecuteCommandAsync();
        await repo.Context.Ado.CommitTranAsync();
    }
}
namespace Vben.Base.Mon.Job.Log;

[Service]
public class MonJobLogService(SqlSugarRepository<MonJobLog> repo)
{
    public SqlSugarRepository<MonJobLog> repo { get; } = repo;

    public async Task<MonJobLog> SingleAsync(string id)
    {
        return await repo.GetSingleAsync(t => t.id == id);
    }

    public async Task DeleteAsync(string[] ids)
    {
        await repo.Context.Deleteable<MonJobLog>().In(ids).ExecuteCommandAsync();
    }
}
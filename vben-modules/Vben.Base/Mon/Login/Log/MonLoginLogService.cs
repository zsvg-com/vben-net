using Vben.Base.Mon.Log.Login;

namespace Vben.Base.Mon.Login.Log;

[Service]
public class MonLoginLogService : BaseService<MonLoginLog>
{
    public MonLoginLogService(SqlSugarRepository<MonLoginLog> repo)
    {
        this.repo = repo;
    }
}
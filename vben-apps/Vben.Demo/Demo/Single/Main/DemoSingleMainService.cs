namespace Vben.Admin.Demo.Single.Main;

/// <summary>
/// 单一主表服务
/// </summary>
[Service]
public class DemoSingleMainService : BaseMainService<DemoSingleMain>
{
    /// <summary>
    /// 单一主表服务
    /// </summary>
    public DemoSingleMainService(SqlSugarRepository<DemoSingleMain> repo)
    {
        Repo = repo;
    }
}
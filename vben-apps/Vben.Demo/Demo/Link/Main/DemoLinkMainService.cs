namespace Vben.Admin.Demo.Link.Main;

/// <summary>
/// 关联主表服务
/// </summary>
[Service]
public class DemoLinkMainService : BaseMainService<DemoLinkMain>
{
    /// <summary>
    /// 单一主表服务
    /// </summary>
    public DemoLinkMainService(SqlSugarRepository<DemoLinkMain> repo)
    {
        Repo = repo;
        AutoNav = true;
    }

}
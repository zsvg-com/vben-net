namespace Vben.Admin.Demo.Link.Cate;

/// <summary>
/// 单一主表服务
/// </summary>
[Service]
public class DemoLinkCateService : BaseCateService<DemoLinkCate>
{
    /// <summary>
    /// 单一主表服务
    /// </summary>
    public DemoLinkCateService(SqlSugarRepository<DemoLinkCate> repo)
    {
        Repo = repo;
    }
}
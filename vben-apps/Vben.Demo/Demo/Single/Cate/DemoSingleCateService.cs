namespace Vben.Admin.Demo.Single.Cate;

/// <summary>
/// 单一主表服务
/// </summary>
[Service]
public class DemoSingleCateService : BaseCateService<DemoSingleCate>
{
    /// <summary>
    /// 单一主表服务
    /// </summary>
    public DemoSingleCateService(SqlSugarRepository<DemoSingleCate> repo)
    {
        Repo = repo;
    }
}
namespace Vben.Base.Tool.Dict.Cate;

[Service]
public class ToolDictCateService : BaseService<ToolDictCate>
{
    public ToolDictCateService(SqlSugarRepository<ToolDictCate> repo)
    {
        base.repo = repo;
    }
}
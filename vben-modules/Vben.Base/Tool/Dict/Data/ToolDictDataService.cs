namespace Vben.Base.Tool.Dict.Data;

[Service]
public class ToolDictDataService : BaseStrMainService<ToolDictData>
{
    public ToolDictDataService(SqlSugarRepository<ToolDictData> repo)
    {
        base.repo = repo;
    }
}
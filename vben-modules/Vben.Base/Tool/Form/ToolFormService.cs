namespace Vben.Base.Tool.Form;

[Service]
public class ToolFormService : BaseMainService<ToolForm>
{
    public ToolFormService(SqlSugarRepository<ToolForm> repo)
    {
        Repo = repo;
    }
}
using Vben.Base.Tool.Dict.Data;

namespace Vben.Base.Tool.Dict.Main;

[Service]
public class ToolDictMainService : BaseStrMainService<ToolDictMain>
{
    public ToolDictMainService(SqlSugarRepository<ToolDictMain> repo)
    {
        this.repo = repo;
    }

    public async Task<List<ToolDictDataVo>> findData(string code)
    {
        string sql = "select t.dalab,t.daval,t.shsty from tool_dict_data t inner join tool_dict_main m on m.id=t.dicid where m.code = @code and t.avtag="+ Db.True+" order by m.ornum";
        var list = await repo.Context.Ado.SqlQueryAsync<dynamic>(sql,new { code  });
        var voList = new List<ToolDictDataVo>();
        foreach (var item in list)
        {
            var vo = new ToolDictDataVo();
            vo.label=item.dalab;
            vo.value=item.daval;
            vo.shsty=item.shsty;
            voList.Add(vo);
        }
        return voList;
    }
}
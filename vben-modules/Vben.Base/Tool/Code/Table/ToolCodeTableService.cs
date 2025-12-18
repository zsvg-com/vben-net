namespace Vben.Base.Tool.Code.Table;

[Service]
public class ToolCodeTableService : BaseMainService<ToolCodeTable>
{
    public async Task<ToolCodeTable> FindOne(long id)
    {
        var main = await Select(id);
        if (main != null)
        {
            main.fields = await Repo.Context.Queryable<ToolCodeField>()
                .Where(it => it.tabid.Equals(main.id))
                .OrderBy(it=>it.ornum).ToListAsync();
        }
        return main;
    }
    
    public async Task Insertx(ToolCodeTable table)
    {
        table.id = YitIdHelper.NextId() ;
        table.cruid = XuserUtil.getUserId();
        table.crtim = DateTime.Now;
        table.avtag = true;

        await Repo.Context.Insertable(table)
            .AddSubList(it => it.fields.First().tabid)
            .ExecuteCommandAsync();
    }
    
    public async Task Updatex(ToolCodeTable table)
    {
        table.uptim = DateTime.Now;
        table.upuid = XuserUtil.getUserId();
        await Repo.Context.Updateable(table).ExecuteCommandAsync();

        //比对数据库中的option,插入页面上新增的，更新页面上修改的，删除页面上删除的
        List<ToolCodeField> list = await Repo.Context.Queryable<ToolCodeField>()
            .Where(t => t.tabid == table.id).ToListAsync();
        List<ToolCodeField> insertList = new List<ToolCodeField>();
        List<ToolCodeField> updateList = new List<ToolCodeField>();
        List<ToolCodeField> deleteList = new List<ToolCodeField>();
        for (int i = 0; i < list.Count; i++)
        {
            bool flag = false;
            for (int j = 0; j < table.fields.Count; j++)
            {
                if (table.fields[j].id == list[i].id)
                {
                    flag = true;
                    updateList.Add(table.fields[j]); //找到相同的则更新
                    break;
                }
            }

            if (!flag)
            {
                deleteList.Add(list[i]);
            }
        }

        for (int j = 0; j < table.fields.Count; j++)
        {
            bool flag = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (table.fields[j].id == list[i].id)
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                insertList.Add(table.fields[j]);
            }
        }

        await Repo.Context.Deleteable(deleteList).ExecuteCommandAsync();
        await Repo.Context.Updateable(updateList).ExecuteCommandAsync();
        await Repo.Context.Insertable(insertList).ExecuteCommandAsync();
    }
    
    
    public ToolCodeTableService(SqlSugarRepository<ToolCodeTable> repo)
    {
        Repo = repo;
    }
}
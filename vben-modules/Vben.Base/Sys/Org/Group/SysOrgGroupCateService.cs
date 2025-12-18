namespace Vben.Base.Sys.Org.Group;

[Service]
public class SysOrgGroupCateService : BaseStrCateService<SysOrgGroupCate>
{
    public async Task Move(TreeMovePo po)
    {
        var dragCate = await SingleAsync(po.draid);
        List<SysOrgGroupCate> list2 = await repo.Context.Queryable<SysOrgGroupCate>()
            .Where(it => it.ornum > dragCate.ornum)
            .Where(it => it.pid == dragCate.pid)
            .ToListAsync();
        foreach (var cate in list2)
        {
            cate.ornum--;
            await UpdateOrnumAsync(cate);
        }

        if (po.type == "inner")
        {
            dragCate.pid = po.droid;
            int count = await repo.Context.Queryable<SysOrgGroupCate>().Where(t => t.pid == po.droid).CountAsync();
            dragCate.ornum = count + 1;
        }
        else if (po.type == "before")
        {
            var dropCate = await SingleAsync(po.droid);
            dragCate.pid = dropCate.pid;
            dragCate.ornum = dropCate.ornum;
            List<SysOrgGroupCate> list = await repo.Context.Queryable<SysOrgGroupCate>()
                .Where(it => it.ornum > dropCate.ornum)
                .Where(it => it.pid == dropCate.pid)
                .ToListAsync();
            foreach (var cate in list)
            {
                cate.ornum++;
                await UpdateOrnumAsync(cate);
            }
            dropCate.ornum++;
            await UpdateOrnumAsync(dropCate);
        }
        else if (po.type == "after")
        {
            var dropCate = await SingleAsync(po.droid);
            int count = await repo.Context.Queryable<SysOrgGroupCate>().Where(t => t.pid == dropCate.pid)
                .CountAsync();
            if (dragCate.pid == dropCate.pid)
            {
                dragCate.ornum = count;
            }
            else
            {
                dragCate.pid = dropCate.pid;
                dragCate.ornum = count + 1;
            }
        }
        await UpdateAsync(dragCate);
    }

    private async Task UpdateOrnumAsync(SysOrgGroupCate cate)
    {
        await repo.Context.Updateable<SysOrgGroupCate>()
            .SetColumns(it => it.ornum == cate.ornum)
            .Where(it => it.id == cate.id)
            .ExecuteCommandAsync();
    }
    
    public SysOrgGroupCateService(SqlSugarRepository<SysOrgGroupCate> repo)
    {
        this.repo = repo;
    }
}
using Vben.Base.Sys.Org.Post;
using Vben.Base.Sys.Org.User;

namespace Vben.Base.Sys.Org.Dept;

[Service]
public class SysOrgDeptService : BaseStrMainService<SysOrgDept>
{
    public SysOrgDeptService(SqlSugarRepository<SysOrgDept> repo)
    {
        base.repo = repo;
    }

    public new async Task InsertAsync(SysOrgDept dept)
    {
        //处理上级部门与部门层级信息
        dept.id = YitIdHelper.NextId() + "";
        if (dept.pid != null)
        {
            var parentTier = await repo.Context.Queryable<SysOrgDept>()
                .Where(it => it.id == dept.pid).Select(it => it.tier).SingleAsync();
            dept.tier = parentTier  + dept.id + "_";
        }
        else
        {
            dept.tier = "_"+dept.id+"_";
        }

        //事务包裹处理数据库操作
        await repo.Context.Ado.BeginTranAsync();
        await base.InsertAsync(dept);
        //增加sys_org_dept时同时增加sys_org表
        await repo.Context.Insertable(new SysOrg { id = dept.id, name = dept.name, type = 1 }).ExecuteCommandAsync();
        await repo.Context.Ado.CommitTranAsync();
    }

    public new async Task UpdateAsync(SysOrgDept dept)
    {
        //处理上级部门与部门层级信息
        if (dept.pid != null)
        {
            var parentTier = await repo.Context.Queryable<SysOrgDept>()
                .Where(it => it.id == dept.pid).Select(it => it.tier).SingleAsync();
            dept.tier = parentTier  + dept.id + "_";
            var arr = parentTier.Split("_");
            if (arr.Any(str => dept.id == str))
            {
                throw new Exception("父部门不能为自己或者自己的子部门");
            }
        }
        else
        {
            dept.tier = "_" + dept.id + "_";
        }

        var olderTier = await repo.Context.Queryable<SysOrgDept>()
            .Where(it => it.id == dept.id).Select(it => it.tier).SingleAsync();
        //事务包裹处理数据库操作
        await repo.Context.Ado.BeginTranAsync();
        await base.UpdateAsync(dept);
        //修改sys_org_dept时同时修改sys_org表name字段
        await repo.Context.Updateable(new SysOrg { id = dept.id, name = dept.name, type = 1 })
            .UpdateColumns(it => new { it.name }).ExecuteCommandAsync();
        //修改sys_org_dept层级时需要同时修改子部门，部门下的员工，岗位的层级信息
        await DealDeptTier(olderTier, dept.tier, dept.id);
        // 事务测试
        // await repo.Context.Updateable<SysOrg>(new {id="228949221105733",name="张三张三张三张三张三张三张三张三张三张三张三张三张三"})
        //     .UpdateColumns(it => new { it.name}).ExecuteCommandAsync();
        await DealUserTier(olderTier, dept.tier);
        await DealPostTier(olderTier, dept.tier);
        await repo.Context.Ado.CommitTranAsync();
    }

    private async Task DealDeptTier(string oldTier, string newTier, string id)
    {
        var idNameList = await
            repo.Context.Ado.SqlQueryAsync<SidName>(
                "select id,tier as name from sys_org_dept where tier like @oldTier and id<>@id",
                new { id, oldTier = oldTier + "%" });

        var dtList = new List<Dictionary<string, object>>();
        foreach (var idName in idNameList)
        {
            var dt = new Dictionary<string, object>
            {
                {"id", idName.id}, {"tier", idName.name.Replace(oldTier, newTier)}
            };
            dtList.Add(dt);
        }

        await repo.Context.Updateable(dtList).AS("sys_org_dept").WhereColumns("id").ExecuteCommandAsync();
    }

    private async Task DealUserTier(string oldTier, string newTier)
    {
        var idNameList = await
            repo.Context.Ado.SqlQueryAsync<SidName>(
                "select id,tier as name from sys_org_user where tier like @oldTier",
                new { oldTier = oldTier + "%" });

        var list = new List<Dictionary<string, object>>();
        foreach (var idName in idNameList)
        {
            var dt = new Dictionary<string, object>
            {
                {"id", idName.id}, {"tier", idName.name.Replace(oldTier, newTier)}
            };
            list.Add(dt);
        }

        await repo.Context.Updateable(list).AS("sys_org_user").WhereColumns("id").ExecuteCommandAsync();
    }

    private async Task DealPostTier(string oldTier, string newTier)
    {
        var idNameList = await
            repo.Context.Ado.SqlQueryAsync<SidName>(
                "select id,tier as name from sys_org_post where tier like @oldTier",
                new { oldTier = oldTier + "%" });

        var list = new List<Dictionary<string, object>>();
        foreach (var idName in idNameList)
        {
            var dt = new Dictionary<string, object>
            {
                {"id", idName.id}, {"tier", idName.name.Replace(oldTier, newTier)}
            };
            list.Add(dt);
        }

        await repo.Context.Updateable(list).AS("sys_org_post").WhereColumns("id").ExecuteCommandAsync();
    }


    public async Task DeleteAsync(string[] ids)
    {
        //最好是通过外键来控制，部门还有其他地方会用到
        foreach (var id in ids)
        {
            bool flag = await CanDelete(id);
            if (!flag)
            {
                throw new Exception("部门下有子部门，员工或岗位不能删除");
            }
        }
        await repo.Context.Ado.BeginTranAsync();
        await repo.Context.Deleteable<SysOrg>().In(ids).ExecuteCommandAsync();
        await repo.Context.Deleteable<SysOrgDept>().In(ids).ExecuteCommandAsync();
        await repo.Context.Ado.CommitTranAsync();
    }

    private async Task<bool> CanDelete(string id)
    {
        var tier = await repo.Context.Queryable<SysOrgDept>()
            .Where(it => it.id == id).Select(it => it.tier).SingleAsync();

        var deptCount = await
            repo.Context.Queryable<SysOrgDept>().Where(it => it.tier.StartsWith(tier)).CountAsync();
        if (deptCount > 1)
        {
            return false;
        }

        var userCount = await
            repo.Context.Queryable<SysOrgUser>().Where(it => it.tier.StartsWith(tier)).CountAsync();
        if (userCount > 0)
        {
            return false;
        }

        var postCount = await
            repo.Context.Queryable<SysOrgPost>().Where(it => it.tier.StartsWith(tier)).CountAsync();
        if (postCount > 0)
        {
            return false;
        }

        return true;
    }

    private async Task UpdateOrnumAsync(SysOrgDept dept)
    {
        await repo.Context.Updateable<SysOrgDept>()
            .SetColumns(it => it.ornum == dept.ornum)
            .Where(it => it.id == dept.id)
            .ExecuteCommandAsync();
    }

    public async Task PostMove(TreeMovePo po)
    {
        var dragDept = await SingleAsync(po.draid);
        List<SysOrgDept> list2 = await repo.Context.Queryable<SysOrgDept>()
            .Where(it => it.ornum > dragDept.ornum)
            .Where(it => it.pid == dragDept.pid)
            .ToListAsync();
        foreach (var dept in list2)
        {
            dept.ornum--;
            await UpdateOrnumAsync(dept);
        }

        if (po.type == "inner")
        {
            dragDept.pid = po.droid;
            int count = await repo.Context.Queryable<SysOrgDept>().Where(t => t.pid == po.droid).CountAsync();
            dragDept.ornum = count + 1;
        }
        else if (po.type == "before")
        {
            var dropDept = await SingleAsync(po.droid);
            dragDept.pid = dropDept.pid;
            dragDept.ornum = dropDept.ornum;
            List<SysOrgDept> list = await repo.Context.Queryable<SysOrgDept>()
                .Where(it => it.ornum > dropDept.ornum)
                .Where(it => it.pid == dropDept.pid)
                .ToListAsync();
            foreach (var dept in list)
            {
                dept.ornum++;
                await UpdateOrnumAsync(dept);
            }
            dropDept.ornum++;
            await UpdateOrnumAsync(dropDept);
        }
        else if (po.type == "after")
        {
            var dropDept = await SingleAsync(po.droid);
            int count = await repo.Context.Queryable<SysOrgDept>().Where(t => t.pid == dropDept.pid)
                .CountAsync();
            if (dragDept.pid == dropDept.pid)
            {
                dragDept.ornum = count;
            }
            else
            {
                dragDept.pid = dropDept.pid;
                dragDept.ornum = count + 1;
            }
        }
        await UpdateAsync(dragDept);
    }
}
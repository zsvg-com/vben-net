using DbType = SqlSugar.DbType;

namespace Vben.Base.Pub.Org.Root;

[Route("pub/org")]
[ApiDescriptionSettings("Pub", Tag = "组织架构综合查询")]
public class PubOrgApi(SqlSugarRepository<SysOrg> repo) : ControllerBase
{
    //根据部门ID，查询下级所有的部门,岗位,用户
    [HttpGet]
    public async Task<List<SidName>> Get(string depid, int type, string name)
    {
        List<SidName> list = new List<SidName>();
        if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(depid))
        {
            return list;
        }

        if ((type & 1) != 0)
        {
            //部门
            Sqler deptSqler = new Sqler("sys_dept");
            if (!string.IsNullOrEmpty(name))
            {
                deptSqler.addLike("t.name", name);
            }
            else
            {
                deptSqler.addEqual("t.pid", depid);
            }

            Console.WriteLine(deptSqler.getSql());
            List<SidName> deptList = await
                repo.Context.Ado.SqlQueryAsync<SidName>(deptSqler.getSql(), deptSqler.getParams());
            list.AddRange(deptList);
        }
        
        if ((type & 2) != 0)
        {
            //用户
            Sqler userSqler = new Sqler("sys_user");
            if (!string.IsNullOrEmpty(name))
            {
                userSqler.addLike("t.name", name);
            }
            else
            {
                userSqler.addEqual("t.depid", depid);
            }

            List<SidName> userList = await
                repo.Context.Ado.SqlQueryAsync<SidName>(userSqler.getSql(), userSqler.getParams());
            list.AddRange(userList);
        }


        if ((type & 4) != 0)
        {
            //岗位
            Sqler postSqler = new Sqler("sys_post");
            if (!string.IsNullOrEmpty(name))
            {
                postSqler.addLike("t.name", name);
            }
            else
            {
                postSqler.addEqual("t.depid", depid);
            }

            List<SidName> postList = await
                repo.Context.Ado.SqlQueryAsync<SidName>(postSqler.getSql(), postSqler.getParams());
            list.AddRange(postList);
        }
       
        return list;
    }

    //根据组织架构ids获取SysOrg对象数组
    [HttpGet("list")]
    public async Task<List<SidName>> GetList(string ids)
    {
        var dbConfig = MyApp.GetOptions<DbConfig>();
        string sql = "select t.id,t.name from sys_org t ";
        if (ids.Contains(";"))
        {
            ids = "'" + ids.Replace(";", "','") + "'";
            sql += " where t.id in (" + ids + ")";
            if (dbConfig.ConnectionConfigs[0].DbType == DbType.MySql)
            {
                sql += " order by field(id," + ids + ")";
            }
            else if (dbConfig.ConnectionConfigs[0].DbType == DbType.SqlServer)
            {
                sql += " order by CHARINDEX(id,'" + ids.Replace("'", "") + "')";
            }
            else if (dbConfig.ConnectionConfigs[0].DbType == DbType.Oracle)
            {
                sql += " order by INSTR('" + ids.Replace("'", "") + "',id)";
            }
        }
        else
        {
            sql += " where t.id='" + ids + "'";
        }

        Console.WriteLine(sql);
        List<SidName> orgList = await repo.Context.Ado.SqlQueryAsync<SidName>(sql);
        return orgList;
    }

    // List<ZidName> deptList = await 
    //     repo.Context.SqlQueryable<ZidName>("select id,name from sys_dept")
    //         .Where("name like @name",new{name="%"+name+"%"})
    //         .ToListAsync();
}
using Vben.Common.Core.Token;
using Vben.Common.Core.Utils;
using Vben.Common.Sqlsugar.Config;
using Vben.Common.Sqlsugar.Mvc.Dao;
using Vben.Common.Sqlsugar.Mvc.Entity;
using Vben.Common.Sqlsugar.Mvc.Pojo;

namespace Vben.Common.Sqlsugar.Mvc.Service;

//分类Service基类，提供分类Entity增删改查的通用方法
public class BaseCateService<TEntity> where TEntity : BaseCateEntity, new()
{
    public required SqlSugarRepository<TEntity> Repo { get; init; }
    
    public async Task<List<Ltree>> findTreeList(Sqler sqler,long id)
    {
        sqler.addSelect("t.pid");
        sqler.addWhere("t.avtag = "+Db.True);
        if (id != 0) {
            sqler.addWhere("t.tier not like @tier", "@tier","%" + id + "%");
        }
        sqler.addOrder("t.ornum");
        List<Ltree> list = await Repo.Context.Ado.SqlQueryAsync<Ltree>(sqler.getSql(),sqler.getSugarParams());
        return TreeUtils.BuildLtree(list);
    }

    //详情查询
    public async Task<TEntity> Select(long id)
    {
        var main = await Repo.GetSingleAsync(t => t.id == id);
        string sql="select name from sys_org where id=@id";
        if (main.cruid != null)
        {
            main.cruna = Repo.Context.Ado.SqlQuerySingle<string>(sql, new { id=main.cruid });
        }
        if (main.upuid != null)
        {
            main.upuna = Repo.Context.Ado.SqlQuerySingle<string>(sql, new { id=main.upuid });
        }
        return main;
    }

    //新增
    public async Task<long> Insert(TEntity cate)
    {
        if (cate.ornum == 0)
        {
            int count = await Repo.Context.Queryable<TEntity>().Where(t => t.pid == cate.pid).CountAsync();
            cate.ornum = count + 1;
        }
        if (cate.id==0)
        {
            cate.id = YitIdHelper.NextId();
        }
        cate.crtim = DateTime.Now;
        cate.cruid = LoginHelper.UserId;
        cate.uptim = cate.crtim;
        cate.upuid = cate.cruid;
        // cate.avtag = true;
        if (cate.pid == 0)
        {
            cate.tier ="_"+cate.id+"_";
        }
        else
        {
            TEntity parent = await Repo.GetSingleAsync(t => t.id == cate.pid);
            cate.tier=parent.tier + cate.id + "_";
        }
        await Repo.InsertAsync(cate);
        return cate.id;
    }

    //更新
    public async Task<long> Update(TEntity cate)
    {
        cate.uptim = DateTime.Now;
        cate.upuid = LoginHelper.UserId;
        await Repo.UpdateAsync(cate);
        return cate.id;
    }
    
    //级联更新
    public async Task<long> Update(TEntity cate, string table) {
       
        cate.uptim = DateTime.Now;
        cate.upuid = LoginHelper.UserId;
        string ptSql="select pid,tier from "+table+" where id=@id";
        LpidTier oldCate = await Repo.Context.Ado.SqlQuerySingleAsync<LpidTier>(ptSql,new { cate.id });
        string oldTier = oldCate.tier;
        if (oldCate.pid==cate.pid) {
            cate.tier = oldTier;
            await Repo.UpdateAsync(cate);
        } else {
            string newTier = "_"+ cate.id + "_";
            if (cate.pid != 0L) {
                TEntity newParentCate = await Repo.GetSingleAsync(t => t.id == cate.pid);
                newTier = newParentCate.tier + cate.id + "_";
            }
            cate.tier=newTier;
            await Repo.UpdateAsync(cate);
            //更新子节点
            string sql = "select id,tier as name from " + table + " where tier like @oldTier and id<>@id";
            var list = await Repo.Context.Ado.SqlQueryAsync<SidName>(sql, new { cate.id, oldTier =  oldTier + "%" });
            var dtList = new List<Dictionary<string, object>>();
            foreach (var idName in list)
            {
                var dt = new Dictionary<string, object>
                {
                    {"id", idName.id}, {"tier", idName.name.Replace(oldTier, newTier)}
                };
                dtList.Add(dt);
            }
            await Repo.Context.Updateable(dtList).AS(table).WhereColumns("id").ExecuteCommandAsync();
        }
        return cate.id;
    }
    

    //删除
    public async Task<int> Delete(string ids)
    {
        var idArr = ids.Split(",");
        await Repo.Context.Deleteable<TEntity>().In(idArr).ExecuteCommandAsync();
        return idArr.Length;
    }
    
    /**
     * 节点移动
     * @param bo
     * @param table
     * draid 拖动节点ID
     * droid 放下时目标节点ID
     */
    public async Task Move(Lmove bo, String table) {
        TEntity dragCate = await Repo.GetSingleAsync(t => t.id == bo.draid);
        //节点移动后，节点同级的下方节点ornum--
        string sql = "select id,ornum from " + table + " where  ornum>@ornum and pid=@pid";
        var list2 = await Repo.Context.Ado.SqlQueryAsync<SidOrnum>(sql, new { dragCate.ornum,dragCate.pid });
        var dtList2 = new List<Dictionary<string, object>>();
        foreach (var idOrnum in list2)
        {
            var dt = new Dictionary<string, object>
            {
                {"id", idOrnum.id}, {"ornum", idOrnum.ornum--}
            };
            dtList2.Add(dt);
        }
        await Repo.Context.Updateable(dtList2).AS(table).WhereColumns("id").ExecuteCommandAsync();
        
        if ("inner"==bo.type) {
            int count = await Repo.Context.Queryable<TEntity>().Where(t => t.pid == bo.droid).CountAsync();
            dragCate.pid= bo.droid;;
            dragCate.ornum = count + 1;
        } else if ("before"==bo.type) {
            TEntity dropCate = await Repo.GetSingleAsync(t => t.id == bo.droid);
            int dropCateOrnum = dropCate.ornum;
            //目标节点与同级下方节点ornum++
            string sql3 = "select id,ornum from " + table + " where  ornum>=@ornum and pid=@pid";
            var list3 = await Repo.Context.Ado.SqlQueryAsync<SidOrnum>(sql3, new { dropCate.ornum,dropCate.pid });
            var dtList3 = new List<Dictionary<string, object>>();
            foreach (var idOrnum in list3)
            {
                var dt = new Dictionary<string, object>
                {
                    {"id", idOrnum.id}, {"ornum", idOrnum.ornum++}
                };
                dtList3.Add(dt);
            }
            await Repo.Context.Updateable(dtList3).AS(table).WhereColumns("id").ExecuteCommandAsync();
            
            dragCate.pid = dropCate.pid;
            dragCate.ornum=dropCateOrnum;
        } else if ("after"==bo.type) {
            TEntity dropCate = await Repo.GetSingleAsync(t => t.id == bo.droid);
            int dropCateOrnum = dropCate.ornum;
            //目标节点同级下方节点ornum++
            string sql4 = "select id,ornum from " + table + " where  ornum>@ornum and pid=@pid";
            var list4 = await Repo.Context.Ado.SqlQueryAsync<SidOrnum>(sql4, new { dropCate.ornum,dropCate.pid });
            var dtList4 = new List<Dictionary<string, object>>();
            foreach (var idOrnum in list4)
            {
                var dt = new Dictionary<string, object>
                {
                    {"id", idOrnum.id}, {"ornum", idOrnum.ornum++}
                };
                dtList4.Add(dt);
            }
            await Repo.Context.Updateable(dtList4).AS(table).WhereColumns("id").ExecuteCommandAsync();
            dragCate.pid=dropCate.pid;
            dragCate.ornum=dropCateOrnum+1;
        }
        await Update(dragCate, table);
    }
}
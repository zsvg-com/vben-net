using Vben.Common.Core.Token;
using Vben.Common.Core.Utils;
using Vben.Common.Sqlsugar.Config;
using Vben.Common.Sqlsugar.Mvc.Entity;

namespace Vben.Common.Sqlsugar.Mvc.Service;

//主数据Service基类，提供主数据Entity增删改查的通用方法
public class BaseMainService<TEntity> where TEntity : BaseMainEntity, new()
{
    public required SqlSugarRepository<TEntity> Repo { get; init; }
    
    protected bool AutoNav { get; init; } 
   
    public async Task<TEntity> SelectWithNav(long id)
    {
        // 自动导航目前还不支持导航表排序配置
        var main= await Repo.Context.Queryable<TEntity>().IncludesAllFirstLayer().SingleAsync(t => t.id == id);
        if (main == null) throw new Exception("记录不存在");
        await SelectCuInfo(main);
        return main;
    }
    
    //查询单个实体详细信息
    public async Task<TEntity> Select(long id)
    {
        if (AutoNav)
        {
            return await SelectWithNav(id);
        }
        var main = await Repo.GetSingleAsync(t => t.id == id);
        if (main == null) throw new Exception("记录不存在");
        await SelectCuInfo(main);
        return main;
    }

    /**
     * 后面加缓存
     */
    public async Task SelectCuInfo(TEntity main)
    {
        string sql="select name from sys_org where id=@id";
        if (main.cruid != null)
        {
            main.cruna =await Repo.Context.Ado.SqlQuerySingleAsync<string>(sql, new { id=main.cruid });
        }
        if (main.upuid != null)
        {
            main.upuna =await Repo.Context.Ado.SqlQuerySingleAsync<string>(sql, new { id=main.upuid });
        }
    }

    //新增
    public async Task<long> InsertWithNav(TEntity entity)
    {
        entity.id = YitIdHelper.NextId(); 
        entity.cruid = LoginHelper.UserId;
        entity.crtim = DateTime.Now;
        entity.avtag = true;
        entity.uptim = entity.crtim;
        entity.upuid =  entity.cruid;
        // await repo.InsertAsync(entity);
        // await repo.Context.InsertNav(entity).IncludesAllFirstLayer().ExecuteCommandAsync();
        await Repo.Context.InsertNav(entity).IncludesAllFirstLayer().ExecuteCommandAsync();
        return entity.id;
    }
    
    //新增
    public async Task<long> Insert(TEntity entity)
    {
        if (AutoNav)
        {
            return await InsertWithNav(entity);
        }
        entity.id = YitIdHelper.NextId(); 
        entity.cruid = LoginHelper.UserId;
        entity.crtim = DateTime.Now;
        entity.avtag = true;
        entity.uptim = entity.crtim;
        entity.upuid =  entity.cruid;
        await Repo.InsertAsync(entity);
        return entity.id;
    }

    //修改
    public async Task<long> UpdateWithNav(TEntity entity)
    {
        entity.uptim = DateTime.Now;
        entity.upuid = LoginHelper.UserId;
        await Repo.Context.UpdateNav(entity).IncludesAllFirstLayer().ExecuteCommandAsync();
        return entity.id;
    }
    
    public async Task<long> Update(TEntity entity)
    {
        if (AutoNav)
        {
            return await UpdateWithNav(entity);
        }
        entity.uptim = DateTime.Now;
        entity.upuid = LoginHelper.UserId;
        await Repo.UpdateAsync(entity);
        return entity.id;
    }
    
    //删除
    public async Task<int> DeleteWithNav(string ids)
    {
        var idArr = ids.Split(",");
        foreach (var id in idArr)
        {
            var main = await Repo.GetSingleAsync(t => t.id.Equals(id));
            await Repo.Context.DeleteNav(main).IncludesAllFirstLayer().ExecuteCommandAsync();
        }
        return idArr.Length;
    }

    //删除
    public async Task<int> Delete(string ids)
    {
        if (AutoNav)
        {
            return await DeleteWithNav(ids);
        }
        var idArr = ids.Split(",");
        await Repo.Context.Deleteable<TEntity>().In(idArr).ExecuteCommandAsync();
        return idArr.Length;
    }
    
}
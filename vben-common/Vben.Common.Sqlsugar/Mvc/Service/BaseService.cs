using Vben.Common.Sqlsugar.Config;
using Vben.Common.Sqlsugar.Mvc.Entity;

namespace Vben.Common.Sqlsugar.Mvc.Service;

//简单表Service基类，提供简单Entity增删改查的通用方法
public class BaseService<TEntity> where TEntity : BaseEntity, new()
{

    public SqlSugarRepository<TEntity> repo { get; set; }

    //查询单个实体详细信息
    public async Task<TEntity> SingleAsync(string id)
    {
        return await repo.GetSingleAsync(t => t.id == id);
    }

    //新增
    public async Task InsertAsync(TEntity entity)
    {
        if (string.IsNullOrEmpty(entity.id))
        {
            entity.id = YitIdHelper.NextId() + "";
        }

        await repo.InsertAsync(entity);
    }

    //修改
    public async Task UpdateAsync(TEntity entity)
    {
        await repo.UpdateAsync(entity);
    }

    //删除
    public async Task DeleteAsync(string ids)
    {
        var idArr = ids.Split(",");
        await repo.Context.Deleteable<TEntity>().In(idArr).ExecuteCommandAsync();
    }
}
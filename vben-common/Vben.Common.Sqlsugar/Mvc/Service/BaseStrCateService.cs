using Vben.Common.Core.Utils;
using Vben.Common.Sqlsugar.Config;
using Vben.Common.Sqlsugar.Mvc.Entity;

namespace Vben.Common.Sqlsugar.Mvc.Service;

//分类Service基类，提供分类Entity增删改查的通用方法
public class BaseStrCateService<TEntity> where TEntity : BaseStrCateEntity, new()
{
    public SqlSugarRepository<TEntity> repo { get; set; }

    //查询单个实体详细信息
    public async Task<TEntity> SingleAsync(string id)
    {
        var main = await repo.GetSingleAsync(t => t.id == id);
        if (main.crmid != null)
        {
            main.crman = await repo.Context.Queryable<SysOrg>()
                .Where(it => it.id == main.crmid).SingleAsync();
        }

        if (main.upmid != null)
        {
            main.upman = await repo.Context.Queryable<SysOrg>()
                .Where(it => it.id == main.upmid).SingleAsync();
        }

        return main;
    }

    //新增
    public async Task<string> InsertAsync(TEntity entity)
    {
        if (string.IsNullOrEmpty(entity.id))
        {
            entity.id = YitIdHelper.NextId() + "";
        }

        entity.crmid = XuserUtil.getUserId();
        entity.crtim = DateTime.Now;
        entity.avtag = true;
        await repo.InsertAsync(entity);
        return entity.id;
    }

    //更新
    public async Task<string> UpdateAsync(TEntity entity)
    {
        entity.uptim = DateTime.Now;
        entity.upmid = XuserUtil.getUserId();
        await repo.UpdateAsync(entity);
        return entity.id;
    }

    //删除
    public async Task DeleteAsync(string ids)
    {
        var idArr = ids.Split(",");
        await repo.Context.Deleteable<TEntity>().In(idArr).ExecuteCommandAsync();
    }
}
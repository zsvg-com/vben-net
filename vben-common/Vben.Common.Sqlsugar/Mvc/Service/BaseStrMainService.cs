using Vben.Common.Core.Token;
using Vben.Common.Core.Utils;
using Vben.Common.Sqlsugar.Config;
using Vben.Common.Sqlsugar.Mvc.Entity;

namespace Vben.Common.Sqlsugar.Mvc.Service;

//主数据Service基类，提供主数据Entity增删改查的通用方法
public class BaseStrMainService<TEntity> where TEntity : BaseStrMainEntity, new()
{
    public SqlSugarRepository<TEntity> repo { get; set; }

    //查询单个实体详细信息
    public async Task<TEntity> SingleAsync(string id)
    {
        var main = await repo.GetSingleAsync(t => t.id == id);
        // if (main == null) throw Oops.Oh(ErrorCode.D1002);
        if (main == null) throw new Exception("记录不存在");

        if (main.cruid != null)
        {
            main.crman = await repo.Context.Queryable<SysOrg>()
                .Where(it => it.id == main.cruid).SingleAsync();
        }

        if (main.upuid != null)
        {
            main.upman = await repo.Context.Queryable<SysOrg>()
                .Where(it => it.id == main.upuid).SingleAsync();
        }

        return main;
    }

    // public async Task<dynamic> GetPageList(dynamic sugarQueryable)
    // {
    //     var pp= XreqUtil.GetPp(); 
    //     var items =await sugarQueryable.ToPageListAsync(pp.page, pp.pageSize, pp.total);
    //     return RestPageResult.Build(pp.total.Value, items);
    // }

    //新增
    public async Task<string> InsertAsync(TEntity entity)
    {
        if (string.IsNullOrEmpty(entity.id))
        {
            entity.id = YitIdHelper.NextId() + ""; //雪花ID
        }

        entity.cruid = LoginHelper.UserId;
        entity.crtim = DateTime.Now;

        await repo.InsertAsync(entity);
        return entity.id;
    }

    //修改
    public async Task<string> UpdateAsync(TEntity entity)
    {
        entity.uptim = DateTime.Now;
        entity.upuid = LoginHelper.UserId;
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
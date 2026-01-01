using Vben.Common.Core.Token;

namespace Vben.Base.Sys.Rece;

[Service]
public class SysReceService 
{
    
    public readonly SqlSugarRepository<SysRece> _repo;

    public SysReceService(SqlSugarRepository<SysRece> repo)
    {
        _repo = repo;
    }
    
    public async Task update(List<SysRece> reces)
    {
        string userId = LoginHelper.UserId;

        List<string> orgidList = new List<string>();
        foreach (var rece in reces)
        {
            rece.oid = rece.id;
            rece.id = YitIdHelper.NextId() + "";
            rece.useid = userId;
            rece.uptim = DateTime.Now;
            orgidList.Add(rece.oid);
        }

        //数据库删除本次已传的记录
        await _repo.Context.Deleteable<SysRece>().Where("useid=@useid", new { useid = userId })
            .Where("oid in (@oid)", new { oid = orgidList.ToArray() }).ExecuteCommandAsync();

        //删除当前数据库最近10次前的数据
        RefAsync<int> total = 0;
        var items = await _repo.Context.Queryable<SysRece>()
            .Where(it => it.useid == userId)
            .OrderBy(u => u.uptim, OrderByType.Desc)
            .Select((t) => t.id)
            .ToPageListAsync(2, 10, total);
        if (items.Count > 0)
        {
            await _repo.Context.Deleteable<SysRece>().In(items.ToArray()).ExecuteCommandAsync();
        }
        //插入本次使用的记录
        await _repo.InsertRangeAsync(reces);
    }

}
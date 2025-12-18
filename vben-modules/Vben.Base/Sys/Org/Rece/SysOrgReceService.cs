namespace Vben.Base.Sys.Org.Rece;

[Service]
public class SysOrgReceService 
{
    
    public readonly SqlSugarRepository<SysOrgRece> _repo;

    public SysOrgReceService(SqlSugarRepository<SysOrgRece> repo)
    {
        _repo = repo;
    }
    
    public async Task update(List<SysOrgRece> reces)
    {
        string userId = XuserUtil.getUserId();

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
        await _repo.Context.Deleteable<SysOrgRece>().Where("useid=@useid", new { useid = userId })
            .Where("oid in (@oid)", new { oid = orgidList.ToArray() }).ExecuteCommandAsync();

        //删除当前数据库最近10次前的数据
        RefAsync<int> total = 0;
        var items = await _repo.Context.Queryable<SysOrgRece>()
            .Where(it => it.useid == userId)
            .OrderBy(u => u.uptim, OrderByType.Desc)
            .Select((t) => t.id)
            .ToPageListAsync(2, 10, total);
        if (items.Count > 0)
        {
            await _repo.Context.Deleteable<SysOrgRece>().In(items.ToArray()).ExecuteCommandAsync();
        }
        //插入本次使用的记录
        await _repo.InsertRangeAsync(reces);
    }

}
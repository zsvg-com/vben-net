using NewLife.Caching;

namespace Vben.Base.Mon.Cache;
[Route("mon/cache")]
[ApiDescriptionSettings("Mon", Tag = "缓存监控")]
public class MonCacheApi : ControllerBase
{

    [HttpGet]
    public CacheInfoVo GetInfo(string id)
    {
        var redis = MyApp.GetRequiredService<Redis>();
        IDictionary<string, string> dict= redis.GetInfo(true);
        CacheInfoVo infoVo = new CacheInfoVo();
        infoVo.info = dict;
        foreach (var item in dict)
        {
            if (item.Key.StartsWith("cmdstat_"))
            {
                var value= item.Value.Split("calls=")[1].Split(",")[0];
                infoVo.commandStats.Add(new CacheCommandVo{name=item.Key.Substring("cmdstat_".Length),value=value});
            }
            else if (item.Key=="db0")
            {
                infoVo.dbSize = int.Parse(item.Value.Split("keys=")[1].Split(",")[0]);
            }
        }
        return infoVo;
    }

}
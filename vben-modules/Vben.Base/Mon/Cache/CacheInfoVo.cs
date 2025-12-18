namespace Vben.Base.Mon.Cache;

public class CacheInfoVo
{
    
    public IDictionary<string, string> info { get; set; }
    
    
    public int dbSize { get; set; }
    
    
    public List<CacheCommandVo> commandStats { get; set; } = new ();
    
}
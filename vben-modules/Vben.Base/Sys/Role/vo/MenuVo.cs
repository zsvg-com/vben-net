namespace Vben.Base.Sys.Role.vo;

public class MenuVo
{
    public long id { get; set; }

    public string name { get; set; }
    
    public string icon { get; set; }
    
    public string type { get; set; }
    
    public long pid { get; set; }
    
    public List<ApiVo> apis { get; set; }= new ();

}
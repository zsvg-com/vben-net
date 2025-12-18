namespace Vben.Base.Sys.Security.Pojo;

//前台返回菜单meta信息
public class Zmeta
{
    public string title { get; set; }

    public string affix { get; set; }

    public string icon { get; set; }

    public int orderNo { get; set; }

    public bool isHide { get; set; }

    public bool isIframe { get; set; } = false;

    public string isLink { get; set; }

    public bool isKeepAlive { get; set; }

    public Zmeta()
    {
    }

}
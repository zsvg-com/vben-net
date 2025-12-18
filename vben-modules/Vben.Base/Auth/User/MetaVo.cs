namespace Vben.Base.Auth.User;

public class MetaVo
{
    /*
     * 设置该路由在侧边栏和面包屑中展示的名字
     */
    public string title { get; set; }

    /**
    * 设置该路由的图标，对应路径src/assets/icons/svg
    */
    public string icon { get; set; }

    /**
    * 设置为true，则不会被 <keep-alive>缓存
    */
    public bool noCache { get; set; }

    /**
    * 内链地址（http(s)://开头）
    */
    public string link { get; set; }
    
}
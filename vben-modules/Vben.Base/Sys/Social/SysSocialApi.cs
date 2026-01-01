namespace Vben.Base.Sys.Social;


[Route("sys/social")]
[ApiDescriptionSettings("Sys", Tag = "社交登录")]
public class SysSocialApi : ControllerBase
{
    [HttpGet("list")]
    public  dynamic GetList()
    {
        return new List<SysSocial>();
    }
}
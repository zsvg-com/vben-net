namespace Vben.Base.Sys.Security.Pojo;

//数据库用户信息，登录时用到
public class UserDo
{
    public string id { get; set; }

    public string name { get; set; }

    public string password { get; set; }

    public bool catag { get; set; }

    public string tier { get; set; }

    public string username { get; set; }

    public string monum { get; set; }//手机号

    public string label { get; set; }//账号标签
    public string type { get; set; }//账号类型

    public string depid { get; set; }//部门id，协同用户则是公司id

    public string depna { get; set; }//部门名称，协同用户则是公司名称

    public string relog { get; set; }//前台通知是否查看过的标记
}
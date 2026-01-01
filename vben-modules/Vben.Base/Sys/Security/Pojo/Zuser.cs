namespace Vben.Base.Sys.Security.Pojo;

//用户pojo，前台与后台都会用到，前台用于获取用户信息，后台存在redis里，用于获取用户信息及权限校验
public class Zuser
{
    public string id { get; set; }

    public string name { get; set; }

    public string username { get; set; }

    public string monum { get; set; }//手机号

    public string label { get; set; }//账号标签
    public string type { get; set; }//账号类型

    public string depid { get; set; }//部门id，协同用户则是公司id

    public string depna { get; set; }//部门名称，协同用户则是公司名称

    public string relog { get; set; }//前台通知是否查看过的标记

    private bool isAdmin;

    public bool IsAdmin
    {
        get
        {
            if ("sa" == username || "admin" == username || "vben" == username)
            {
                return true;
            }
            return false;
        }
        set => isAdmin = value;
    }

    public string perms { get; set; } //权限集,用于验证URL权限 比较下哪个方式快0

    // public long[] permArr{ get; set; }//权限集,用于验证URL权限 比较下哪个方式快1

    // public List<string> permList{ get; set; }//权限集,用于验证URL权限 比较下哪个方式快2

    public string conds { get; set; } //组织架构集，用户ID，所有上级部门ID,岗位ID,群组ID

    public Zuser()
    {
    }

    public Zuser(string id, string name, string username)
    {
        this.id = id;
        this.name = name;
        this.username = username;
    }

    public Zuser(UserDo userDo)
    {
        this.id = userDo.id;
        this.name = userDo.name;
        this.username = userDo.username;
        this.monum = userDo.monum;
        this.label = userDo.label;
        this.type = userDo.type;
        this.relog = userDo.relog;
        this.depid = userDo.depid;
        this.depna = userDo.depna;
    }
}
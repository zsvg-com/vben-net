namespace Vben.Base.Sys.Security.Pojo;

//前台返回菜单
public class Zmenu
{

    public string id { get; set; }

    public string pid { get; set; }

    public string perm { get; set; }

    public string path { get; set; }

    public string name { get; set; }
    public string porid { get; set; }

    public string type { get; set; }

    public string component { get; set; }

    public Zmeta meta { get; set; }

    public string redirect { get; set; }

    public List<Zmenu> children { get; set; }
}
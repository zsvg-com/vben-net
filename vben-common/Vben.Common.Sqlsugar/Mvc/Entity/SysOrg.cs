namespace Vben.Common.Sqlsugar.Mvc.Entity;

[SugarTable("sys_org", TableDescription = "组织架构表")]
[Description("组织架构表")]
public class SysOrg
{
    [SugarColumn(ColumnDescription = "ID", IsPrimaryKey = true, Length = 36)]
    public string id { get; set; }

    [SugarColumn(ColumnDescription = "名称", IsNullable = true, Length = 128)]
    public string name { get; set; }

    //1为机构,2为部门,4为岗位,8为用户,16为群组,32为角色
    [SugarColumn(ColumnDescription = "类型", IsNullable = true)]
    public int type { get; set; }


    public SysOrg()
    {
    }

    public SysOrg(string id)
    {
        this.id = id;
    }

    public SysOrg(string id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public SysOrg(string id, string name, int type)
    {
        this.id = id;
        this.name = name;
        this.type = type;
    }
}
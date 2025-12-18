namespace Vben.Base.Sys.Org.Post;

[SugarTable("sys_org_post", TableDescription = "组织架构-岗位")]
[Description("组织架构-岗位")]
public class SysOrgPost : BaseStrMainEntity
{
    [SugarColumn(ColumnDescription = "部门ID", IsNullable = true, Length = 36)]
    public string depid { get; set; }

    [SugarColumn(ColumnDescription = "标签", IsNullable = true, Length = 32)]
    public string label { get; set; }

    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }

    [Navigate(typeof(SysOrgPostOrg), nameof(SysOrgPostOrg.pid), nameof(SysOrgPostOrg.oid))]
    public List<SysOrg> users { get; set; }
    
    [SugarColumn(ColumnDescription = "层级", IsNullable = true, Length = 512)]
    public string tier { get; set; }
    
}
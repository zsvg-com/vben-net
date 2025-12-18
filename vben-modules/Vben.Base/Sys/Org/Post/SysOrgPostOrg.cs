namespace Vben.Base.Sys.Org.Post;

[SugarTable("sys_org_post_org", TableDescription = "岗位员工关系表")]
public class SysOrgPostOrg
{
    [SugarColumn(ColumnDescription = "岗位ID", IsNullable = true)]
    public string pid { get; set; }

    [SugarColumn(ColumnDescription = "用户ID", IsNullable = true)]
    public string oid { get; set; }
}
namespace Vben.Base.Sys.Org.Group;

[SugarTable("sys_org_group_org", TableDescription = "组织架构群组成员关系表")]
public class SysOrgGroupOrg
{
    [SugarColumn(ColumnDescription = "群组ID", IsNullable = true)]
    public string gid { get; set; }

    [SugarColumn(ColumnDescription = "成员ID", IsNullable = true)]
    public string oid { get; set; }
}
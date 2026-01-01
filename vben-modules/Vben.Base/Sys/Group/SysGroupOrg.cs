namespace Vben.Base.Sys.Group;

[SugarTable("sys_group_org", TableDescription = "组织架构群组成员关系表")]
public class SysGroupOrg
{
    [SugarColumn(ColumnDescription = "群组ID", IsNullable = true)]
    public string gid { get; set; }

    [SugarColumn(ColumnDescription = "成员ID", IsNullable = true)]
    public string oid { get; set; }
}
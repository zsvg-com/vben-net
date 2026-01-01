namespace Vben.Base.Sys.Role;

[SugarTable("sys_role_org", TableDescription = "权限角色与组织架构关联表")]
public class SysRoleToOrg
{
    [SugarColumn(ColumnDescription = "角色ID", IsNullable = true)]
    public long rid { get; set; }

    [SugarColumn(ColumnDescription = "组织架构ID", IsNullable = true, Length = 36)]
    public string oid { get; set; }
}
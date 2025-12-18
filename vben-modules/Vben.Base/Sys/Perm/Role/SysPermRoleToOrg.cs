namespace Vben.Base.Sys.Perm.Role;

[SugarTable("sys_perm_role_org", TableDescription = "权限角色与组织架构关联表")]
public class SysPermRoleToOrg
{
    [SugarColumn(ColumnDescription = "角色ID", IsNullable = true)]
    public long rid { get; set; }

    [SugarColumn(ColumnDescription = "组织架构ID", IsNullable = true, Length = 36)]
    public string oid { get; set; }
}
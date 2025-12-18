namespace Vben.Base.Sys.Perm.Role;

[SugarTable("sys_perm_role_api", TableDescription = "权限角色与接口关联表")]
public class SysPermRoleToApi
{
    [SugarColumn(ColumnDescription = "角色ID", IsNullable = true)]
    public long rid { get; set; }

    [SugarColumn(ColumnDescription = "接口ID", IsNullable = true)]
    public long aid { get; set; }
}
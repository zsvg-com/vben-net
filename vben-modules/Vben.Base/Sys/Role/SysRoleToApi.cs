namespace Vben.Base.Sys.Role;

[SugarTable("sys_role_api", TableDescription = "权限角色与接口关联表")]
public class SysRoleToApi
{
    [SugarColumn(ColumnDescription = "角色ID", IsNullable = true)]
    public long rid { get; set; }

    [SugarColumn(ColumnDescription = "接口ID", IsNullable = true)]
    public long aid { get; set; }
}
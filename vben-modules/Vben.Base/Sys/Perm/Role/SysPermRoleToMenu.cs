namespace Vben.Base.Sys.Perm.Role;

[SugarTable("sys_perm_role_menu", TableDescription = "权限角色与菜单关联表")]
public class SysPermRoleToMenu
{
    [SugarColumn(ColumnDescription = "角色ID", IsNullable = true)]
    public long rid { get; set; }

    [SugarColumn(ColumnDescription = "菜单ID", IsNullable = true)]
    public long mid { get; set; }
}
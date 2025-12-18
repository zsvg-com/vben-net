using Vben.Base.Sys.Perm.Api;
using Vben.Base.Sys.Perm.Menu;

namespace Vben.Base.Sys.Perm.Role;

/// <summary>
/// 权限角色
/// </summary>
[SugarTable("sys_perm_role", TableDescription = "权限角色")]
[Description("权限角色")]
public class SysPermRole
{
    
    /// <summary>
    /// 主键Id
    /// </summary>
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public long id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", IsNullable = true, Length = 32)]
    public string name { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 255)]
    public string notes { get; set; }
    
    /// <summary>
    /// 可用标记
    /// </summary>
    [SugarColumn(ColumnDescription = "可用标记", IsNullable = true)]
    public bool avtag { get; set; } = true;
    
    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }
    
    /// <summary>
    /// 角色类型
    /// </summary>
    [SugarColumn(ColumnDescription = "角色类型", IsNullable = true)]
    public int type { get; set; }
    
    /// <summary>
    /// 数据权限
    /// </summary>
    [SugarColumn(ColumnDescription = "数据权限", IsNullable = true)]
    public int scope { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间", IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public DateTime? crtim { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
    public DateTime? uptim { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(cruid))]
    public SysOrg crman { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    [SugarColumn(ColumnName = "cruid", ColumnDescription = "创建者Id", IsNullable = true, IsOnlyIgnoreUpdate = true,
        Length = 36)]
    public string cruid { get; set; }

    /// <summary>
    /// 修改者
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(upuid))]
    public SysOrg upman { get; set; }

    /// <summary>
    /// 修改者Id
    /// </summary>
    [SugarColumn(ColumnName = "upuid", ColumnDescription = "修改者Id", IsNullable = true, Length = 36)]
    public string upuid { get; set; }

    [Navigate(typeof(SysPermRoleToOrg), nameof(SysPermRoleToOrg.rid), nameof(SysPermRoleToOrg.oid))]
    public List<SysOrg> orgs { get; set; }
    
    [Navigate(typeof(SysPermRoleToMenu), nameof(SysPermRoleToMenu.rid), nameof(SysPermRoleToMenu.mid))]
    public List<SysPermMenu> menus { get; set; }
    
    [Navigate(typeof(SysPermRoleToApi), nameof(SysPermRoleToApi.rid), nameof(SysPermRoleToApi.aid))]
    public List<SysPermApi> apis { get; set; }

}
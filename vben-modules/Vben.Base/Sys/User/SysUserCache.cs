namespace Vben.Base.Sys.User;

[SugarTable("sys_user_cache", TableDescription = "用户缓存表")]
[Description("用户缓存表")]
public class SysUserCache
{
    /// <summary>
    /// 主键Id
    /// </summary>
    /// <example></example>
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true, Length = 36)]
    public string id { get; set; }

    /// <summary>
    /// 组织架构集
    /// </summary>
    /// <example></example>
    //组织架构集，用户ID，所有上级部门ID,岗位ID,群组ID
    [SugarColumn(ColumnDescription = "组织架构集", IsNullable = true, Length = 2000)]
    public string conds { get; set; }

    /// <summary>
    /// 后台所有权限集
    /// </summary>
    /// <example></example>
    //后台所有权限集,用于验证URL权限
    [SugarColumn(ColumnDescription = "后台所有权限集", IsNullable = true, Length = 2000)]
    public string perms { get; set; }

    /// <summary>
    /// 前台菜单缓存
    /// </summary>
    /// <example></example>
    [SugarColumn(ColumnDescription = "前台菜单缓存", IsNullable = true, ColumnDataType = "varchar(max)")]
    public string menus { get; set; }

    /// <summary>
    /// 前台按钮缓存
    /// </summary>
    /// <example></example>
    [SugarColumn(ColumnDescription = "前台按钮缓存", IsNullable = true, ColumnDataType = "varchar(max)")]
    public string btns { get; set; }

    /// <summary>
    /// 前台门户缓存
    /// </summary>
    /// <example></example>
    [SugarColumn(ColumnDescription = "前台门户缓存", IsNullable = true, Length = 2000)]
    public string portals { get; set; }
}
namespace Vben.Base.Tool.Oss.Main;

[SugarTable("tool_oss_main", TableDescription = "OSS存储引用")]
public class ToolOssMain
{
    [SugarColumn(ColumnDescription = "主键", IsPrimaryKey = true, Length = 32)]
    public string id { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    [SugarColumn(ColumnDescription = "文件名称", IsNullable = true, Length = 255)]
    public string name { get; set; }

    /// <summary>
    /// 类型（后缀）
    /// </summary>
    [SugarColumn(ColumnDescription = "类型（后缀）", IsNullable = true, Length = 32)]
    public string type { get; set; }

    /// <summary>
    /// 文件ID
    /// </summary>
    [SugarColumn(ColumnDescription = "文件ID", IsNullable = true, Length = 32)]
    public string filid { get; set; }

    /// <summary>
    /// 业务ID
    /// </summary>
    [SugarColumn(ColumnDescription = "业务ID", IsNullable = true, Length = 32)]
    public string busid { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间", IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public DateTime? crtim { get; set; } = DateTime.Now;

    /// <summary>
    /// 创建者Id
    /// </summary>
    [SugarColumn(ColumnName = "crman", ColumnDescription = "创建者Id", IsNullable = true, IsOnlyIgnoreUpdate = true,
        Length = 36)]
    public virtual string crmid { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public virtual SysOrg crman { get; set; }

}
using Vben.Common.Core.Enum;

namespace Vben.Base.Mon.Log.Audit;

/// <summary>
/// 系统操作/审计日志表
/// </summary>
[SugarTable("mon_log_audit")]
[Description("系统操作/审计日志表")]
public class MonLogAudit
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true, Length = 36)]
    public string Id { get; set; }

    /// <summary>
    /// 表名
    /// </summary>
    [MaxLength(50)]
    [SugarColumn(ColumnDescription = "表名", IsNullable = true)]
    public string TableName { get; set; }

    /// <summary>
    /// 列名
    /// </summary>
    [MaxLength(50)]
    [SugarColumn(ColumnDescription = "列名", IsNullable = true)]
    public string ColumnName { get; set; }

    /// <summary>
    /// 新值
    /// </summary>
    [SugarColumn(ColumnDescription = "新值", IsNullable = true)]
    public string NewValue { get; set; }

    /// <summary>
    /// 旧值
    /// </summary>
    [SugarColumn(ColumnDescription = "旧值", IsNullable = true)]
    public string OldValue { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    [SugarColumn(ColumnDescription = "操作时间", IsNullable = true)]
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// 操作人Id
    /// </summary>
    [SugarColumn(ColumnDescription = "操作人Id", IsNullable = true)]
    public long UserId { get; set; }

    /// <summary>
    /// 操作人名称
    /// </summary>
    [MaxLength(20)]
    [SugarColumn(ColumnDescription = "操作人名称", IsNullable = true)]
    public string UserName { get; set; }

    /// <summary>
    /// 操作方式：新增、更新、删除
    /// </summary>
    [SugarColumn(ColumnDescription = "操作方式：新增、更新、删除", IsNullable = true)]
    public DataOpType Operate { get; set; }
}
namespace Vben.Base.Mon.Log.Error;

/// <summary>
/// 错误日志
/// </summary>
[SugarTable("mon_log_error")]
[Description("错误日志")]
public class MonLogError
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true, Length = 36)]
    public string id { get; set; }

    /// <summary>
    /// 操作名称
    /// </summary>
    [MaxLength(100)]
    [SugarColumn(ColumnDescription = "操作名称", IsNullable = true)]
    public string name { get; set; }


    /// <summary>
    /// 用户ID
    /// </summary>
    [MaxLength(32)]
    [SugarColumn(ColumnDescription = "用户ID", IsNullable = true)]
    public string useid { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    [MaxLength(32)]
    [SugarColumn(ColumnDescription = "用户姓名", IsNullable = true)]
    public string usena { get; set; }

    /// <summary>
    /// 用户账号
    /// </summary>
    [MaxLength(32)]
    [SugarColumn(ColumnDescription = "用户账号", IsNullable = true)]
    public string usnam { get; set; }

    /// <summary>
    /// 类名
    /// </summary>
    [MaxLength(100)]
    [SugarColumn(ColumnDescription = "类名", IsNullable = true)]
    public string clazz { get; set; }

    /// <summary>
    /// 方法名
    /// </summary>
    [MaxLength(100)]
    [SugarColumn(ColumnDescription = "方法名", IsNullable = true)]
    public string method { get; set; }


    /// <summary>
    /// 异常名称
    /// </summary>
    [SugarColumn(ColumnDescription = "异常名称", IsNullable = true)]
    public string ExceptionName { get; set; }

    /// <summary>
    /// 异常信息
    /// </summary>
    [SugarColumn(ColumnDescription = "异常信息", IsNullable = true)]
    public string ExceptionMsg { get; set; }

    /// <summary>
    /// 异常源
    /// </summary>
    [SugarColumn(ColumnDescription = "异常源", IsNullable = true)]
    public string ExceptionSource { get; set; }

    /// <summary>
    /// 堆栈信息
    /// </summary>
    [SugarColumn(ColumnDescription = "堆栈信息", ColumnDataType = "varchar(max)", IsNullable = true)]
    public string error { get; set; }

    /// <summary>
    /// 参数对象
    /// </summary>
    [SugarColumn(ColumnDescription = "参数对象", IsNullable = true)]
    public string param { get; set; }

    /// <summary>
    /// 异常时间
    /// </summary>
    [SugarColumn(ColumnDescription = "异常时间", IsNullable = true)]
    public DateTime crtim { get; set; }
}
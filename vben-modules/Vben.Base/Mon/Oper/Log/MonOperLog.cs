namespace Vben.Base.Mon.Oper.Log;

/// <summary>
/// 操作日志
/// </summary>
[SugarTable("mon_oper_log")]
[Description("操作日志")]
public class MonOperLog
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
    public string opmod { get; set; }
    
    /// <summary>
    /// 业务类型（0其它 1新增 2修改 3删除）
    /// </summary>
    [MaxLength(100)]
    [SugarColumn(ColumnDescription = "业务类型", IsNullable = true)]
    public int butyp { get; set; }


    /// <summary>
    /// 用户ID
    /// </summary>
    [MaxLength(32)]
    [SugarColumn(ColumnDescription = "用户ID", IsNullable = true)]
    public string opuid { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    [MaxLength(32)]
    [SugarColumn(ColumnDescription = "用户姓名", IsNullable = true)]
    public string opuna { get; set; }

    /// <summary>
    /// 用户账号
    /// </summary>
    [MaxLength(32)]
    [SugarColumn(ColumnDescription = "用户账号", IsNullable = true)]
    public string username { get; set; }

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
    public string remet { get; set; }

    /// <summary>
    /// 参数对象
    /// </summary>
    [SugarColumn(ColumnDescription = "参数对象", IsNullable = true)]
    public string repar { get; set; }

    /// <summary>
    /// 异常时间
    /// </summary>
    [SugarColumn(ColumnDescription = "操作时间", IsNullable = true)]
    public DateTime optim { get; set; }


    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    [SugarColumn(ColumnDescription = "耗时（毫秒）", IsNullable = true)]
    public long cotim { get; set; }


    /// <summary>
    /// IP地址
    /// </summary>
    [MaxLength(32)]
    public string opip { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [MaxLength(64)]
    [SugarColumn(ColumnDescription = "操作系统", IsNullable = true)]
    public string ageos { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [MaxLength(64)]
    [SugarColumn(ColumnDescription = "浏览器", IsNullable = true)]
    public string agbro { get; set; }

    /// <summary>
    /// 客户端详情
    /// </summary>
    [MaxLength(512)]
    [SugarColumn(ColumnDescription = "客户端详情", IsNullable = true)]
    public string agdet { get; set; }

    /// <summary>
    /// 成功标记
    /// </summary>
    [SugarColumn(ColumnDescription = "成功标记", IsNullable = true)]
    public bool sutag { get; set; }

}
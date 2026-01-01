namespace Vben.Base.Mon.Log.Login;

/// <summary>
/// 登录日志表
/// </summary>
[SugarTable("mon_login_log")]
[Description("登录日志")]
public class MonLoginLog : BaseEntity
{
    // /// <summary>
    // /// 主键ID
    // /// </summary>
    // [SugarColumn(ColumnDescription = "ID主键", IsPrimaryKey = true)]
    // public string id { get; set; }
    //
    // /// <summary>
    // /// 用户姓名
    // /// </summary>
    // [MaxLength(100)]
    // [SugarColumn(ColumnDescription = "用户姓名", IsNullable = true, Length = 32)]
    // public string name { get; set; }

    /// <summary>
    /// 用户账号
    /// </summary>
    [MaxLength(36)]
    [SugarColumn(ColumnDescription = "用户账号", IsNullable = true)]
    public string username { get; set; }


    /// <summary>
    /// IP地址
    /// </summary>
    [MaxLength(32)]
    public string loip { get; set; }

    /// <summary>
    /// 登录地点
    /// </summary>
    [MaxLength(100)]
    [SugarColumn(ColumnDescription = "登录地点", IsNullable = true)]
    public string loloc { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [MaxLength(64)]
    [SugarColumn(ColumnDescription = "操作系统", IsNullable = true)]
    public string os { get; set; }
    
    /// <summary>
    /// 客户端
    /// </summary>
    [MaxLength(36)]
    [SugarColumn(ColumnDescription = "客户端", IsNullable = true)]
    public string clkey { get; set; }
    
    /// <summary>
    /// 设备类型
    /// </summary>
    [MaxLength(36)]
    [SugarColumn(ColumnDescription = "设备类型", IsNullable = true)]
    public string detyp { get; set; }
    
    /// <summary>
    /// 登录状态
    /// </summary>
    [SugarColumn(ColumnDescription = "登录状态", IsNullable = true)]
    public bool sutag { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [MaxLength(64)]
    [SugarColumn(ColumnDescription = "浏览器", IsNullable = true)]
    public string browser { get; set; }

    /// <summary>
    /// 客户端详情
    /// </summary>
    [MaxLength(512)]
    [SugarColumn(ColumnDescription = "客户端详情", IsNullable = true)]
    public string agdet { get; set; }

    /// <summary>
    /// 登录时间
    /// </summary>
    [SugarColumn(ColumnDescription = "登录时间", IsNullable = true)]
    public DateTime lotim { get; set; }
}
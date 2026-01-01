namespace Vben.Base.Mon.Online.User;

/// <summary>
/// 在线用户
/// </summary>
[SugarTable("mon_online_user", "在线用户")]
public class MonOnlineUser 
{
    
    /// <summary>
    /// 雪花Id
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键Id", IsPrimaryKey = true, IsIdentity = false)]
    public long id { get; set; }
    
    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", IsOnlyIgnoreUpdate = true)]
    public long? tenid { get; set; }
    
    /// <summary>
    /// 连接Id
    /// </summary>
    [SugarColumn(ColumnDescription = "连接Id")]
    public string? conid { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "用户Id")]
    public string useid { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    [SugarColumn(ColumnDescription = "账号", Length = 32)]
    [Required, MaxLength(32)]
    public virtual string usena { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "真实姓名", Length = 32)]
    [MaxLength(32)]
    public string? nicna { get; set; }

    /// <summary>
    /// 连接时间
    /// </summary>
    [SugarColumn(ColumnDescription = "连接时间")]
    public DateTime? cotim { get; set; }

    /// <summary>
    /// 连接IP
    /// </summary>
    [SugarColumn(ColumnDescription = "连接IP", Length = 256)]
    [MaxLength(256)]
    public string? ip { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [SugarColumn(ColumnDescription = "浏览器", Length = 128)]
    [MaxLength(128)]
    public string? browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [SugarColumn(ColumnDescription = "操作系统", Length = 128)]
    [MaxLength(128)]
    public string? os { get; set; }
}
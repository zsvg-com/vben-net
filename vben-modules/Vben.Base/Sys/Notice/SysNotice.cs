namespace Vben.Base.Sys.Notice;


[SugarTable("sys_notice", TableDescription = "系统通知")]
[Description("系统通知")]
public class SysNotice
{
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public long id { get; set; }
    
    [SugarColumn(ColumnDescription = "公告标题", IsNullable = true, Length = 64)]
    public string name { get; set; }
    
    [SugarColumn(ColumnDescription = "公告内容", IsNullable = true, Length = 2000)]
    public string cont { get; set; }
    
    [SugarColumn(ColumnDescription = "公告类型（1通知 2公告）", IsNullable = true)]
    public int type { get; set; }
    
    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 255)]
    public string notes { get; set; }
    
    [SugarColumn(ColumnDescription = "创建时间", IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public DateTime? crtim { get; set; } = DateTime.Now;
    
    [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
    public DateTime? uptim { get; set; } = DateTime.Now;
    
    [SugarColumn(ColumnDescription = "是否可用", IsNullable = true)]
    public bool avtag { get; set; }
    
    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }
    
}
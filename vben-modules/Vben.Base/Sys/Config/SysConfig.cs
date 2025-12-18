namespace Vben.Base.Sys.Config;


[SugarTable("sys_config", TableDescription = "系统参数")]
[Description("系统参数")]
public class SysConfig
{
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public long id { get; set; }
    
    [SugarColumn(ColumnDescription = "参数名称", IsNullable = true, Length = 32)]
    public string name { get; set; }
    
    [SugarColumn(ColumnDescription = "参数键名", IsNullable = true, Length = 32)]
    public string kenam { get; set; }
    
    [SugarColumn(ColumnDescription = "参数键值", IsNullable = true, Length = 64)]
    public string keval { get; set; }
    
    [SugarColumn(ColumnDescription = "内置标记", IsNullable = true)]
    public bool intag { get; set; } 
    
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
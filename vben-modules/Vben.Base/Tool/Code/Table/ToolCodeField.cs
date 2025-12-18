namespace Vben.Base.Tool.Code.Table;

[SugarTable("tool_code_field", TableDescription = "代码生成-字段信息")]
[Description("代码生成-字段信息")]
public class ToolCodeField : BaseStrMainEntity
{
    
    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }

    [SugarColumn(ColumnDescription = "字段注释", IsNullable = true, Length = 64)]
    public string remark { get; set; }
    
    [SugarColumn(ColumnDescription = "字段类型", IsNullable = true, Length = 32)]
    public string type { get; set; }
    
    [SugarColumn(ColumnDescription = "表格ID", IsNullable = true, Length = 32)]
    public long tabid { get; set; } 
    
    [SugarColumn(ColumnDescription = "字段长度", IsNullable = true)]
    public int length { get; set; }
    
    
}
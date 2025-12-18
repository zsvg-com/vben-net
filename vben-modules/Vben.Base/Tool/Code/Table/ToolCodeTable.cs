namespace Vben.Base.Tool.Code.Table;

[SugarTable("tool_code_table", TableDescription = "代码生成-表信息")]
[Description("代码生成-表信息")]
public class ToolCodeTable : BaseMainEntity
{
    
    [SugarColumn(ColumnDescription = "实例类", IsNullable = true, Length = 64)]
    public string bunam { get; set; }
    
    [SugarColumn(ColumnDescription = "继承基类", IsNullable = true, Length = 64)]
    public string baent { get; set; } 
    
    [SugarColumn(IsIgnore = true)]
    public string baser { get; set; } 
    
    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }
    
    [SugarColumn(ColumnDescription = "表描述", IsNullable = true, Length = 64)]
    public string remark { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(ToolCodeField.tabid))]
    [SugarColumn(IsIgnore = true)]
    public List<ToolCodeField> fields { get; set; } = new List<ToolCodeField>();
    
    [SugarColumn(ColumnDescription = "所属门户ID", IsNullable = true, Length = 32)]
    public string porid { get; set; } 
    
    [SugarColumn(ColumnDescription = "上级菜单ID", IsNullable = true, Length = 32)]
    public string pmeid { get; set; } 
    
    [SugarColumn(ColumnDescription = "编辑页类型", IsNullable = true, Length = 32)]
    public string edtyp { get; set; } 
    
    [SugarColumn(ColumnDescription = "每行列数", IsNullable = true)]
    public int pecol { get; set; }

    /// <summary>
    /// 新增按钮
    /// </summary>
    [SugarColumn(ColumnDescription = "新增按钮", IsNullable = true)]
    public bool addbt { get; set; }
    
    /// <summary>
    /// 删除按钮
    /// </summary>
    [SugarColumn(ColumnDescription = "删除按钮", IsNullable = true)]
    public bool delbt { get; set; }
    
    /// <summary>
    /// 导入按钮
    /// </summary>
    [SugarColumn(ColumnDescription = "导入按钮", IsNullable = true)]
    public bool impbt { get; set; }
    
    /// <summary>
    /// 导出按钮
    /// </summary>
    [SugarColumn(ColumnDescription = "导出按钮", IsNullable = true)]
    public bool expbt { get; set; }
    
    [SugarColumn(ColumnDescription = "路由类型", IsNullable = true, Length = 32)]
    public string rotyp { get; set; } 
    
    [SugarColumn(ColumnDescription = "排序字段", IsNullable = true, Length = 32)]
    public string orfie { get; set; } 
    
    [SugarColumn(ColumnDescription = "orm类型", IsNullable = true, Length = 32)]
    public string ortyp { get; set; } 
    
   
}
namespace Vben.Base.Tool.Dict.Data;

[SugarTable("tool_dict_data", TableDescription = "字典数据")]
[Description("字典数据")]
public class ToolDictData : BaseStrMainEntity
{
    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }

    [SugarColumn(ColumnDescription = "字典ID", Length = 36)]
    public string dicid { get; set; }

    /// <summary>
    /// 数据标签
    /// </summary>
    [SugarColumn(ColumnDescription = "数据标签", Length = 36,IsNullable = true)]
    public string dalab { get; set; }
    
    /// <summary>
    /// 数据键值
    /// </summary>
    [SugarColumn(ColumnDescription = "字典ID", Length = 36,IsNullable = true)]
    public string daval { get; set; }
    
    
    /// <summary>
    /// 显示样式
    /// </summary>
    [SugarColumn(ColumnDescription = "显示样式", Length = 36,IsNullable = true)]
    public string shsty { get; set; }

}
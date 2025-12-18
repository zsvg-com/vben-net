namespace Vben.Base.Tool.Dict.Main;

[SugarTable("tool_dict_main", TableDescription = "字典信息")]
[Description("字典信息")]
public class ToolDictMain : BaseStrMainEntity
{
    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }

    [SugarColumn(ColumnDescription = "类型", IsNullable = true, Length = 32)]
    public string catid { get; set; }
    

    [SugarColumn(ColumnDescription = "字典代码", IsNullable = true, Length = 32)]
    public string code { get; set; }

}
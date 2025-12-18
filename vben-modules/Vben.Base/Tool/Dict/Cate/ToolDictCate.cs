namespace Vben.Base.Tool.Dict.Cate;

[SugarTable("tool_dict_cate", TableDescription = "字典分类")]
[Description("字典分类")]
public class ToolDictCate : BaseEntity
{
    [SugarColumn(ColumnDescription = "可用标记：1可用，0禁用", IsNullable = true)]
    public bool? avtag { get; set; }

    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }

    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 64)]
    public string notes { get; set; }

    [SugarColumn(ColumnDescription = "代码", Length = 32)]
    public string code { get; set; }
}
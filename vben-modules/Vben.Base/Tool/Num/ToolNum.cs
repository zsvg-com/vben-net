namespace Vben.Base.Tool.Num;

[SugarTable("tool_num", TableDescription = "编号工具")]
[Description("编号策略表")]
public class ToolNum : BaseEntity
{
    [SugarColumn(ColumnDescription = "编号标签", IsNullable = true, Length = 100)]
    public string label { get; set; }

    [SugarColumn(ColumnDescription = "编号生成模式", Length = 32)]
    public string numod { get; set; }

    [SugarColumn(ColumnDescription = "编号前缀", IsNullable = true, Length = 32)]
    public string nupre { get; set; }

    [SugarColumn(ColumnDescription = "判断标记", IsNullable = true)]
    public bool nflag { get; set; } = true;

    [SugarColumn(ColumnDescription = "下一个编号", Length = 8, IsNullable = true)]
    public string nunex { get; set; }

    [SugarColumn(ColumnDescription = "编号长度")]
    public int nulen { get; set; }

    [SugarColumn(ColumnDescription = "当前日期", Length = 8, IsNullable = true)]
    public string cudat { get; set; }

    [SugarColumn(ColumnDescription = "创建时间", IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public DateTime? crtim { get; set; } = DateTime.Now;

    [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
    public DateTime? uptim { get; set; }

    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 64)]
    public string notes { get; set; }
}
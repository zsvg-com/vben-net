namespace Vben.Base.Tool.Form;

[SugarTable("tool_form", TableDescription = "在线表单")]
[Description("在线表单")]
public class ToolForm : BaseMainEntity
{

    [SugarColumn(ColumnDescription = "表单规则", ColumnDataType = "varchar(max)", IsNullable = true)]
    public string frule { get; set; }
}
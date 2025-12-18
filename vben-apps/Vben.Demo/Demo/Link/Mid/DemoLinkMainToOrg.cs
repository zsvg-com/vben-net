namespace Vben.Admin.Demo.Link.Mid;

[SugarTable("demo_link_main_org", TableDescription = "中间关联表")]
public class DemoLinkMainToOrg
{
    [SugarColumn(ColumnDescription = "主表ID", IsNullable = true)]
    public long mid { get; set; }

    [SugarColumn(ColumnDescription = "组织架构ID", IsNullable = true, Length = 36)]
    public string oid { get; set; }
}
namespace Vben.Admin.Demo.Link.Item;

[SugarTable("demo_link_item", TableDescription = "关联子表")]
public class DemoLinkItem
{
    /// <summary>
    /// Id主键
    /// </summary>
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true, Length = 32)]
    public string id { get; set; }
    
    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", IsNullable = true, Length = 128)]
    public string name { get; set; }
    
    
    /// <summary>
    /// 主表ID
    /// </summary>
    [SugarColumn(ColumnDescription = "主表ID", IsNullable = true)]
    public long maiid { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 255)]
    public string notes { get; set; }

}


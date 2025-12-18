using Vben.Admin.Demo.Link.Item;
using Vben.Admin.Demo.Link.Mid;

namespace Vben.Admin.Demo.Link.Main;

/// <summary>
/// 关联主表
/// </summary>
[SugarTable("demo_link_main", TableDescription = "关联主表")]
[Description("关联主表")]
public class DemoLinkMain : BaseMainEntity
{
    
    /// <summary>
    /// 分类ID
    /// </summary>
    [SugarColumn(ColumnDescription = "分类ID", IsNullable = true)]
    public long catid { get; set; }
    
    /// <summary>
    /// 行项目列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(DemoLinkItem.maiid))]
    [SugarColumn(IsIgnore = true)]
    public List<DemoLinkItem> items { get; set; }
    
    /// <summary>
    /// 关联用户中间表
    /// </summary>
    [Navigate(typeof(DemoLinkMainToOrg), nameof(DemoLinkMainToOrg.mid), nameof(DemoLinkMainToOrg.oid))]
    public List<SysOrg> orgs { get; set; }

}
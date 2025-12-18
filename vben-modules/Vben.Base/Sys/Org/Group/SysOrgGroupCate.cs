namespace Vben.Base.Sys.Org.Group;


[SugarTable("sys_org_group_cate", TableDescription = "组织架构群组分类")]
public class SysOrgGroupCate : BaseStrCateEntity
{
    /// <summary>
    /// 子分类集合
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<SysOrgGroupCate> children { get; set; }

}
namespace Vben.Base.Sys.Group;


[SugarTable("sys_group_cate", TableDescription = "组织架构群组分类")]
public class SysGroupCate : BaseStrCateEntity
{
    /// <summary>
    /// 子分类集合
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<SysGroupCate> children { get; set; }

}
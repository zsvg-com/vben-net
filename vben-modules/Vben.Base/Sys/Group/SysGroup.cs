namespace Vben.Base.Sys.Group;

[SugarTable("sys_group", TableDescription = "组织架构-群组")]
[Description("组织架构-群组")]
public class SysGroup : BaseStrMainEntity
{
    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }

    [SugarColumn(ColumnDescription = "标签", IsNullable = true, Length = 32)]
    public string label { get; set; }

    [SugarColumn(ColumnDescription = "分类ID", IsNullable = true, Length = 36)]
    public string catid { get; set; }

    [Navigate(typeof(SysGroupOrg), nameof(SysGroupOrg.gid), nameof(SysGroupOrg.oid))]
    [SugarColumn(IsIgnore = true)] public List<SysOrg> members { get; set; }
}
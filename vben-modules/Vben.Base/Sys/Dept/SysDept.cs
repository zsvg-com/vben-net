namespace Vben.Base.Sys.Dept;

[SugarTable("sys_dept", TableDescription = "组织架构-部门")]
[Description("组织架构-部门")]
public class SysDept : BaseStrMainEntity
{
    [SugarColumn(ColumnDescription = "父ID", IsNullable = true, Length = 32)]
    public string pid { get; set; }
    
    [SugarColumn(ColumnDescription = "层级", IsNullable = true, Length = 512)]
    public string tier { get; set; }

    [SugarColumn(ColumnDescription = "标签", IsNullable = true, Length = 32)]
    public string label { get; set; }
    
    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }
    
    /**
     * 部门在整个组织架构sys_org表中类别为1，这个字段是进行部门细分的，比如分为集团、企业、机构、部门等
     */
    [SugarColumn(ColumnDescription = "部门类型", IsNullable = true)]
    public int type { get; set; }

    [SugarColumn(IsIgnore = true)]
    public List<SysDept> children { get; set; }
    
    [SugarColumn(ColumnDescription = "扩展字段1", IsNullable = true, Length = 32)]
    public string ex1 { get; set; }
    
    [SugarColumn(ColumnDescription = "扩展字段2", IsNullable = true, Length = 32)]
    public string ex2 { get; set; }
    
    [SugarColumn(ColumnDescription = "扩展字段3", IsNullable = true, Length = 32)]
    public string ex3 { get; set; }
    
    [SugarColumn(ColumnDescription = "扩展字段4", IsNullable = true, Length = 32)]
    public string ex4 { get; set; }
    
}
namespace Vben.Base.Sys.Rece;

[SugarTable("sys_rece", TableDescription = "组织架构-最近访问记录")]
[Description("组织架构-最近访问记录")]
public class SysRece
{
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true, Length = 36)]
    public string id { get; set; }

    [SugarColumn(ColumnDescription = "用户ID", Length = 36)]
    public string useid { get; set; }

    [SugarColumn(ColumnDescription = "组织架构ID", Length = 36)]
    public string oid { get; set; }

    [SugarColumn(ColumnDescription = "最近使用时间", IsNullable = true)]
    public virtual DateTime? uptim { get; set; }
}
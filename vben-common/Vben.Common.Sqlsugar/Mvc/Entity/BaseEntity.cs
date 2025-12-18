namespace Vben.Common.Sqlsugar.Mvc.Entity;

//简单Entity基类，仅提供id与name字段，如有租户需求，可加租户ID。
public abstract class BaseEntity
{
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true, Length = 36)]
    public virtual string id { get; set; }

    [SugarColumn(ColumnDescription = "名称", IsNullable = true, Length = 255)]
    public virtual string name { get; set; }
}
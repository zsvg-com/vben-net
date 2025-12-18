namespace Vben.Common.Sqlsugar.Mvc.Entity;

//主数据Entity基类，提供通用的字段
public abstract class BaseMainEntity
{
    /// <summary>
    /// 主键Id
    /// </summary>
    /// <example></example>
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public virtual long id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    /// <example>API测试</example>
    [SugarColumn(ColumnDescription = "名称", IsNullable = true, Length = 255)]
    public virtual string name { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间", IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public virtual DateTime? crtim { get; set; } = DateTime.Now;

    /// <summary>
    /// 创建者Id
    /// </summary>
    [SugarColumn(ColumnDescription = "创建人Id", IsNullable = true, IsOnlyIgnoreUpdate = true,Length = 36)]
    public virtual string cruid { get; set; }
    
    /// <summary>
    /// 创建者姓名
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public virtual string cruna { get; set; }
    
    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
    public virtual DateTime? uptim { get; set; }

    /// <summary>
    /// 修改人Id
    /// </summary>
    [SugarColumn(ColumnDescription = "修改人Id", IsNullable = true, Length = 36)]
    public virtual string upuid { get; set; }
    
    /// <summary>
    /// 修改人姓名
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public virtual string upuna { get; set; }

    /// <summary>
    /// 可用标记，1可用，0不可用
    /// </summary>
    [SugarColumn(ColumnDescription = "可用标记：1可用，0禁用", IsNullable = true)]
    public virtual bool avtag { get; set; } = true;

    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 255)]
    public virtual string notes { get; set; }


}
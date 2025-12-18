namespace Vben.Common.Sqlsugar.Mvc.Entity;

//分类Entity基类，可实现无限多级的树结构
public abstract class BaseCateEntity
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public virtual long id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", IsNullable = true, Length = 255)]
    public virtual string name { get; set; }

    /// <summary>
    /// 父ID
    /// </summary>
    [SugarColumn(ColumnDescription = "父ID", IsNullable = true)]
    public virtual long pid { get; set; }

    /// <summary>
    /// 层级信息
    /// </summary>
    [SugarColumn(ColumnDescription = "层级信息", IsNullable = true, Length = 512)]
    public virtual string tier { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间", IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public virtual DateTime? crtim { get; set; } = DateTime.Now;

    /// <summary>
    /// 创建者Id
    /// </summary>
    [SugarColumn(ColumnDescription = "创建者Id", IsNullable = true, IsOnlyIgnoreUpdate = true, Length = 36)]
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
    /// 修改者Id
    /// </summary>
    [SugarColumn(ColumnDescription = "修改者Id", IsNullable = true, Length = 36)]
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
    
    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 255)]
    public virtual string notes { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public virtual int ornum { get; set; }
}
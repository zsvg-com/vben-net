namespace Vben.Common.Sqlsugar.Mvc.Entity;

//主数据Entity基类，提供通用的字段
public abstract class BaseStrMainEntity
{
    /// <summary>
    /// 主键Id
    /// </summary>
    /// <example></example>
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true, Length = 36)]
    public virtual string id { get; set; }

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
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
    public virtual DateTime? uptim { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(cruid))]
    public virtual SysOrg crman { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    [SugarColumn(ColumnName = "cruid", ColumnDescription = "创建者Id", IsNullable = true, IsOnlyIgnoreUpdate = true,
        Length = 36)]
    public virtual string cruid { get; set; }

    /// <summary>
    /// 修改者
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(upuid))]
    public virtual SysOrg upman { get; set; }

    /// <summary>
    /// 修改者Id
    /// </summary>
    [SugarColumn(ColumnName = "upuid", ColumnDescription = "修改者Id", IsNullable = true, Length = 36)]
    public virtual string upuid { get; set; }

    /// <summary>
    /// 是否可用，1可用，0不可用
    /// </summary>
    [SugarColumn(ColumnDescription = "可用标记：1可用，0禁用", IsNullable = true)]
    public virtual bool avtag { get; set; } = true;

    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 255)]
    public virtual string notes { get; set; }


    // public virtual void Create()
    // {
    //     var userId = App.User.FindFirst(ClaimConst.CLAINM_USERID)?.Value;
    //     var userName = App.User.FindFirst(ClaimConst.CLAINM_ACCOUNT)?.Value;
    //     Id = YitIdHelper.NextId();
    //     CreatedTime = DateTime.Now;
    //     if (!string.IsNullOrEmpty(userId))
    //     {
    //         CreatedUserId = long.Parse(userId);
    //         CreatedUserName = userName;
    //     }
    // }
    //
    // public void Modify()
    // {
    //     var userId = App.User.FindFirst(ClaimConst.CLAINM_USERID)?.Value;
    //     var userName = App.User.FindFirst(ClaimConst.CLAINM_ACCOUNT)?.Value;
    //     UpdatedTime = DateTime.Now;
    //     if (!string.IsNullOrEmpty(userId))
    //     {
    //         UpdatedUserId = long.Parse(userId);
    //         UpdatedUserName = userName;
    //     }
    // }

    // public virtual string[] UpdateColumn()
    // {
    //     var result = new[] {nameof(UpdatedUserId), nameof(UpdatedUserName), nameof(UpdatedTime)};
    //     return result;
    // }

    // public virtual string[] FalseDeleteColumn()
    // {
    //     var updateColumn = UpdateColumn();
    //     var deleteColumn = new[] {nameof(IsDeleted)};
    //     var result = new string [updateColumn.Length + deleteColumn.Length];
    //     deleteColumn.CopyTo(result, 0);
    //     updateColumn.CopyTo(result, deleteColumn.Length);
    //     return result;
    // }
}

/// <summary>
/// 主键实体基类
/// </summary>
public abstract class PrimaryKeyEntity
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    // 注意是在这里定义你的公共实体
    public virtual string id { get; set; }
}
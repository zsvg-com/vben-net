using Vben.Common.Core.Enum;

namespace Vben.Base.Mon.Job.Main;

/// <summary>
/// 定时任务
/// </summary>
[SugarTable("mon_job_main")]
[Description("定时任务")]
public class MonJobMain : BaseStrMainEntity
{
    /// <summary>
    /// 任务code
    /// </summary>
    [SugarColumn(ColumnDescription = "任务代码", IsNullable = true)]
    public string code { get; set; }

    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }

    /// <summary>
    /// 请求url
    /// </summary>
    [MaxLength(200)]
    [SugarColumn(ColumnDescription = "请求url", IsNullable = true)]
    public string reurl { get; set; }


    /// <summary>
    /// 请求类型
    /// </summary>
    /// <example>2</example>
    [SugarColumn(ColumnDescription = "请求类型", IsNullable = true)]
    public RequestTypeEnum retyp { get; set; } = RequestTypeEnum.Post;

    /// <summary>
    /// Headers(可以包含如：Authorization授权认证)
    /// 格式：{"Authorization":"userpassword.."}
    /// </summary>
    [SugarColumn(ColumnDescription = "Headers", IsNullable = true)]
    public string rehea { get; set; }

    /// <summary>
    /// 请求参数（Post，Put请求用）
    /// </summary>
    [SugarColumn(ColumnDescription = "请求参数", IsNullable = true)]
    public string repar { get; set; }

    /// <summary>
    /// Cron表达式
    /// </summary>
    /// <example></example>
    [MaxLength(20)]
    [SugarColumn(ColumnDescription = "Cron表达式", IsNullable = true)]
    public string cron { get; set; }

    // /// <summary>
    // /// 执行类型(并行、列队)
    // /// </summary>
    // [SugarColumn(ColumnDescription = "执行类型", IsNullable = true)]
    // public SpareTimeExecuteTypes extyp { get; set; } = SpareTimeExecuteTypes.Parallel;
}
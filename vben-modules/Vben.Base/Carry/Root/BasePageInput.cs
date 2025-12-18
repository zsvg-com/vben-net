// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Dm.filter;
using Furion.DataValidation;

namespace Admin.NET.Core;

/// <summary>
/// 全局分页查询输入参数
/// </summary>
public class BasePageInput : BaseFilter
{
    /// <summary>
    /// 当前页码
    /// </summary>
    [DataValidation(ValidationTypes.Numeric)]
    public virtual int pageNum { get; set; } = 1;

    /// <summary>
    /// 页码容量
    /// </summary>
    //[Range(0, 100, ErrorMessage = "页码容量超过最大限制")]
    [DataValidation(ValidationTypes.Numeric)]
    public virtual int pageSize { get; set; } = 10;

    // /// <summary>
    // /// 排序字段
    // /// </summary>
    // public virtual string Field { get; set; }
    //
    // /// <summary>
    // /// 排序方向
    // /// </summary>
    // public virtual string Order { get; set; }
    //
    // /// <summary>
    // /// 降序排序
    // /// </summary>
    // public virtual string DescStr { get; set; } = "descending";
}

/// <summary>
/// 全局分页查询输入参数(带时间)
/// </summary>
public class BasePageTimeInput : BasePageInput
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}
// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace Admin.NET.Core;

/// <summary>
/// 消息类型枚举
/// </summary>
[Description("消息类型枚举")]
public enum MessageTypeEnum
{
    /// <summary>
    /// 普通信息
    /// </summary>
    [Description("消息"), Theme("info")]
    Info = 0,

    /// <summary>
    /// 成功提示
    /// </summary>
    [Description("成功"), Theme("success")]
    Success = 1,

    /// <summary>
    /// 警告提示
    /// </summary>
    [Description("警告"), Theme("warning")]
    Warning = 2,

    /// <summary>
    /// 错误提示
    /// </summary>
    [Description("错误"), Theme("danger")]
    Error = 3
}
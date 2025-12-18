// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace Admin.NET.Core;

/// <summary>
/// 数据脱敏特性（支持自定义脱敏位置和脱敏字符）
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class DataMaskAttribute : Attribute
{
    /// <summary>
    /// 脱敏起始位置（从0开始）
    /// </summary>
    private int StartIndex { get; }

    /// <summary>
    /// 脱敏长度
    /// </summary>
    private int Length { get; }

    /// <summary>
    /// 脱敏字符（默认*）
    /// </summary>
    private char MaskChar { get; set; } = '*';

    /// <summary>
    /// 是否保留原始长度（默认true）
    /// </summary>
    private bool KeepLength { get; set; } = true;

    public DataMaskAttribute(int startIndex, int length)
    {
        if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
        if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));

        StartIndex = startIndex;
        Length = length;
    }

    /// <summary>
    /// 执行脱敏处理
    /// </summary>
    public string Mask(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length <= StartIndex)
            return input;

        var maskedLength = Math.Min(Length, input.Length - StartIndex);
        var maskStr = new string(MaskChar, KeepLength ? maskedLength : Math.Min(4, maskedLength));

        return input.Substring(0, StartIndex) + maskStr +
               (StartIndex + maskedLength < input.Length ?
                   input.Substring(StartIndex + maskedLength) : "");
    }
}
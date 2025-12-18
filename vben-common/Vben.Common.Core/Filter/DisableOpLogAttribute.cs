namespace Vben.Common.Core.Filter;

/// <summary>
/// 禁用操作日志
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DisableOpLogAttribute : System.Attribute
{
}
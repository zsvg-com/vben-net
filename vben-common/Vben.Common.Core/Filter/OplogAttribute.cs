namespace Vben.Common.Core.Filter;

/// <summary>
/// 操作日志特性
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class OplogAttribute : System.Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public OplogAttribute()
    {

    }

    public string name { get; set; }
    
    public int type { get; set; }
    
    public string title { get; set; }

    public OplogAttribute(string name) => this.name = name;

}
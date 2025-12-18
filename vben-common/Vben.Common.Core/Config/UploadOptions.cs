namespace Vben.Common.Core.Config;

/// <summary>
/// 文件上传配置
/// </summary>
public sealed class UploadOptions
{
    /// <summary>
    /// 上传服务
    /// </summary>
    public string Service { get; set; }

    /// <summary>
    /// 上传地址
    /// </summary>
    public string Path { get; set; }


    /// <summary>
    /// 大小
    /// </summary>
    public long MaxSize { get; set; }

    /// <summary>
    /// 上传格式
    /// </summary>
    public List<string> ContentType { get; set; }

    //xtodo
    public string DrawPath { get; set; }
}
namespace Vben.Common.Core.Config;

/// <summary>
/// 文件存储配置
/// </summary>
public sealed class OssOptions 
{
    public string AccessKeyId { get; set; }

    public string AccessKeySecret { get; set; }

    public string Endpoint { get; set; }

    public string Bucket { get; set; }

}
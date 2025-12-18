using Aliyun.OSS;
using Aliyun.OSS.Common;

namespace Vben.Base.Tool.Oss.Root;

public class AliyunOssUtil
{

    /// <summary>
    /// 上传到阿里云
    /// </summary>
    /// <param name="filestreams"></param>
    /// <param name="dirPath">存储路径 eg： upload/2020/01/01/xxx.png</param>
    /// <param name="bucketName">存储桶 如果为空默认取配置文件</param>
    public static System.Net.HttpStatusCode PutObjectFromFile(Stream filestreams, string dirPath,
        string bucketName = "")
    {
        var option = MyApp.GetOptions<OssOptions>();
        OssClient client = new(option.Endpoint, option.AccessKeyId, option.AccessKeySecret);
        if (string.IsNullOrEmpty(bucketName))
        {
            bucketName = option.Bucket;
        }

        try
        {
            dirPath = dirPath.Replace("\\", "/");
            Console.WriteLine("上传path:" + dirPath);
            PutObjectResult putObjectResult = client.PutObject(bucketName, dirPath, filestreams);
            // Console.WriteLine("Put object:{0} succeeded", directory);

            return putObjectResult.HttpStatusCode;
        }
        catch (OssException ex)
        {
            Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed with error info: {0}", ex.Message);
        }

        return System.Net.HttpStatusCode.BadRequest;
    }

    public static OssObject GetObject(string path)
    {
        var option = MyApp.GetOptions<OssOptions>();
        OssClient client = new(option.Endpoint, option.AccessKeyId, option.AccessKeySecret);
        path = path.Replace("\\", "/");
        return client.GetObject(option.Bucket, path);

        // using (var requestStream = result.Content)
        // {
        //     using (fs = File.Open(path, FileMode.OpenOrCreate))
        //     {
        //         int length = 4 * 1024;
        //         var buf = new byte[length];
        //         do
        //         {
        //             length = requestStream.Read(buf, 0, length);
        //             fs.Write(buf, 0, length);
        //         } while (length != 0);
        //     }
        // }
        // return fs;
    }


    /// <summary>
    /// 删除资源
    /// </summary>
    /// <param name="dirPath"></param>
    /// <param name="bucketName"></param>
    /// <returns></returns>
    public static System.Net.HttpStatusCode DeleteFile(string dirPath, string bucketName = "")
    {
        var option = MyApp.GetOptions<OssOptions>();
        if (string.IsNullOrEmpty(bucketName))
        {
            bucketName = option.Bucket;
        }

        try
        {
            OssClient client = new(option.Endpoint, option.AccessKeyId, option.AccessKeySecret);
            DeleteObjectResult putObjectResult = client.DeleteObject(bucketName, dirPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return System.Net.HttpStatusCode.BadRequest;
    }
}
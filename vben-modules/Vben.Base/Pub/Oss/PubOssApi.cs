using Microsoft.AspNetCore.Authorization;
using Vben.Base.Tool.Oss.Main;
using Vben.Base.Tool.Oss.Root;

namespace Vben.Base.Pub.Oss;

[Route("pub/oss")]
[ApiDescriptionSettings("Pub", Tag = "文件存储")]
public class PubOssApi(ToolOssMainService ossService) : ControllerBase
{
    [HttpPost("upload")]
    // [NonUnify]
    public async Task<Zfile> PostUpload(IFormFile file)
    {
        var sysFile = await ossService.UploadFile(file);
        return sysFile;
    }

    [HttpGet("download")]
    public IActionResult GetDownload(string table, string id)
    {
        return ossService.DownloadFile(table, id);
    }

    [HttpGet("info")]
    public ToolOssMain GetInfo(string id)
    {
        return ossService.GetInfo(id);
    }

    [HttpGet("infos")]
    public List<ToolOssMain> GetInfos(string ids)
    {
        return ossService.GetInfos(ids);
    }
    
    [HttpGet("login2")]
    // [AllowAnonymous]
    public  Task Test()
    {
        return Task.FromResult(0);
    }

}
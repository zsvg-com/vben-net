using Vben.Base.Tool.Oss.File;
using Vben.Base.Tool.Oss.Root;

namespace Vben.Base.Tool.Oss.Main;

[Route("tool/oss/main")]
[ApiDescriptionSettings("Tool", Tag = "文件存储")]
public class ToolOssMainApi(ToolOssMainService service) : ControllerBase
{
    [HttpGet]
    [SaCheckPermission("tool:oss:query")]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await service._repo.Context.Queryable<ToolOssMain, ToolOssFile, SysOrg>
            ((t, f, o) => new JoinQueryInfos(JoinType.Left, f.id == t.filid,
                JoinType.Left, o.id == t.crmid))
            .Select((t, f, o) => new { t.id, t.name, t.type, t.crtim, f.service, f.path, crman = o.name,f.fsize })
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    [SaCheckPermission("tool:oss:query")]
    public async Task<ToolOssMain> GetInfo(string id)
    {
        var cate = await service.SingleAsync(id);
        return cate;
    }

    [HttpDelete("{ids}")]
    [SaCheckPermission("tool:oss:delete")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await service.DeleteAsync(idArr);
    }

    // [NonUnify]
    // [HttpPost("upload")]
    // public async Task<Zfile> PostUpload([FromBody] IFormFile file)
    // {
    //     Console.WriteLine(file);
    //     var sysFile = await service.UploadFile(file);
    //     return sysFile;
    // }
    
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [SaCheckPermission("tool:oss:upload")]
    public async Task<ToolOssUploadVo> PostUpload(IFormFile file)
    {
        Console.WriteLine(file);
        var sysFile = await service.UploadFile(file);
        ToolOssUploadVo uploadVo = new ToolOssUploadVo();
//        uploadVo.setUrl(oss.getUrl());
        uploadVo.fileName=sysFile.name;
        uploadVo.ossId = sysFile.filid;
        return uploadVo;
    }

    [HttpGet("download")]
    [SaCheckPermission("tool:oss:download")]
    public IActionResult GetDownload(string table, string id)
    {
        return service.DownloadFile(table, id);
    }

    // [AllowAnonymous]
    // [HttpGet("show")]
    // public IActionResult GetShow(string token, string id)
    // {
    //     var (isValid, tokenData, validationResult) = JWTEncryption.Validate(token.Substring(7));
    //     // Console.WriteLine(isValid);
    //     // Console.WriteLine(tokenData);
    //     // Console.WriteLine(validationResult);
    //     if (isValid)
    //     {
    //         return service.DownloadFile(null, id);
    //     }
    //     return null;
    // }
    
    [AllowAnonymous]
    [HttpGet("show")]
    public IActionResult GetShow(string id)
    {
        return service.DownloadFile(null, id);
    }
}
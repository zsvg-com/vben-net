using Vben.Base.Tool.Oss.File;
using Vben.Base.Tool.Oss.Root;

namespace Vben.Base.Tool.Oss.Main;

[Route("tool/oss/main")]
[ApiDescriptionSettings("Tool", Tag = "文件存储")]
public class ToolOssMainApi(ToolOssMainService service) : ControllerBase
{
    [HttpGet]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await service._repo.Context.Queryable<ToolOssMain, ToolOssFile, SysOrg>
            ((t, f, o) => new JoinQueryInfos(JoinType.Left, f.id == t.filid,
                JoinType.Left, o.id == t.crmid))
            .Select((t, f, o) => new { t.id, t.name, t.type, t.crtim, f.service, f.path, crman = o.name })
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("info/{id}")]
    public async Task<ToolOssMain> GetInfo(string id)
    {
        var cate = await service.SingleAsync(id);
        return cate;
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        await service.DeleteAsync(idArr);
    }

    // [NonUnify]
    [HttpPost("upload")]
    public async Task<Zfile> PostUpload([FromBody] IFormFile file)
    {
        Console.WriteLine(file);
        var sysFile = await service.UploadFile(file);
        return sysFile;
    }

    [HttpGet("download")]
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
}
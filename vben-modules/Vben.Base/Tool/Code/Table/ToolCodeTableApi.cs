using Microsoft.AspNetCore.Hosting;
using Vben.Base.Tool.Code.Root;
using Vben.Common.Core.Filter;

namespace Vben.Base.Tool.Code.Table;


[Route("tool/code/table")]
[ApiDescriptionSettings("Tool", Tag = "代码生成")]
public class ToolCodeTableApi : ControllerBase
{
    private readonly ToolCodeTableService _service;

    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public ToolCodeTableApi(ToolCodeTableService service,
        IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<dynamic> Get(string maiid, string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await _service.Repo.Context.Queryable<ToolCodeTable>()
            .LeftJoin<SysOrg>((t, c) => c.id == t.cruid)
            .LeftJoin<SysOrg>((t, c, u) => u.id == t.upuid)
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .Select((t, c, u) => new
            {
                t.id, t.name, t.crtim, t.uptim, t.notes,
                cruna = c.name, upuna = u.name
            })
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    [HttpGet("list")]
    public async Task<dynamic> GetList(string maiid)
    {
        return await _service.Repo.Context.Queryable<ToolCodeTable>().ToListAsync();
    }

    [HttpGet("zip")]
    public async Task<IActionResult> GetZip(long id)
    {
        GenerateDto dto = new GenerateDto();
        dto.GenTable = await _service.FindOne(id);
        if (!string.IsNullOrEmpty(dto.GenTable.baent))
        {
            dto.GenTable.baser = dto.GenTable.baent.Replace("Entity", "Service");
        }

        //自定义路径
        CodeGenConfig config = MyApp.GetOptions<CodeGenConfig>();
        // dto.ZipPath = Path.Combine(_webHostEnvironment.WebRootPath, "vms","zips");
        dto.ZipPath = Path.Combine(config.Path, "zips");
        dto.GenCodePath = Path.Combine(dto.ZipPath, DateTime.Now.ToString("yyyyMMdd"));

        string zipReturnFileName = $"vboot-{DateTime.Now:MMddHHmmss}.zip";

        //生成代码到指定文件夹
        CodeGeneratorTool.Generate(dto);
        FileHelper.ZipGenCode(dto.ZipPath, dto.GenCodePath, zipReturnFileName);
        // Console.WriteLine("path=" + dto.ZipPath);
        // Console.WriteLine("filename=" + zipReturnFileName);
        string zipFileFullName = Path.Combine(dto.ZipPath, zipReturnFileName);
        _httpContextAccessor.HttpContext.Response.Headers.Add("Content-Disposition", 
        $"attachment; download-filename={zipReturnFileName}");
        // _httpContextAccessor.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "download-filename");
        _httpContextAccessor.HttpContext.Response.Headers.Add("download-filename", zipReturnFileName);
        return new FileStreamResult(new FileStream(zipFileFullName, FileMode.Open), "application/octet-stream")
            {FileDownloadName = zipReturnFileName};
    }
    
    [HttpGet("show")]
    public async Task<List<GenCode>> GetShow(long id)
    {
        GenerateDto dto = new GenerateDto();
        dto.GenTable = await _service.FindOne(id);
        if (!string.IsNullOrEmpty(dto.GenTable.baent))
        {
            dto.GenTable.baser = dto.GenTable.baent.Replace("Entity", "Service");
        }
        CodeGenConfig config = MyApp.GetOptions<CodeGenConfig>();
        // dto.ZipPath = Path.Combine(_webHostEnvironment.WebRootPath,"vms", "zips");
        dto.ZipPath = Path.Combine(config.Path, "zips");
        dto.GenCodePath = Path.Combine(dto.ZipPath, DateTime.Now.ToString("yyyyMMdd"));

        CodeGeneratorTool.Generate(dto);
        return dto.GenCodes;
    }

    [HttpGet("info/{id}")]
    public async Task<ToolCodeTable> GetInfo(long id)
    {
        return await _service.FindOne(id);
    }

    [HttpPost]
    public async Task Post([FromBody] ToolCodeTable data)
    {
        await _service.Insertx(data);
    }

    [HttpPut]
    [MyUnitOfWork]
    public async Task Put([FromBody] ToolCodeTable data)
    {
        await _service.Updatex(data);
    }

    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        await _service.Delete(ids);
    }
}
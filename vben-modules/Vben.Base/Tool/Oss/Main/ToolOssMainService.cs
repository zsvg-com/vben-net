using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Vben.Base.Tool.Oss.File;
using Vben.Base.Tool.Oss.Root;
using Vben.Common.Core.Token;

namespace Vben.Base.Tool.Oss.Main;

[Service]
public class ToolOssMainService
{
    
    public byte[] ToByteArray(IFormFile formFile)
    {
        var fileLength = formFile.Length;
        using var stream = formFile.OpenReadStream();
        var bytes = new byte[fileLength];

        _ = stream.Read(bytes, 0, (int)fileLength);

        return bytes;
    }
    
    
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file">文件</param>
    /// <returns></returns>
    public async Task<Zfile> UploadFile(IFormFile file)
    {
        MD5 md5Provider = MD5.Create();
        byte[] retVal = md5Provider.ComputeHash(ToByteArray(file));
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < retVal.Length; i++)
        {
            sb.Append(retVal[i].ToString("x2"));
        }

        var md5 = sb.ToString();


        var dbFile = await _repo.Context.Queryable<ToolOssFile>()
            .Where(it => it.md5 == md5).FirstAsync();
        Zfile zfile = new Zfile();

        if (dbFile != null)
        {
            //相同文件已上传过
            ToolOssMain main = new ToolOssMain();
            main.id = YitIdHelper.NextId() + "";
            main.filid = dbFile.id;
            main.name = file.FileName;
            if (main.name.Contains("."))
            {
                main.type = main.name.Substring(main.name.LastIndexOf(".") + 1);
            }

            await _repo.InsertAsync(main);
            zfile.id = main.id;
            zfile.name = main.name;
            zfile.size = XfileUtil.GetFileSize(dbFile.fsize);
            zfile.path = dbFile.path;
            zfile.filid = dbFile.id;
        }
        else
        {
            ToolOssFile newFile = null;
            //上传到本地
            if (_appSettings.Upload.Service == "local")
            {
                newFile = await LocalUpload(file, md5);
            }
            else if (_appSettings.Upload.Service == "aliyun")
            {
                newFile = AliyunUpload(file, md5);
            }

            await _repo.Context.Insertable(newFile).ExecuteCommandAsync();
            ToolOssMain main = new ToolOssMain();
            main.id = newFile.id;
            main.filid = newFile.id;
            main.name = file.FileName;
            if (main.name.Contains("."))
            {
                main.type = main.name.Substring(main.name.LastIndexOf(".") + 1);
            }

            main.crmid = LoginHelper.UserId;
            await _repo.InsertAsync(main);
            zfile.id = main.id;
            zfile.name = main.name;
            zfile.size = XfileUtil.GetFileSize(newFile.fsize);
            zfile.path = newFile.path;
            zfile.filid = newFile.id;
        }

        return zfile;
    }

    public IActionResult DownloadFile(string table, string id)
    {
        string sql = "select t.path \"path\",t.name \"name\",f.service \"service\" from " + table + " t " +
                     "inner join tool_oss_file f on f.id=t.filid " +
                     "where t.id=@id";
        dynamic map = null;
        if (!string.IsNullOrEmpty(table))
        {
            map = _repo.Context.Ado.SqlQuerySingle<dynamic>(sql, new { id });
        }
        if (map == null)
        {
            sql = "select f.path \"path\",t.name \"name\",f.service \"service\" from tool_oss_main t " +
                  "inner join tool_oss_file f on f.id=t.filid " +
                  "where t.id=@id";
            map = _repo.Context.Ado.SqlQuerySingle<dynamic>(sql, new { id });
        }

        //if ("aliyun" == map.service)
        //{
        //    return AliyunDownload(map.name, map.path);
        //}

        return LocalDownload(map.name, map.path);
    }


    private IActionResult LocalDownload(string name, string path)
    {
        var downloadPath = Path.Combine(_appSettings.Upload.Path, path);
        // _httpContextAccessor.HttpContext.Response.Headers.Add("download-filename", HttpUtility.UrlEncode(name));
        return new FileStreamResult(new FileStream(downloadPath, FileMode.Open), "application/octet-stream")
        { FileDownloadName = name };
    }

    private IActionResult AliyunDownload(string name, string path)
    {
        // _httpContextAccessor.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "download-filename");
        // _httpContextAccessor.HttpContext.Response.Headers.Add("download-filename", HttpUtility.UrlEncode(name));
        return new FileStreamResult(AliyunOssUtil.GetObject(path).Content, "application/octet-stream")
        { FileDownloadName = name };
    }

    private async Task<ToolOssFile> LocalUpload(IFormFile file, String md5)
    {
        // string path = "upload/{yyyy}/{MM}/{dd}";
        string path = "{yyyy}/{MM}/{dd}";
        var reg = new Regex(@"(\{.+?})");
        var match = reg.Matches(path);
        match.ToList().ForEach(a =>
        {
            var str = DateTime.Now.ToString(a.ToString().Substring(1, a.Length - 2));
            path = path.Replace(a.ToString(), str);
        });

        // if (!_appSettings.Upload.ContentType.Contains(file.ContentType))
        //     throw Oops.Oh(ErrorCodeEnum.D8001);

        var sizeKb = (long)(file.Length / 1024.0); // 大小KB
        // if (sizeKb > _appSettings.Upload.MaxSize)
        //     throw Oops.Oh(ErrorCodeEnum.D8002);

        var suffix = Path.GetExtension(file.FileName).ToLower(); // 后缀
        if (suffix.Length > 0)
        {
            suffix = suffix.Substring(1);
        }

        // 先存库获取Id
        var id = YitIdHelper.NextId() + "";
        var finalName = id + "." + suffix; // 文件最终名称
        if (suffix == "")
        {
            finalName = id;
        }

        var filePath = Path.Combine(_appSettings.Upload.Path, path);
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        await using var fs = System.IO.File.Create(Path.Combine(filePath, finalName));
        await file.CopyToAsync(fs);
        var newFile = new ToolOssFile
        {
            id = id,
            md5 = md5,
            fsize = file.Length,
            service = "local",
            path = path + "/" + finalName
        };
        return newFile;
    }

    private ToolOssFile AliyunUpload(IFormFile file, String md5)
    {
        string path = "{yyyy}/{MM}/{dd}";
        var reg = new Regex(@"(\{.+?})");
        var match = reg.Matches(path);
        match.ToList().ForEach(a =>
        {
            var str = DateTime.Now.ToString(a.ToString().Substring(1, a.Length - 2));
            path = path.Replace(a.ToString(), str);
        });

        // if (!_appSettings.Upload.ContentType.Contains(file.ContentType))
        //     throw Oops.Oh(ErrorCodeEnum.D8001);

        var sizeKb = (long)(file.Length / 1024.0); // 大小KB
        // if (sizeKb > _appSettings.Upload.MaxSize)
        //     throw Oops.Oh(ErrorCodeEnum.D8002);

        var suffix = Path.GetExtension(file.FileName).ToLower(); // 后缀
        if (suffix.Length > 0)
        {
            suffix = suffix.Substring(1);
        }

        // 先存库获取Id
        var id = YitIdHelper.NextId() + "";
        var finalName = id + "." + suffix; // 文件最终名称
        if (suffix == "")
        {
            finalName = id;
        }

        HttpStatusCode statusCode = AliyunOssUtil.PutObjectFromFile(file.OpenReadStream(), path + "/" + finalName, "");
        if (statusCode == HttpStatusCode.OK)
        {
            var newFile = new ToolOssFile
            {
                id = id,
                md5 = md5,
                fsize = file.Length,
                service = "aliyun",
                path = path + "/" + finalName
            };
            return newFile;
        }
        else
        {
            return null;
        }
    }

    public ToolOssMain GetInfo(string id)
    {
        return _repo.GetById(id);
    }

    public List<ToolOssMain> GetInfos(string ids)
    {
        string[] arr = ids.Split(",");
        return _repo.GetList(t => arr.Contains(t.id));
    }


    // private readonly IHttpContextAccessor _httpContextAccessor;

    // private readonly UploadOptions _uploadOptions;

    public SqlSugarRepository<ToolOssMain> _repo { get; }
    
    private readonly AppSettings _appSettings;

    public ToolOssMainService(SqlSugarRepository<ToolOssMain> repo,
        // IHttpContextAccessor httpContextAccessor,
        IOptions<UploadOptions> uploadOptions,
        IOptions<AppSettings> appSettings)
    {
        _repo = repo;
        // _uploadOptions = uploadOptions.Value;
        _appSettings = appSettings.Value;
        // _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ToolOssMain> SingleAsync(string id)
    {
        return await _repo.GetSingleAsync(t => t.id == id);
    }

    public async Task DeleteAsync(string[] ids)
    {
        await _repo.Context.Deleteable<ToolOssMain>().In(ids).ExecuteCommandAsync();
    }
}
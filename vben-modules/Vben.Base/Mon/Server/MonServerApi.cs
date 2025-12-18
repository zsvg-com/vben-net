using Microsoft.AspNetCore.Hosting;

namespace Vben.Base.Mon.Server;

[Route("mon/server")]
[ApiDescriptionSettings("Mon", Tag = "服务监控")]
public class MonServerApi(
    IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment hostEnvironment)
    : ControllerBase
{
    [HttpGet]
    public dynamic Get()
    {
        //核心数
        int cpuNum = Environment.ProcessorCount;
        string computerName = Environment.MachineName;
        string osName = RuntimeInformation.OSDescription;
        string osArch = RuntimeInformation.OSArchitecture.ToString();
        string version = RuntimeInformation.FrameworkDescription;
        string appRAM = ((double)Process.GetCurrentProcess().WorkingSet64 / 1048576).ToString("N2") + " MB";
        string startTime = Process.GetCurrentProcess().StartTime.ToString("yyyy-MM-dd HH:mm:ss");
        string sysRunTime = ComputerHelper.GetRunTime();
        string serverIP = httpContextAccessor.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":"
            + httpContextAccessor.HttpContext.Connection.LocalPort;//获取服务器IP

        var programStartTime = Process.GetCurrentProcess().StartTime;
        string programRunTime = DateTimeHelper.FormatTime((DateTime.Now - programStartTime).TotalMilliseconds.ToString().Split('.')[0].ParseToLong());
        var data = new
        {
            cpu = ComputerHelper.GetComputerInfo(),
            disk = ComputerHelper.GetDiskInfos(),
            sys = new { cpuNum, computerName, osName, osArch, serverIP, runTime = sysRunTime },
            app = new
            {
                name = hostEnvironment.EnvironmentName,
                rootPath = hostEnvironment.ContentRootPath,
                webRootPath = hostEnvironment.WebRootPath,
                version,
                appRAM,
                startTime,
                runTime = programRunTime,
                host = serverIP
            },
        };
        return data;
    }
}
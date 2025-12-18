using Furion.Schedule;
using Microsoft.Extensions.Logging;

namespace Vben.Base.Mon.Job.Main;

[JobDetail("myjob2","1分钟执行一次的DEMO2")]  
[Minutely]
public class MyJob2 : IJob
{
    private readonly ILogger<MyJob2> _logger;
    public MyJob2(ILogger<MyJob2> logger)
    {
        _logger = logger;
    }

    public async Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {        
        _logger.LogInformation($"开始：{context}");
        await Task.Delay(5000, stoppingToken); //
        throw new Exception("假装出错");
        _logger.LogInformation($"结束：{context}");
    }
}
using Furion.Schedule;
using Furion.TimeCrontab;
using Microsoft.Extensions.Logging;

namespace Vben.Base.Mon.Job.Main;

[JobDetail("myjob","30秒钟执行一次的DEMO1")]  
[Cron("*/30 * * * * *", CronStringFormat.WithSeconds)]
public class MyJob : IJob
{
    private readonly ILogger<MyJob> _logger;
    public MyJob(ILogger<MyJob> logger)
    {
        _logger = logger;
    }

    public Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{context}");
        return Task.CompletedTask;
    }
}
namespace Vben.Base.Mon.Job.Main;

[Route("mon/job/main")]
[ApiDescriptionSettings("Mon", Tag = "定时任务-清单")]
public class MonJobMainApi(MonJobMainService monJobMainService) : ControllerBase
{
    [HttpGet]
    public async Task<dynamic> Get(string name)
    {
        var pp = XreqUtil.GetPp();
        var items = await monJobMainService.repo.Context.Queryable<MonJobMain>()
            .WhereIF(!string.IsNullOrWhiteSpace(name), t => t.name.Contains(name.Trim()))
            .Select((t) => new {t.id, t.name, t.crtim, t.uptim, t.code, t.reurl, t.avtag, t.cron})
            .ToPageListAsync(pp.page, pp.pageSize, pp.total);
        return RestPageResult.Build(pp.total.Value, items);
    }

    /// <summary>
    /// 查看任务
    /// </summary>
    /// 
    [HttpGet("info/{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        return await monJobMainService.SingleAsync(id);
    }

    /// <summary>
    /// 增加任务
    /// </summary>
    // public async Task Post(MonJobMain job)
    // {
    //     await _service.InsertAsync(job);
    //     if (job.avtag)
    //     {
    //         bool runOk = _service.AddTimerJob(job, false);
    //         if (!runOk)
    //         {
    //             throw Oops.Oh($"定时任务委托创建失败！JobCode:{job.code}");
    //         }
    //     }
    // }

    /// <summary>
    /// 任务初始化
    /// </summary>
    [HttpPost("init")]
    public void PostInit()
    {
        
         // _service.StartAllJob();
    }

    /// <summary>
    /// 修改任务
    /// </summary>
    // public async Task Put(MonJobMain job)
    // {
    //     var dbJob = await _service.SingleAsync(job.id);
    //     // 先从调度器里取消
    //     if (dbJob.avtag)
    //     {
    //         SpareTime.Cancel(dbJob.code);
    //     }
    //
    //     await _service.UpdateAsync(job);
    //     // 再添加到任务调度里
    //     if (job.avtag)
    //     {
    //         bool runOk = _service.AddTimerJob(job, false);
    //         if (!runOk)
    //         {
    //             throw Oops.Oh($"定时任务委托创建失败！JobCode:{job.code}");
    //         }
    //     }
    // }

    /// <summary>
    /// 删除任务
    /// </summary>
    // public async Task Delete(string ids)
    // {
    //     var idArr = ids.Split(",");
    //     foreach (var id in idArr)
    //     {
    //         MonJobMain job = await _service.SingleAsync(id);
    //         await _service.DeleteAsync(id);
    //         SpareTime.Cancel(job.code);
    //     }
    // }

    /// <summary>
    /// 启动任务
    /// </summary>
    [HttpPost("start")]
    public async Task PostStart(string ids)
    {
        var idArr = ids.Split(",");
        foreach (var id in idArr)
        {
            var job = await monJobMainService.SingleAsync(id);
            job.avtag = true;
            await monJobMainService.UpdateAsync(job);
            monJobMainService.Start(job.code);

            // var timer = SpareTime.GetWorkers().ToList().Find(u => u.WorkerName == job.code);
            // if (timer == null)
            // {
            //     bool runOk = _service.AddTimerJob(job, false);
            //     if (!runOk)
            //     {
            //         throw Oops.Oh($"定时任务委托创建失败！JobCode:{job.code}");
            //     }
            // }
            // else
            // {
            //     SpareTime.Start(job.code);
            // }
        }
    }

    /// <summary>
    /// 停止任务
    /// </summary>
    [HttpPost("stop")]
    public async Task PostStop(string ids)
    {
        var idArr = ids.Split(",");
        foreach (var id in idArr)
        {
            var job = await monJobMainService.SingleAsync(id);
            job.avtag = false;
            await monJobMainService.UpdateAsync(job);
            monJobMainService.Stop(job.code);
        }
    }

    /// <summary>
    /// 立即执行一次
    /// </summary>
    
    [HttpPost("once")]
    public async Task PostOnce(string id)
    {
        var job = await monJobMainService.SingleAsync(id);
        if (job == null)
            // throw Oops.Oh(ErrorCode.D1002);
            throw new Exception("记录不存在");
        // job.code = YitIdHelper.NextId() + "";
        monJobMainService.runOnce(job.code);
        
        // bool runOk = _service.runOnce(job, true);
        // if (!runOk)
        // {
        //     throw Oops.Oh($"定时任务委托创建失败！JobCode:{job.code}");
        // }
    }
}
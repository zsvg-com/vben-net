using Furion.Schedule;
using Vben.Common.Core.Enum;

namespace Vben.Base.Mon.Job.Main;

[Service]
public class MonJobMainService : BaseStrMainService<MonJobMain>
{

    private readonly ISchedulerFactory _schedulerFactory;


    public MonJobMainService(
        SqlSugarRepository<MonJobMain> repo,
        ISchedulerFactory schedulerFactory
    )
    {
        this.repo = repo;
        _schedulerFactory = schedulerFactory;
    }


    // 新增定时任务
    public bool AddJob(MonJobMain job, bool doOnce)
    {
        _schedulerFactory.AddJob<MyJob>("test", Triggers.Secondly());
        
        // Action<SpareTimer, long> action = null;
        //
        // switch (job.retyp)
        // {
        //     // 创建本地方法委托
        //     case RequestTypeEnum.Run:
        //     {
        //         // 查询符合条件的任务方法
        //         var taskMethod = GetTaskMethods().FirstOrDefault(m => m.RequestUrl == job.reurl);
        //         if (taskMethod == null) break;
        //         // 创建任务对象
        //         var typeInstance = Activator.CreateInstance(taskMethod.DeclaringType);
        //         action = (Action<SpareTimer, long>)Delegate.CreateDelegate(typeof(Action<SpareTimer, long>),
        //             typeInstance, taskMethod.MethodName);
        //         break;
        //     }
        //     // 创建网络任务委托
        //     default:
        //     {
        //         action = async (_, _) =>
        //         {
        //             var requestUrl = job.reurl.Trim();
        //             requestUrl = requestUrl?.IndexOf("http") == 0 ? requestUrl : "http://" + requestUrl;
        //             var requestParameters = job.repar;
        //             var headersString = job.rehea;
        //             var headers = string.IsNullOrEmpty(headersString)
        //                 ? null
        //                 : JSON.Deserialize<Dictionary<string, string>>(headersString);
        //
        //             switch (job.retyp)
        //             {
        //                 case RequestTypeEnum.Get:
        //                     await requestUrl.SetHeaders(headers).GetAsync();
        //                     break;
        //                 case RequestTypeEnum.Post:
        //                     await requestUrl.SetHeaders(headers).SetQueries(requestParameters).PostAsync();
        //                     break;
        //                 case RequestTypeEnum.Put:
        //                     await requestUrl.SetHeaders(headers).SetQueries(requestParameters).PutAsync();
        //                     break;
        //                 case RequestTypeEnum.Delete:
        //                     await requestUrl.SetHeaders(headers).DeleteAsync();
        //                     break;
        //             }
        //         };
        //         break;
        //     }
        // }
        //
        // if (action == null)
        // {
        //     return false;
        // }
        //
        // // 缓存任务配置参数，以供任务运行时读取
        // if (job.retyp == RequestTypeEnum.Run)
        // {
        //     var jobParametersCode = $"{job.code}_Parameters";
        //     var jobParameters = _cache.Exists(jobParametersCode);
        //     var requestParametersIsNull = string.IsNullOrEmpty(job.repar);
        //
        //     // 如果没有任务配置却又存在缓存，则删除缓存
        //     if (requestParametersIsNull && jobParameters)
        //         _cache.Del(jobParametersCode);
        //     else if (!requestParametersIsNull)
        //         _cache.Set(jobParametersCode, JSON.Deserialize<Dictionary<string, string>>(job.repar));
        // }
        //
        // // 创建定时任务
        // if (doOnce)
        // {
        //     SpareTime.DoOnce(1000, action, job.code, job.notes, true, executeType: job.extyp);
        // }
        // else
        // {
        //     SpareTime.Do(job.cron, action, job.code, job.notes, job.avtag, executeType: job.extyp);
        // }

        return true;
    }

    //获取所有本地任务
    // public IEnumerable<TaskMethodInfo> GetTaskMethods()
    // {
    //     // 有缓存就返回缓存
    //     var taskMethods =  _cache.Get<IEnumerable<IJob>>("TaskMethodInfos");
    //     if (taskMethods != null) return taskMethods;
    //
    //     // 获取所有本地任务方法，必须有spareTimeAttribute特性
    //     taskMethods = App.EffectiveTypes
    //         .Where(u => u.IsClass && !u.IsInterface && !u.IsAbstract &&
    //                     typeof(IJob).IsAssignableFrom(u))
    //         .SelectMany(u => u.GetMethods(BindingFlags.Public | BindingFlags.Instance)
    //             .Where(m => m.IsDefined(typeof(SpareTimeAttribute), false) &&
    //                         m.GetParameters().Length == 2 &&
    //                         m.GetParameters()[0].ParameterType == typeof(SpareTimer) &&
    //                         m.GetParameters()[1].ParameterType == typeof(long) && m.ReturnType == typeof(void))
    //             .Select(m =>
    //             {
    //                 // 默认获取第一条任务特性
    //                 var spareTimeAttribute = m.GetCustomAttribute<SpareTimeAttribute>();
    //                 return new TaskMethodInfo
    //                 {
    //                     JobName = spareTimeAttribute.WorkerName,
    //                     RequestUrl = $"{m.DeclaringType.Name}/{m.Name}",
    //                     Cron = spareTimeAttribute.CronExpression,
    //                     DoOnce = spareTimeAttribute.DoOnce,
    //                     ExecuteType = spareTimeAttribute.ExecuteType,
    //                     Interval = (int) spareTimeAttribute.Interval / 1000,
    //                     StartNow = spareTimeAttribute.StartNow,
    //                     RequestType = RequestTypeEnum.Run,
    //                     Remark = spareTimeAttribute.Description,
    //                     TimerType = string.IsNullOrEmpty(spareTimeAttribute.CronExpression)
    //                         ? SpareTimeTypes.Interval
    //                         : SpareTimeTypes.Cron,
    //                     MethodName = m.Name,
    //                     DeclaringType = m.DeclaringType
    //                 };
    //             }));
    //
    //     _cache.Set("TaskMethodInfos", taskMethods);
    //     return taskMethods;
    // }


    // 启动自启动任务
     // public void StartAllJob()
     // {
     //     // var jobMethodList =await GetTaskMethods();
     //     var jobMethodList = GetTaskMethods();
     //     foreach (var jobMethod in jobMethodList)
     //     {
     //         bool isAny = repo.IsAny(t => t.code == jobMethod.JobName);
     //         if (!isAny)
     //         {
     //             MonJobMain job = new MonJobMain();
     //             if (jobMethod.Remark.Contains("###"))
     //             {
     //                 job.name = jobMethod.Remark.Split("###")[0];
     //             }
     //             else
     //             {
     //                 job.name = jobMethod.JobName;
     //             }
     //
     //             job.code = jobMethod.JobName;
     //             job.avtag = false;
     //             job.cron = jobMethod.Cron;
     //             job.notes = jobMethod.Remark;
     //             job.reurl = jobMethod.RequestUrl;
     //             job.retyp = RequestTypeEnum.Run;
     //             job.crtim = DateTime.Now;
     //             job.id = YitIdHelper.NextId() + "";
     //             job.extyp = jobMethod.ExecuteType;
     //             repo.Insert(job);
     //         }
     //     }
     //
     //     var jobList = repo.GetList(t => t.avtag);
     //     foreach (var job in jobList)
     //     {
     //         _schedulerFactory.AddJob<MyJob>("test", Triggers.Secondly());
     //         // AddTimerJob(job, false);
     //     }
     // }

     public async Task StartAllJobs()
     {
         //获取所有代码定义的作业
         var allJobs = App.EffectiveTypes.ScanToBuilders().ToList();
         
         List<MonJobMain> dbJobList= await repo.GetListAsync();
         List<SchedulerBuilder> canRunList= new List<SchedulerBuilder>();
         foreach (var schedulerBuilder in allJobs)
         {
             bool flag=false;
             foreach (var dbJob in dbJobList)
             {
                 if (dbJob.code == schedulerBuilder.GetJobBuilder().JobId)
                 {
                     flag = true;
                     if (dbJob.cron.Contains("]"))
                     {
                         var cron = dbJob.cron.Split("[\"")[1].Split("\"")[0];
                         var format = dbJob.cron.Split("[\"")[1].Split(",")[1].Split("]")[0];
                         schedulerBuilder.GetTriggerBuilders().First().AlterToCron(cron,int.Parse(format));
                     }
                     else
                     {
                         schedulerBuilder.GetTriggerBuilders().First().AlterToCron(dbJob.cron);
                     }

                     if (dbJob.avtag)
                     {
                         canRunList.Add(schedulerBuilder);
                     }
                     break;
                 }
             }
             if (!flag)
             {
                 MonJobMain job = new MonJobMain();
                 job.code = schedulerBuilder.GetJobBuilder().JobId;
                 job.avtag = false;
                 job.cron = schedulerBuilder.GetTriggerBuilders().First().Args;
                 job.name =  schedulerBuilder.GetJobBuilder().Description;
                 job.reurl = schedulerBuilder.GetJobBuilder().JobType;
                 job.retyp = RequestTypeEnum.Run;
                 job.crtim = DateTime.Now;
                 job.id = YitIdHelper.NextId() + "";
                 // job.extyp = schedulerBuilder.GetJobBuilder().JobType;
                 await repo.InsertAsync(job);
             }
         }
         
         //添加作业
         foreach (var schedulerBuilder in canRunList)
         {
             Console.WriteLine(schedulerBuilder.ConvertToJSON());
             Console.WriteLine(schedulerBuilder.GetTriggerBuilders().First().ConvertToJSON());
             Console.WriteLine(schedulerBuilder.GetJobBuilder().ConvertToJSON());
             Console.WriteLine(schedulerBuilder.GetEnumerable());
             _schedulerFactory.AddJob(schedulerBuilder);
         }
         
         //启动作业
         var list=_schedulerFactory.GetJobs();
         foreach (var scheduler in list)
         {
             _schedulerFactory.StartJob(scheduler);
         }
     }

     public void runOnce(string jobId)
     {
         var schedulerBuilder = _schedulerFactory.GetJob(jobId);
         if (schedulerBuilder != null)
         {
             _schedulerFactory.RunJob(jobId);
         }
         else
         {
             Console.WriteLine("进来了");
             var allJobs = App.EffectiveTypes.ScanToBuilders().ToList();
             foreach (var job in allJobs)
             {
                 if (job.GetJobBuilder().JobId == jobId)
                 {
                     _schedulerFactory.AddJob(job);
                     break;
                 }
             }
             _schedulerFactory.RunJob(jobId);
             Thread.Sleep(1000);
             _schedulerFactory.RemoveJob(jobId);
         }
     }
     
     public void Start(string jobId)
     {
         var allJobs = App.EffectiveTypes.ScanToBuilders().ToList();
         foreach (var job in allJobs)
         {
             if (job.GetJobBuilder().JobId == jobId)
             {
                 _schedulerFactory.AddJob(job);
                 break;
             }
         }
         _schedulerFactory.StartJob(jobId);
     }
     
     public void Stop(string jobId)
     {
         _schedulerFactory.RemoveJob(jobId);
     }
}
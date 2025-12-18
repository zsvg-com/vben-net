//using Furion.TaskScheduler;

//namespace Vben.Common.Core.Module.Mon;

///// <summary>
///// 定时任务测试类
///// </summary>
//public class MonJobDemo : ISpareTimeWorker
//{
//    [SpareTime("*/20 * * * * *", "DemoTest1",
//        Description = "20秒一次的测试###后台批量写错误日志，配置项参数：{\"quantity\": 2}，不填默认为2",
//        DoOnce = false, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
//    public void Test1(SpareTimer timer, long count)
//    {
//        MonJobLog log = new MonJobLog();
//        log.id = YitIdHelper.NextId() + "";
//        log.name = "20秒一次的测试";
//        log.sttim = DateTime.Now;
//        log.msg = "运行中";
//        // 判断是否有异常
//        if (timer.Exception.Any())
//        {
//            Console.WriteLine("这里一直没有到过");
//            //好像没有捕获到异常
//            Console.WriteLine(timer.Exception.Values.LastOrDefault()?.Message);
//        }

//        Scoped.Create((_, scope) =>
//        {
//            var services = scope.ServiceProvider;
//            var db = App.GetService<ISqlSugarClient>(services);
//            db.Insertable(log).ExecuteCommand();
//            try
//            {
//                int i = 0;
//                Console.WriteLine("20秒一次的测试" + DateTime.Now);
//                int j = 3 / i; //过意抛异常
//                Thread.Sleep(10000);
//                log.entim = DateTime.Now;
//                log.ret = "成功";
//                log.msg = "用时：" + log.entim.Subtract(log.sttim).Seconds + "秒";
//                db.Updateable(log).ExecuteCommand();
//            }
//            catch (Exception e)
//            {
//                log.entim = DateTime.Now;
//                log.ret = "失败";
//                log.msg = e.Message;
//                db.Updateable(log).ExecuteCommand();
//                Console.WriteLine(e);
//                throw;
//            }
//        });
//    }


//    [SpareTime("* * * * *", "DemoTest2",
//        Description = "1分钟一次的测试###后台批量写错误日志，配置项参数：{\"quantity\": 2}，不填默认为2",
//        DoOnce = false, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
//    public void Test2(SpareTimer timer, long count)
//    {
//        Console.WriteLine("1分钟一次的测试" + DateTime.Now);
//    }
//}
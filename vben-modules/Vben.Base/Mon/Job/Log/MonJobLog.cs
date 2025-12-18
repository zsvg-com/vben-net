namespace Vben.Base.Mon.Job.Log;

[SugarTable("mon_job_log")]
[Description("定时任务日志")]
public class MonJobLog
{
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true, Length = 36)]
    public string id { get; set; }

    [SugarColumn(ColumnDescription = "名称", IsNullable = true, Length = 255)]
    public string name { get; set; }

    [SugarColumn(ColumnDescription = "信息", IsNullable = true, Length = 2000)]
    public string msg { get; set; }

    [SugarColumn(ColumnDescription = "结果", IsNullable = true, Length = 255)]
    public string ret { get; set; }

    [SugarColumn(ColumnDescription = "开始时间", IsNullable = true)]
    public DateTime sttim { get; set; }

    [SugarColumn(ColumnDescription = "结束时间", IsNullable = true)]
    public DateTime entim { get; set; }
    
    
}
namespace Vben.Common.Sqlsugar.Mvc.Pojo;

//基础pojo，只有id与name属性
public class SidName
{
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public string id { get; set; }

    public string name { get; set; }
}